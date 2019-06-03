using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ARLocation
{
    using Utils;

    [AddComponentMenu("AR+GPS/AR Location Provider")]
    public class ARLocationProvider : Singleton<ARLocationProvider>
    {
        [Tooltip("The options for the Location Provider.")]
        [Header("Update Settings")]
        public LocationProviderOptions LocationUpdateSettings;

        [Tooltip("The data of mock location. If presend, overrides the Mock Location above.")]
        [Header("Mock Data")]
        public LocationData MockLocationData;

        [Tooltip("The maximum wait time to wait for location initialization.")]
        [Header("Initialization")]
        public uint MaxWaitTime = 20;

        public ILocationProvider provider { get; private set; }

        public bool isEnabled
        {
            get
            {
                return provider.isEnabled;
            }
        }

        public bool isPaused
        {
            get
            {
                return provider.paused;
            }
        }

        public LocationReading currentLocation
        {
            get
            {
                return provider.currentLocation;
            }
        }

        public LocationReading lastLocation
        {
            get
            {
                return provider.lastLocation;
            }
        }

        public HeadingReading currentHeading
        {
            get
            {
                return provider.currentHeading;
            }
        }


        public float TimeSinceStart
        {
            get
            {
                return Time.time - provider.startTime;
            }
        }

        public double distanceFromStartPoint
        {
            get
            {
                return provider.distanceFromStartPoint;
            }
        }

        int measurementCount = 0;

        event LocationUpdatedDelegate onLocationUpdated;
        event CompassUpdateDelegate onCompassUpdated;

        public override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            provider = new MockLocationProvider();

            if (MockLocationData != null)
            {
                (provider as MockLocationProvider).mockLocation = MockLocationData.location;
            }
#else
        provider = new UnityLocationProvider();
#endif

            Debug.Log("[ARLocationProvider]: Using provider " + provider.name);

            provider.options = LocationUpdateSettings;

            provider.LocationUpdated += Provider_LocationUpdated;
            provider.CompassUpdated += Provider_CompassUpdated;
        }

        IEnumerator Start()
        {
            yield return StartCoroutine(provider.Start(MaxWaitTime));
        }

        private void Provider_CompassUpdated(HeadingReading heading, HeadingReading lastReading)
        {
            if (onCompassUpdated != null)
            {
                onCompassUpdated(heading, lastReading);
            }
        }

        private void Provider_LocationUpdated(LocationReading currentLocation, LocationReading lastLocation)
        {
            measurementCount++;

            if ((LocationUpdateSettings.MaxNumberOfUpdates > 0) && (measurementCount >= LocationUpdateSettings.MaxNumberOfUpdates))
            {
                provider.Pause();
            }

            if (onLocationUpdated != null)
            {
                onLocationUpdated(currentLocation, lastLocation);
            }
        }

        void Update()
        {
            if (provider == null || !provider.isEnabled)
            {
                return;
            }

            provider.Update();
        }

        /// <summary>
        /// Pauses location updates
        /// </summary>
        public void Pause()
        {
            if (provider != null)
            {
                provider.Pause();
            }
        }

        /// <summary>
        /// Resumes location updates
        /// </summary>
        public void Resume()
        {
            if (provider != null)
            {
                provider.Pause();
            }
        }

        public void OnLocationUpdated(LocationUpdatedDelegate locationUpdatedDelegate)
        {
            onLocationUpdated += locationUpdatedDelegate;
        }

        public void OnCompassUpdated(CompassUpdateDelegate compassUpdateDelegate)
        {
            onCompassUpdated += compassUpdateDelegate;
        }

        /// <summary>
        /// Register a delegate for when the provider enables location updates.
        /// </summary>
        /// <param name="del">Del.</param>
        public void OnEnabled(LocationEnabledDelegate del)
        {
            provider.OnEnabled(del);
        }

        /// <summary>
        /// Register a delegate for when the provider fails to initialize location services.
        /// </summary>
        /// <param name="del">Del.</param>
        public void OnFailed(LocationFailedDelegate del)
        {
            provider.OnFail(del);
        }
    }
}
