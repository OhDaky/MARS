using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ARLocation
{
    using Utils;

    [Serializable]
    public class OverrideAltitudeData
    {
        public bool overrideAltitude = false;

        public double altitude = 0;

        public Location.AltitudeMode altitudeMode = Location.AltitudeMode.Absolute;
    }

    /// <summary>
    /// Apply to a GameObject to place it at a specified geographic location.
    /// </summary>
    [AddComponentMenu("AR+GPS/Place At Location")]
    public class PlaceAtLocation : MonoBehaviour
    {
        [Serializable]
        public class ObjectUpdatedEvent : UnityEvent<GameObject, Location, int> { }

        [Serializable]
        public class PlaceAtOptions
        {
            [Tooltip("If true, use a moving average filter.")]
            public bool UseMovingAverage = false;

            [Tooltip("The smoothing factor for movement due to GPS location adjustments; if set to zero it is disabled."), Range(0, 500)]
            public float MovementSmoothingFactor = 250.0f;

            [Tooltip("The maximum number of times this object will be affected by GPS location updates. Zero means no limits are imposed.")]
            public int MaxNumberOfLocationUpdates = 0;
        }

        [Tooltip("The location to place the GameObject at.")]
        public LocationData ObjectLocation;

        [Tooltip("Overrides the location's altitude.")]
        public OverrideAltitudeData OverrideAltitude = new OverrideAltitudeData();

        [Space(4.0f)]

        public PlaceAtOptions PlacementOptions;

        [Space(4.0f)]

        [Tooltip("Event called when the object's location is updated. The arguments are the current GameObject, the location, and the number of location updates received " +
            "by the object so far.")]
        public ObjectUpdatedEvent ObjectLocationUpdated;

        public Location Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value.Clone();
                Debug.Assert(locationProvider != null, "[AR+GPS][PlaceAtLocation]: Location Provider is null.");
                UpdatePosition(locationProvider.currentLocation.ToLocation());
            }
        }

        private ARLocationProvider locationProvider;
        private GameObject root;
        private SmoothMove smoothMove;
        private MovingAveragePosition movingAverageFilter;
        private Location location;
        private Canvas debugCanvas;
        private GameObject debugPanel;
        private int updateCount;

        // Use this for initialization
        void Start()
        { 
            locationProvider = ARLocationProvider.Instance;

            root = ARLocationManager.Instance.gameObject;

            if (locationProvider == null)
            {
                Debug.LogError("[AR+GPS][PlaceAtLocation]: LocationProvider GameObject or Component not found.");
                return;
            }

            if (ObjectLocation != null)
            {
                Initialize();
            }
            else
            {
                Debug.LogWarning("[AR+GPS][PlaceAtLocation]: ObjectLocation is null. Set the location on the editor inspector, or set it via code " +
                    "and call `Initialize` for late initialization.");
            }

            if (ObjectLocationUpdated == null)
            {
                ObjectLocationUpdated = new ObjectUpdatedEvent();
            }
        }

        void Initialize()
        {
            Debug.Assert(ObjectLocation != null, "[AR+GPS][PlaceAtLocation]: ObjectLocation is null.");

            if (PlacementOptions.MovementSmoothingFactor > 0)
            {
                smoothMove = gameObject.AddComponent<SmoothMove>();
                smoothMove.smoothing = PlacementOptions.MovementSmoothingFactor;
            }

            if (PlacementOptions.UseMovingAverage)
            {
                movingAverageFilter = new MovingAveragePosition();
                movingAverageFilter.aMax = locationProvider.provider.options.MaxAccuracyRadius > 0 ? locationProvider.provider.options.MaxAccuracyRadius : 20;
            }

            location = ObjectLocation.location.Clone();

            if (OverrideAltitude.overrideAltitude)
            {
                location.altitude = OverrideAltitude.altitude;
                location.altitudeMode = OverrideAltitude.altitudeMode;
            }

            var ObjectInfoObject = GameObject.Find("ARLocationInfo/ObjectInfoCanvas");

            if (ObjectInfoObject != null)
            {
                debugCanvas = ObjectInfoObject.GetComponent<Canvas>();
            }

            locationProvider.OnLocationUpdated(locationUpdatedHandler);
        }

        private void locationUpdatedHandler(LocationReading currentLocation, LocationReading lastLocation)
        {
            UpdatePosition(currentLocation.ToLocation());
        }

        public void UpdatePosition(Location deviceLocation)
        {
            Vector3 targetPosition;

            var isHeightRelative = location.altitudeMode == Location.AltitudeMode.DeviceRelative;
            var UseNearestDetectedPlaneHeight = location.altitudeMode == Location.AltitudeMode.GroundRelative;
            var ignoreAltitude = location.altitudeMode == Location.AltitudeMode.Ignore;

            if (PlacementOptions.MaxNumberOfLocationUpdates > 0 && updateCount >= PlacementOptions.MaxNumberOfLocationUpdates)
            {
                return;
            }
            else
            {
                updateCount++;
            }


            if (movingAverageFilter != null)
            {
                var position = global::ARLocation.Location.GetGameObjectPositionForLocation(
                    Camera.main.transform, deviceLocation, location, isHeightRelative
                );

                var accuracy = locationProvider.currentLocation.accuracy;

                movingAverageFilter.AddEntry(new DVector3(position), accuracy);

                targetPosition = movingAverageFilter.CalculateAveragePosition().toVector3();
            }
            else
            {
                targetPosition = global::ARLocation.Location.GetGameObjectPositionForLocation(
                    Camera.main.transform, deviceLocation, location, isHeightRelative
                );
            }

            var useSmoothMove = smoothMove != null;
            var arLocationplaneManager = ARLocationManager.Instance.PlaneManager;

            if (UseNearestDetectedPlaneHeight)
            {
                if ( arLocationplaneManager == null)
                {
#if ARGPS_USE_VUFORIA
                Debug.LogWarning("[ARLocationManager]: When using `UseNearestDetectedPlaneHeight` you should add a Vuforia Ground Plane Finder to your scene.");
#else
                    Debug.LogWarning("[ARLocationManager]: ARLocationPlane manager is null.");
#endif
                }
                else
                {
                    targetPosition.y = arLocationplaneManager.GetClosestPlaneHeight(targetPosition) + (ignoreAltitude ? 0.0f : (float) location.altitude);
                }
            }

            if (useSmoothMove)
            {
                smoothMove.Target = targetPosition;
            }
            else
            {
                transform.localPosition = targetPosition;
            }

            if (ObjectLocationUpdated != null)
            {
                ObjectLocationUpdated.Invoke(gameObject, location, updateCount);
            }
        }

        public static GameObject CreatePlacedInstance(GameObject go, LocationData location, OverrideAltitudeData overrirdeAltitude, PlaceAtOptions options)
        {
            var newLocData = Instantiate(location);

            if (overrirdeAltitude.overrideAltitude)
            {
                newLocData.location.altitude = overrirdeAltitude.altitude;
                newLocData.location.altitudeMode = overrirdeAltitude.altitudeMode;
            }

            return CreatePlacedInstance(go, newLocData, options);
        }

        public static GameObject CreatePlacedInstance(GameObject go, LocationData location, PlaceAtOptions options)
        {
            Debug.Assert(ARLocationManager.Instance != null, "[AR+GPS][PlaceAtLocation#CreatePlacedInstance]: No ARLocationManager instance found.");

            var instance = Instantiate(go, ARLocationManager.Instance.gameObject.transform);

            instance.SetActive(false);

            var placeAt = instance.AddComponent<PlaceAtLocation>();

            placeAt.PlacementOptions = options;
            placeAt.ObjectLocation = location;

            instance.SetActive(true);

            return instance;
        }
    }
}
