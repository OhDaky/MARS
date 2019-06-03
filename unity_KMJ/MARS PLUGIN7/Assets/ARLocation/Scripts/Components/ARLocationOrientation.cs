using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARLocation
{
    using System;
    using Utils;


    /// <summary>
    /// This component should be placed on the "ARLocationRoot" GameObject (which should be a child of the
    /// "AR Session Origin") for correctly aligning the coordinate system to the north/east geographical lines.
    /// </summary>
    public class ARLocationOrientation : Singleton<ARLocationOrientation>
    {

        [Tooltip("The minimum desired update time, in seconds.")]
        [Header("Update Settings")]
        public float TimeBetweenUpdates = 2.0f;

        [Tooltip("The maximum distance between consectutive location updates, in meters.")]
        public double MaxAngleBetweenUpdates = 20.0f;

        [Tooltip("The maximum number of orientation updates. The updates will be paused after this amount. Zero means there is no limit and " +
        "the updates won't be paused automatically.")]
        public uint MaxNumberOfUpdates = 4;


        /// <summary>
        /// Only update after measuring the heading N times, and take the average.
        /// </summary>
        [Tooltip("Only update after measuring the heading N times, and take the average."), Range(1, 500)]
        [Header("Averaging")]
        public int averageCount = 250;

        /// <summary>
        /// If set to true, use raw heading values until measuring the first average.
        /// </summary>
        [Tooltip("If set to true, use raw heading values until measuring the first average.")]
        public bool useRawUntilFirstAverage = true;

        /// <summary>
        /// The smoothing factor. Zero means disabled. Values around 100 seem to give good results.
        /// </summary>
        [Tooltip("The smoothing factor. Zero means disabled. Values around 100 seem to give good results."), Range(0, 500)]
        [Header("Smoothing")]
        public float MovementSmoothingFactor = 50.0f;

        /// <summary>
        /// A custom offset to the device-calculated true north direction.
        /// </summary>
        [Tooltip("A custom offset to the device-calculated true north direction.")]
        [Header("Calibration")]
        public float TrueNorthOffset = 0.0f;

        ARLocationProvider locationProvider;

        int updateCounter = 0;
        List<float> values = new List<float>();
        bool isFirstAverage = true;
        float targetAngle = 0.0f;
        float lastUptdateTime = -1.0f;

        /// <summary>
        /// Restarts the orientation tracking.
        /// </summary>
        public void Restart()
        {
            isFirstAverage = true;
            targetAngle = Camera.main.transform.rotation.eulerAngles.y;
        }

        // Use this for initialization
        void Start()
        {
            // Look for the LocationProvider
            locationProvider = ARLocationProvider.Instance;

            targetAngle = Camera.main.transform.rotation.eulerAngles.y;

            // Register compass update delegate
            locationProvider.OnCompassUpdated(OnCompassUpdatedHandler);
        }

        private void OnCompassUpdatedHandler(HeadingReading newHeading, HeadingReading lastReading)
        {

            if (!newHeading.isMagneticHeadingAvailable)
            {
                Debug.LogWarning("[AR+GPS][ARLocationOrientation]: Magnetic heading data not available.");
                return;
            }

            if (MaxNumberOfUpdates > 0 && updateCounter >= MaxNumberOfUpdates)
            {
                return;
            }

            var trueHeading = newHeading.heading + TrueNorthOffset;


            float currentCameraHeading = Camera.main.transform.rotation.eulerAngles.y;
            float value = Misc.GetNormalizedDegrees(currentCameraHeading - ((float)trueHeading));

            if (Mathf.Abs(value) < 0.0000001f)
            {
                return;
            }

            // If averaging is not enabled
            if (averageCount <= 1)
            {
                if (updateCounter == 0)
                {
                    transform.localRotation = Quaternion.AngleAxis(value, Vector3.up);
                    TrySetOrientation(value, true);
                }
                else
                {
                    TrySetOrientation(value);
                }

                return;
            }

            values.Add(value);

            if (updateCounter == 0 && values.Count == 1)
            {
                TrySetOrientation(value, true);
                return;
            }


            if (isFirstAverage && useRawUntilFirstAverage)
            {
                TrySetOrientation(value, true);
                return;
            }

            if (values.Count >= averageCount)
            {
                if (isFirstAverage)
                {
                    isFirstAverage = false;
                }

                var average = Misc.FloatListAverage(values);
                values.Clear();

                TrySetOrientation(average);

                return;
            }
        }

        public bool IsUpdateTime()
        {
            float currentTime = Time.time;

            float deltaTime = currentTime - lastUptdateTime;

            if (lastUptdateTime < 0)
            {
                deltaTime = 0.0f;
            }

            if (deltaTime < TimeBetweenUpdates)
            {
                lastUptdateTime = currentTime;
                return true;
            }

            return false;
        }

        private void TrySetOrientation(float angle, bool isFirstUpdate = false)
        {
            if (isFirstUpdate)
            {
                targetAngle = angle;
                transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
                updateCounter++;

                return;
            }

            if (MaxNumberOfUpdates > 0 && updateCounter >= MaxNumberOfUpdates)
            {
                return;
            }

            if (!IsUpdateTime())
            {
                return;
            }

            float delta = targetAngle - angle;

            if (Mathf.Abs(delta) > MaxAngleBetweenUpdates)
            {
                return;
            }

            targetAngle = angle;
            updateCounter++;
        }

        private void Update()
        {
            if (locationProvider.provider == null || !locationProvider.provider.isCompassEnabled)
            {
                return;
            }

            if (Mathf.Abs(transform.rotation.eulerAngles.y - targetAngle) <= 0.05f)
            {
                return;
            }

            var value = Mathf.Lerp(transform.rotation.eulerAngles.y, targetAngle, Mathf.Exp(-MovementSmoothingFactor * Time.deltaTime));
            transform.localRotation = Quaternion.AngleAxis(value, Vector3.up);
        }
    }
}
