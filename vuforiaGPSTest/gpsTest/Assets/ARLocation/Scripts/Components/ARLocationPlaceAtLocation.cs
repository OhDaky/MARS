using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Apply to a GameObject to place it at a specified geographic location.
/// </summary>
public class ARLocationPlaceAtLocation : MonoBehaviour
{
    /// <summary>
    /// The location to place the GameObject at.
    /// </summary>
    [Tooltip("The location to place the GameObject at.")]
    public Location location;

    /// <summary>
    /// The data of location to place the GameObject at. If present will override the 'Location' option above.
    /// </summary>
    [Tooltip("The data of location to place the GameObject at. If present will override the 'Location' option above.")]
    public LocationData locationData;

    /// <summary>
    /// If true, the altitude will be computed as relative to the device level.
    /// </summary>
    [Tooltip("If true, the altitude will be computed as relative to the device level.")]
    public bool isHeightRelative = true;

    /// <summary>
    /// If true, the altitude will be computed as relative to the device level.
    /// </summary>
    [Tooltip("If true, the altitude will be computed as relative to nearest detected plane. Takes precedence from isHeightRelative.")]
    public bool UseNearestDetectedPlaneHeight = true;

    /// <summary>
    /// If true, will display a UI panel with debug information above the object.
    /// </summary>
    [Tooltip("If true, will display a UI panel with debug information above the object.")]
    public bool showDebugInfoPanel = false;

    /// <summary>
    /// The smoothing factor for movement due to GPS location adjustments; if set to zero it is disabled.
    /// </summary>
    [Tooltip("The smoothing factor for movement due to GPS location adjustments; if set to zero it is disabled."), Range(0, 500)]
    public float movementSmoothingFactor = 250.0f;

    [Tooltip("If true, use a moving average filter.")]
    public bool useAverageFilter = false;

    [System.NonSerialized, HideInInspector]
    public ARLocationManager manager;

    [System.NonSerialized, HideInInspector]
    private ARLocationManagerEntry entry;

    private Guid entryID = Guid.Empty;

    // Use this for initialization
    void Start()
    {
        manager = ARLocationManager.Instance;

        if (locationData != null)
        {
            location = locationData.location.Clone();
        }


        entry = new ARLocationManagerEntry
        {
            instance = gameObject,
            location = location.Clone(),
            options = new ARLocationObjectOptions
            {
                isHeightRelative = isHeightRelative,
                showDebugInfoPanel = showDebugInfoPanel,
                movementSmoothingFactor = movementSmoothingFactor,
                createInstance = false,
                useAverageFilter = useAverageFilter,
                UseNearestDetectedPlaneHeight = UseNearestDetectedPlaneHeight
            }
        };

        entryID = manager.Add(entry);
    }

    /// <summary>
    /// Sets the GameObject's location to a new one.
    /// </summary>
    /// <param name="newLocation"></param>
    public void SetLocation(Location newLocation)
    {
        location = newLocation.Clone();
        entry.location = newLocation.Clone();
        entry.isDirty = true;

        var manager = ARLocationManager.Instance;

        if (manager && entryID != Guid.Empty)
        {
            manager.UpdateObjectPosition(entryID);
        }
    }
}
