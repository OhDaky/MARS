using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARLocation
{
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

        public static LocationData FromLocation(Location location) {
            var data = CreateInstance<LocationData>();
            data.location = location;

            return data;
        }
    }
}
