using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data used to construct a spline passing trough a set of geographical
/// locations.
/// </summary>
[CreateAssetMenu(fileName = "AR Location Data", menuName = "ARLocation/Location")]
public class LocationData : ScriptableObject
{
    /// <summary>
    /// The geographical locations that the path will interpolate.
    /// </summary>
    [Tooltip("The geographical locations that the path will interpolate.")]
    public Location location;
}