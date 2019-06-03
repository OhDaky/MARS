using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARLocation
{
    [Serializable]
    public class LocationProviderOptions
    {
        /// <summary>
        /// The minimum desired update time, in seconds.
        /// </summary>
        [Tooltip("The minimum desired update time, in seconds.")]
        public float TimeBetweenUpdates = 2.0f;

        /// <summary>
        /// The minimum distance between consecutive location updates, in meters.
        /// </summary>
        [Tooltip("The minimum distance between consecutive location updates, in meters.")]
        public double MinDistanceBetweenUpdates = 2.0f;

        /// <summary>
        /// The maximum distance between consectutive location updates, in meters.
        /// </summary>
        [Tooltip("The maximum distance between consectutive location updates, in meters.")]
        public double MaxDistanceBetweenUpdates = 20.0f;

        /// <summary>
        /// The minimum accuracy of accepted location measurements, in meters.
        /// </summary>
        [Tooltip("The minimum accuracy of accepted location measurements, in meters. " +
            "Accuracy here means the radius of uncertainty of the device's location, " +
            "defining a circle where it can possibly be found in.")]
        public double MaxAccuracyRadius = 25.0f;

        [Tooltip("The maximum number of location updates. The updates will be paused after this amount. Zero means there is no limit and " +
            "the updates won't be paused automatically.")]
        public uint MaxNumberOfUpdates = 4;
    }

    public enum LocationProviderStatus
    {
        Idle,
        Initializing,
        Started,
        Failed
    }

    // Location provider delegates/events
    public delegate void LocationUpdatedDelegate(LocationReading currentLocation, LocationReading lastLocation);
    public delegate void CompassUpdateDelegate(HeadingReading heading, HeadingReading lastReading);
    public delegate void LocationEnabledDelegate();
    public delegate void LocationFailedDelegate(string message);

    public interface ILocationProvider
    {
        string name { get; }

        LocationProviderOptions options { get; set; }

        LocationReading currentLocation { get; }
        LocationReading currentLocationRaw { get; }
        LocationReading lastLocation { get; }
        LocationReading lastLocationRaw { get; }
        LocationReading firstLocation { get; }

        HeadingReading currentHeading { get; }
        HeadingReading lastHeading { get; }

        float startTime { get; }
        bool isCompassEnabled { get; }
        double distanceFromStartPoint { get; }
        bool isEnabled { get; }
        bool paused { get; }

        event LocationUpdatedDelegate LocationUpdated;
        event LocationUpdatedDelegate LocationUpdatedRaw;
        event CompassUpdateDelegate CompassUpdated;
        event CompassUpdateDelegate CompassUpdatedRaw;
        event LocationEnabledDelegate LocationEnabled;
        event LocationFailedDelegate LocationFailed;

        IEnumerator Start(uint maxWaitTime = 10000);

        void Pause();
        void Resume();
        void Update();

        void OnEnabled(LocationEnabledDelegate del);
        void OnFail(LocationFailedDelegate del);

        string GetInfoString();
        string GetStatusString();
    }
}
