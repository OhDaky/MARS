using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARLocation {

    [AddComponentMenu("AR+GPS/Hotspot")]
    public class Hotspot : MonoBehaviour
    {
        [System.Serializable]
        public enum PositionModes {
            HotspotCenter,
            CameraPosition
        };

        public GameObject prefab;

        public LocationData HotspotLocation;

        public float ActivationRadius;

        public bool AlignToCamera = true;

        public PositionModes PositionMode;
        

        private Transform root;
        private Camera arCamera;
        private GameObject instance;
        private Location location;

        void Start()
        {
            // HideChildren();

            ARLocationProvider.Instance.provider.LocationUpdatedRaw += Provider_LocationUpdatedRaw;

            root = ARLocationManager.Instance.gameObject.transform;

            location = HotspotLocation == null ? new Location() : HotspotLocation.location;

            arCamera = Camera.main;
        }

        //private void HideChildren()
        //{
        //    foreach (Transform child in transform)
        //    {
        //        child.gameObject.SetActive(false);
        //    }
        //}

        //private void ShowChildren()
        //{
        //    foreach (Transform child in transform)
        //    {
        //        child.gameObject.SetActive(true);
        //    }
        //}

        private void Provider_LocationUpdatedRaw(LocationReading currentLocation, LocationReading lastLocation)
        {
            var distance = Location.HorizontalDistance(currentLocation.ToLocation(), location);

            var delta = Location.HorizontalVectorFromTo(currentLocation.ToLocation(), location);

            if (distance <= ActivationRadius)
            {
                ActivateHotspot((float) distance, new Vector3((float) delta.x, 0, (float) delta.y));
            }
        }

        private void ActivateHotspot(float distance, Vector3 delta)
        {
            // ShowChildren();

            Debug.Log("[AR+GPS][Hotspot]: Activated!");

            if (prefab != null)
            {
                instance = Instantiate(prefab, root);

                switch (PositionMode)
                {
                    case PositionModes.HotspotCenter:
                        instance.transform.position = arCamera.transform.position + delta;
                        break;
                    case PositionModes.CameraPosition:
                        instance.transform.position = arCamera.transform.position;
                        break;
                    default:
                        break;
                }

                if (AlignToCamera)
                {
                    instance.transform.LookAt(arCamera.transform);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
