using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// The options passed to the ARLocationManager when adding a new positioned GameObject.
/// </summary>
[System.Serializable]
public class ARLocationObjectOptions
{
    /// <summary>
    /// If true, the altitude will be computed as relative to the device level.
    /// </summary>
    [Tooltip("If true, the altitude will be computed as relative to the device level.")]
    public bool isHeightRelative;

    /// <summary>
    /// If true, will display a UI panel with debug information above the object.
    /// </summary>
    [Tooltip("If true, will display a UI panel with debug information above the object.")]
    public bool showDebugInfoPanel;

    /// <summary>
    /// If true, will clone the object when placing it on the scene.
    /// </summary>
    [Tooltip("If true, will clone the object when placing it on the scene.")]
    public bool createInstance;

    /// <summary>
    /// The smoothing factor for movement due to GPS location adjustments; if set to zero it is disabled.
    /// </summary>
    [Tooltip("The smoothing factor for movement due to GPS location adjustments; if set to zero it is disabled."), Range(0, 500)]
    public float movementSmoothingFactor;

    /// <summary>
    /// If true, the altitude will be computed as relative to the device level.
    /// </summary>
    public bool UseNearestDetectedPlaneHeight;

    /// <summary>
    /// If true, use a moving average filter.
    /// </summary>
    [Tooltip("If true, use a moving average filter.")]
    public bool useAverageFilter;
};

/// <summary>
/// This structure holds all data for a positioned GameObject in the ARLocationManager.
/// </summary>
[System.Serializable]
public class ARLocationManagerEntry
{
    /// <summary>
    /// The GameObject to be placed in the scene.
    /// </summary>
    [Tooltip("The GameObject to be placed in the scene.")]
    public GameObject instance;

    /// <summary>
    /// The GPS/geolocation coordinates.
    /// </summary>
    [Tooltip("The GPS/geolocation coordinates.")]
    public Location location;

    /// <summary>
    /// The placement options.
    /// </summary>
    [Tooltip("The placement options.")]
    public ARLocationObjectOptions options;

    /// <summary>
    /// Dirty location flag.
    /// </summary>
    [HideInInspector]
    public bool isDirty = true;

    [HideInInspector]
    public bool enabled = true;

    /// <summary>
    /// The entry's unique identifier
    /// </summary>
    [HideInInspector]
    public Guid uuid;

    /// <summary>
    /// Changes the location of the entry.
    /// </summary>
    /// <param name="newLocation"></param>
    public void Relocate(Location newLocation)
    {
        location = newLocation.Clone();
        isDirty = true;
    }

    /// <summary>
    /// Enables this entry
    /// </summary>
    public void Enable()
    {
        enabled = true;
    }

    /// <summary>
    /// Disables this entry
    /// </summary>
    public void Disable()
    {
        enabled = false;
    }

    /// <summary>
    /// This will return the world distance of the object to the main camera.
    /// </summary>
    /// <returns></returns>
    public float Distance()
    {
        return Vector3.Distance(instance.transform.position, Camera.main.transform.position);
    }

    /// <summary>
    /// This will return the geo distance between the entry and the last raw gps location data.
    /// </summary>
    /// <returns></returns>
    public double GPSHorizontalDistance()
    {
        return Location.HorizontalDistance(ARLocationProvider.Instance.provider.rawLocationLast.ToLocation(), location);
    }

    public MovingAveragePosition averageFilter;
}
