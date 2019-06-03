using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARLocation
{

    /// <summary>
    /// This class instantiates a prefab at the given GPS locations. Must 
    /// be in the `ARLocationRoot` GameObject with a `ARLocatedObjectsManager` 
    /// Component.
    /// </summary>
    public class PlaceAtLocations : MonoBehaviour
    {
        [Serializable]
        public class Entry
        {
            public LocationData ObjectLocation;
            public OverrideAltitudeData OverrideAltitude = new OverrideAltitudeData();
        }

        [Tooltip("The locations where the objects will be instantiated.")]
        public List<Entry> Locations;

        /// <summary>
        /// The game object that will be instantiated.
        /// </summary>
        [Tooltip("The game object that will be instantiated.")]
        public GameObject prefab;

        public PlaceAtLocation.PlaceAtOptions Options;

        ARLocationManager manager;
        List<Location> locations = new List<Location>();
        List<GameObject> instances = new List<GameObject>();

        private void Start()
        {
            manager = ARLocationManager.Instance;

            if (manager == null)
            {
                Debug.LogError("[ARFoundation+GPSLocation][PlaceAtLocations]: ARLocatedObjectsManager Component not found.");
                return;
            }

            foreach (var entry in Locations)
            {
                var newLoc = entry.ObjectLocation.location.Clone();

                if (entry.OverrideAltitude.overrideAltitude)
                {
                    newLoc.altitude = entry.OverrideAltitude.altitude;
                    newLoc.altitudeMode = entry.OverrideAltitude.altitudeMode;
                }

                AddLocation(newLoc);
            }
        }

        public void AddLocation(Entry entry)
        {
            var instance = PlaceAtLocation.CreatePlacedInstance(prefab, entry.ObjectLocation, entry.OverrideAltitude, Options);

            locations.Add(instance.GetComponent<PlaceAtLocation>().Location);
            instances.Add(instance);
        }

        public void AddLocation(Location location)
        {
            var instance = PlaceAtLocation.CreatePlacedInstance(prefab, LocationData.FromLocation(location), Options);

            locations.Add(location);
            instances.Add(instance);
        }
    }
}
