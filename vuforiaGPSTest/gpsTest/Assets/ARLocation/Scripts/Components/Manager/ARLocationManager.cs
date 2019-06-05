// Copyright (C) 2018 Daniel Fortes <daniel.fortes@gmail.com>
// All rights reserved.
//
// See LICENSE.TXT for more info


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if !ARGPS_USE_VUFORIA
using UnityEngine.XR.ARFoundation;
#endif


/// <summary>
/// This Component manages all positioned GameObjects, synchronizing their world position in the scene
/// with their geographical coordinates. This is done by calculating their position relative to the device's position.
/// 
/// Should be placed in a GameObject called "ARLocationRoot", whose parent is the "AR Session Origin".
/// </summary>
public class ARLocationManager : Singleton<ARLocationManager>
{
    /// <summary>
    /// An array describing a set of objects to be placed on the scene via GPS/geolocation
    /// coordinates.
    /// </summary>
    [Tooltip("An array describing a set of objects to be placed on the scene via GPS/geolocation.")]
    public ARLocationManagerEntry[] objects;

    /// <summary>
    /// The ar session reset distance.
    /// </summary>
    [Tooltip("Distance that the user can move away from the initial position before the" +
             "AR Session is reset/refreshed. This is useful if the content's alignment " +
             "is large due to true-north error (which increases with distance). " +
             "A zero value means disabled.")]
    public float arSessionResetDistance = 20.0f;

    /// <summary>
    /// A 2D screen-space canvas to fill the screen while the AR Session is being reset (when arSessionResetDistance > 0)
    /// </summary>
    [Tooltip("A 2D screen-space canvas to fill the screen while the AR Session is being reset (when arSessionResetDistance > 0).")]
    public GameObject resetWaitScreen;

    /// <summary>
    /// A delegate that is called when a new object is addded to the manager.
    /// </summary>
    public delegate void OnObjectAddedDelegate(ARLocationManagerEntry entry);

    /// <summary>
    /// A delegate that is called when a new object is removed from the manager.
    /// </summary>
    /// <param name="entry"></param>
    public delegate void OnObjectRemovedDelegate(ARLocationManagerEntry entry);

    /// <summary>
    /// A delegate that is called when the manager is restarted.
    /// </summary>
    public delegate void OnRestartDelegate();

    /// <summary>
    /// Called when the manager has started and objects can be added.
    /// </summary>
    public delegate void OnStartDelegate();

    Dictionary<Guid, ARLocationManagerEntry> entries = new Dictionary<Guid, ARLocationManagerEntry>();

    OnObjectAddedDelegate onObjectAddedDelegates;
    OnObjectRemovedDelegate onObjectRemovedDelegates;

    OnStartDelegate onStartDelegate;

    OnRestartDelegate onRestartDelegates;

    ARLocationProvider locationProvider;

    private bool hasStarted = false;

    private bool waitingReset = false;

    private GameObject waitScreen;

    private ARLocationPlaneManager arLocationplaneManager;

    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60;

        // Find the LocationProvider
        locationProvider = ARLocationProvider.Instance;
        if (locationProvider == null)
        {
            Debug.LogError("[ARFoundation+GPSLocation][ARLocatedObjectsManager]: LocationProvider GameObject or Component not found.");
            return;
        }

        // Add the initially set objects
        foreach (var entry in objects)
        {
            Add(entry);
        }

        // Register callback for handling location updates
        locationProvider.OnLocationUpdated(HandleLocationUpdatedDelegate);

        // Register callback for handling new camera frames
#if !ARGPS_USE_VUFORIA
        var arPlaneManager = GameObject.FindObjectOfType<ARPlaneManager>();

        if (arPlaneManager != null)
        {
            arLocationplaneManager = new ARLocationPlaneManager(GameObject.FindObjectOfType<ARPlaneManager>());
        }
#else
        var planeFinder = GameObject.FindObjectOfType<Vuforia.PlaneFinderBehaviour>();

        if (planeFinder != null)
        {
            arLocationplaneManager = new ARLocationPlaneManager(planeFinder);
        }
#endif
        // Cart onStart delegates
        hasStarted = true;
        if (onStartDelegate != null)
        {
            onStartDelegate();
        }
    }

    /// <summary>
    /// Ons the object added.
    /// </summary>
    /// <param name="del">Del.</param>
    public void OnObjectAdded(OnObjectAddedDelegate del)
    {
        onObjectAddedDelegates += del;
    }

    public void OnObjectRemoved(OnObjectRemovedDelegate del)
    {
        onObjectRemovedDelegates += del;
    }

    /// <summary>
    /// Adds a delegate to be called when the ARLocationManager session is restarted.
    /// </summary>
    /// <param name="del"></param>
    public void OnRestart(OnRestartDelegate del)
    {
        onRestartDelegates += del;
    }

    /// <summary>
    /// Adds a listener for the OnStart event.
    /// </summary>
    /// <param name="del">Del.</param>
    public void OnStart(OnStartDelegate del)
    {
        if (hasStarted)
        {
            del();
        }
        else
        {
            onStartDelegate += del;
        }
    }

    /// <summary>
    /// Add a given game object to the manager, at a given location.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="location"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public Guid Add(GameObject go, Location location, ARLocationObjectOptions options = null)
    {
        return Add(new ARLocationManagerEntry
        {
            instance = go,
            location = location,
            options = options == null ? new ARLocationObjectOptions() : options
        });
    }

    /// <summary>
    /// Registers a new entry in the ARLocationManager.
    /// </summary>
    /// <param name="entry">Entry.</param>
    public Guid Add(ARLocationManagerEntry entry)
    {
        Guid uuid = Guid.NewGuid();
        entry.uuid = uuid;

        if (entry.options.createInstance)
        {
            var instance = Instantiate(entry.instance, transform);
            entry.instance = instance;
        }
        else
        {
            entry.instance.transform.SetParent(transform);
        }

        entries.Add(uuid, entry);

        // Check if we use smooth movement
        if (entry.options.movementSmoothingFactor > 0)
        {
            entry.instance.AddComponent<SmoothMove>();
            entry.instance.GetComponent<SmoothMove>().smoothing = entry.options.movementSmoothingFactor;
        }

        if (onObjectAddedDelegates != null)
        {
            onObjectAddedDelegates(entry);
        }

        entry.isDirty = true;

        if (entry.options.useAverageFilter)
        {
            entry.averageFilter = new MovingAveragePosition();
            entry.averageFilter.aMax = locationProvider.provider.options.accuracyFilter > 0 ? locationProvider.provider.options.accuracyFilter : 20;
        }

        return uuid;
    }

    /// <summary>
    /// Removes all entries from the manager
    /// </summary>
    public void Clear()
    {
        foreach (var item in entries.Keys)
        {
            Remove(item);
        }
    }

    /// <summary>
    /// Removes and entry from the manager
    /// </summary>
    /// <param name="entry"></param>
    public void Remove(ARLocationManagerEntry entry)
    {
        Remove(entry.uuid);
    }

    /// <summary>
    /// Removes an entry from the manager
    /// </summary>
    /// <param name="id"></param>
    public void Remove(Guid id)
    {
        var entry = GetEntry(id);

        if (entry == null)
        {
            return;
        }

        entries.Remove(id);

        if (onObjectRemovedDelegates != null)
        {
            onObjectRemovedDelegates(entry);
        }

        if (entry.options.createInstance && entry.instance != null)
        {
            GameObject.Destroy(entry.instance);
        }
    }

    /// <summary>
    /// Removes an entry from the manager
    /// </summary>
    /// <param name="instance"></param>
    public void Remove(GameObject instance)
    {
        var entry = GetEntry(instance);

        if (entry == null)
        {
            return;
        }

        Remove(entry.uuid);
    }

    /// <summary>
    /// Fetches the entry for a given instance id.
    /// </summary>
    /// <param name="id">The transform instance ID</param>
    /// <returns>ARLocationManagerEntry</returns>
    public ARLocationManagerEntry GetEntry(Guid id)
    {
        return entries[id];
    }

    /// <summary>
    /// Fetches the entry for a given instance.
    /// </summary>
    /// <param name="instance"></param>
    /// <returns></returns>
    public ARLocationManagerEntry GetEntry(GameObject instance)
    {
        foreach (var item in entries)
        {
            if (item.Value.instance == instance)
            {
                return item.Value;
            }
        }

        return null;
    }


    /// <summary>
    /// Changes the entry's location.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newLocation"></param>
    public void RelocateEntry(Guid id, Location newLocation)
    {
        var entry = GetEntry(id);

        if (entry == null)
        {
            return;
        }

        entry.Relocate(newLocation);
        UpdateObjectPosition(id);
    }

    /// <summary>
    /// Changes the entry's location.
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="newLocation"></param>
    public void RelocateEntry(ARLocationManagerEntry entry, Location newLocation)
    {
        RelocateEntry(entry.uuid, newLocation);
    }

    public void RelocateEntry(GameObject instance, Location newLocation)
    {
        var entry = GetEntry(instance);

        if (entry == null)
        {
            return;
        }

        RelocateEntry(entry.uuid, newLocation);
    }

    /// <summary>
    /// Called when the device location is updated
    /// </summary>
    /// <param name="location">Location.</param>
    /// <param name="accuracy">Accuracy.</param>
    void HandleLocationUpdatedDelegate(LocationReading currentLocation, LocationReading lastLocation, DVector3 displacement)
    {
        UpdatePositions(currentLocation.ToLocation());
    }

    /// <summary>
    /// Updates the position of all the GameObjects
    /// </summary>
    /// <param name="deviceLocation">Location.</param>
    void UpdatePositions(Location deviceLocation)
    {
        foreach (var entry in entries)
        {
            if (entry.Value.enabled)
            {
                UpdateObjectPosition(entry.Value, deviceLocation);
            }
        }
    }

    /// <summary>
    /// Update the positions of all entries. If `useRawData` is true, it will
    /// use the raw GPS data instead of the filtered data set.
    /// </summary>
    /// <param name="useRawData"></param>
    public void UpdatePositions(bool useRawData = false)
    {
        var location = useRawData ? locationProvider.provider.rawLocationLast : locationProvider.currentLocation;
        UpdatePositions(location.ToLocation());
    }

    /// <summary>
    /// Updates the object position.
    /// </summary>
    /// <param name="id">The object's transform instance ID.</param>
    public void UpdateObjectPosition(Guid id)
    {
        var entry = GetEntry(id);

        if (entry == null)
        {
            return;
        }

        var currentLocation = locationProvider.currentLocation;

        UpdateObjectPosition(entry, currentLocation.ToLocation(), false);
    }

    /// <summary>
    /// Updates the object position.
    /// </summary>
    /// <param name="instance">Instance.</param>
    /// <param name="instanceLocation">Instance location.</param>
    /// <param name="instanceOptions">Instance options.</param>
    /// <param name="deviceLocation">Device location.</param>
    public void UpdateObjectPosition(
        ARLocationManagerEntry entry,
        Location deviceLocation,
        bool forceDisableSmooth = false
    )
    {
        var instance = entry.instance;
        var instanceLocation = entry.location;
        var instanceOptions = entry.options;
        var smoothMove = instance.GetComponent<SmoothMove>();

        Vector3 targetPosition;
        if (entry.averageFilter != null)
        {
            var position = Location.GetGameObjectPositionForLocation(
                Camera.main.transform, deviceLocation, instanceLocation, instanceOptions.isHeightRelative
            );

            var accuracy = locationProvider.currentLocation.accuracy;

            entry.averageFilter.AddEntry(new DVector3(position), accuracy);

            targetPosition = entry.averageFilter.CalculateAveragePosition().toVector3();
        }
        else
        {
            targetPosition = Location.GetGameObjectPositionForLocation(
                Camera.main.transform, deviceLocation, instanceLocation, instanceOptions.isHeightRelative
            );
        }

        var useSmoothMove = !(smoothMove == null || forceDisableSmooth || locationProvider.provider.isRawTime());

        if (instanceOptions.UseNearestDetectedPlaneHeight)
        {
            if (arLocationplaneManager == null)
            {
#if ARGPS_USE_VUFORIA
                Debug.LogWarning("[ARLocationManager]: When using `UseNearestDetectedPlaneHeight` you should add a Vuforia Ground Plane Finder to your scene.");
#else
                Debug.LogWarning("[ARLocationManager]: ARLocationPlane manager is null.");
#endif
            }
            else
            {
                targetPosition.y = arLocationplaneManager.GetClosestPlaneHeight(targetPosition) + (instanceLocation.ignoreAltitude ? 0.0f : (float)instanceLocation.altitude);
            }
        }

        if (useSmoothMove)
        {
            smoothMove.Target = targetPosition;
        }
        else
        {
            instance.transform.localPosition = targetPosition;
        }

    }

   

    /// <summary>
    /// Restarts the current ARLocationManager session. Causes the ARFoundation session
    /// to reset, and flags all objects as dirty.
    /// </summary>
    void Restart()
    {
#if !ARGPS_USE_VUFORIA
        var arSession = GameObject.Find("AR Session").GetComponent<ARSession>();
        waitScreen = Instantiate(resetWaitScreen, Camera.main.transform);
        waitingReset = true;

        arSession.Reset();

        var orientation = GetComponent<ARLocationOrientation>();

        if (orientation != null)
        {
            orientation.Restart();
        }

        if (onRestartDelegates != null)
        {
            onRestartDelegates();
        }
#endif
    }
}
