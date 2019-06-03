using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARLocation
{
    using Utils;

    /// <summary>
    /// This component, when attached to a GameObject, makes it traverse a
    /// path that interpolates a given set of geographical locations.
    /// </summary>
    public class MoveAlongPath : MonoBehaviour
    {
        /// <summary>
        /// The LocationPath describing the path to be traversed.
        /// </summary>
        [Tooltip("The LocationPath describing the path to be traversed.")]
        public LocationPath locationPath;

        /// <summary>
        /// The speed along the path.
        /// </summary>
        [Tooltip("The speed along the path.")]
        public float speed = 1.0f;

        /// <summary>
        /// The up direction to be used for orientation along the path.
        /// </summary>
        [Tooltip("The up direction to be used for orientation along the path.")]
        public Vector3 up = Vector3.up;

        /// <summary>
        /// If true, play the path traversal in a loop.
        /// </summary>
        [Tooltip("If true, play the path traversal in a loop.")]
        public bool loop = true;

        /// <summary>
        /// If true, start playing automatically.
        /// </summary>
        [Tooltip("If true, start playing automatically.")]
        public bool autoPlay = true;

        /// <summary>
        /// If true, all altitude data is considered relative to the current device elevation.
        /// </summary>
        [Tooltip("If true, all altitude data is considered relative to the current device elevation.")]
        public bool heightRelativeToDevice = true;

        /// <summary>
        /// If true, the altitude will be computed as relative to the device level.
        /// </summary>
        [Tooltip("If true, the altitude will be computed as relative to nearest detected plane. Takes precedence from isHeightRelative.")]
        public bool UseNearestDetectedPlaneHeight = true;

        /// <summary>
        /// The number of points-per-segment used to calculate the spline.
        /// </summary>
        [Tooltip("The number of points-per-segment used to calculate the spline.")]
        public int splineSampleCount = 250;

        /// <summary>
        /// The smoothing factor for movement due to GPS location adjustments; if set to zero it is disabled.
        /// </summary>
        [Tooltip("The smoothing factor for movement due to GPS location adjustments; if set to zero it is disabled."), Range(0, 500)]
        public float movementSmoothingFactor = 50.0f;

        /// <summary>
        /// If present, renders the spline in the scene using the given line renderer.
        /// </summary>
        [Tooltip("If present, renders the spline in the scene using the given line renderer.")]
        public LineRenderer lineRenderer;

        [Tooltip("The parameters offset; marks the initial position of the object along the curve.")]
        public float offset = 0.0f;

        ARLocationProvider locationProvider;

        Spline spline;

        Vector3[] points;

        int pointCount;

        bool playing = false;

        Vector3 translation = new Vector3();

        public void SetLocationPath(LocationPath path)
        {
            locationPath = path;

            pointCount = locationPath.locations.Length;
            points = new Vector3[pointCount];


            BuildSlpine(locationProvider.currentLocation.ToLocation());
        }


        void Start()
        {
            if (locationPath == null)
            {
                throw new Exception("null location path");
            }

            pointCount = locationPath.locations.Length;
            points = new Vector3[pointCount];

            locationProvider = ARLocationProvider.Instance;
            locationProvider.OnLocationUpdated(LocationUpdated);

            arLocationRoot = Misc.FindAndLogError("ARLocationRoot", "[ARLocationMoveAlongPath]: ARLocationRoot GameObject not found.");

            // Check if we use smooth movement
            if (movementSmoothingFactor > 0)
            {
                gameObject.AddComponent<SmoothMove>();
                GetComponent<SmoothMove>().smoothing = movementSmoothingFactor;
            }

            transform.SetParent(arLocationRoot.transform);

            playing = autoPlay;

            u += offset;
        }


        /// <summary>
        /// Starts playing or resumes the playback.
        /// </summary>
        public void Play()
        {
            playing = true;
        }

        /// <summary>
        /// Moves the object to the spline point corresponding 
        /// to the given parameter.
        /// </summary>
        /// <param name="t">Between 0 and 1</param>
        public void GoTo(float t)
        {
            u = Mathf.Clamp(t, 0, 1);
        }

        /// <summary>
        /// Pauses the movement along the path.
        /// </summary>
        public void Pause()
        {
            playing = false;
        }

        /// <summary>
        /// Stops the movement along the path.
        /// </summary>
        public void Stop()
        {
            playing = false;
            u = 0;
        }

        private void OnRestartHandler()
        {

        }

        void BuildSlpine(Location location)
        {
            for (var i = 0; i < pointCount; i++)
            {
                var loc = locationPath.locations[i];
                points[i] = Camera.main.transform.position + Location.VectorFromTo(location, loc, heightRelativeToDevice).toVector3()
                    + new Vector3(0, heightRelativeToDevice ? ((float)loc.altitude) : 0, 0);
            }

            spline = Misc.BuildSpline(locationPath.splineType, points, splineSampleCount, locationPath.alpha);
        }

        private void LocationUpdated(LocationReading location, LocationReading _)
        {
            BuildSlpine(location.ToLocation());
            translation = new Vector3(0, 0, 0);
            //translation += heightRelativeToDevice ? new Vector3((float)delta.x, 0, (float)delta.y) : delta.toVector3();
        }

        /// <summary>
        /// Normalized spline parameter
        /// </summary>
        float u = 0.0f;
        // private DebugInfo arLocationDebugInfo;
        private GameObject arLocationRoot;

        private void Update()
        {
            if (!playing)
            {
                return;
            }

            // If there is no location provider, or spline, do nothing
            if (locationProvider == null || spline == null || !locationProvider.isEnabled)
            {
                return;
            }

            // Get spline point at current parameter
            var s = spline.Length * u;

            var data = spline.GetPointAndTangentAtArcLength(s);
            var tan = data.tangent;

            // Move object to the point
            var smoothMove = GetComponent<SmoothMove>();
            var useSmoothMove = smoothMove != null;

            if (useSmoothMove)
            {
                smoothMove.Target = data.point; // - translation;
            }
            else
            {
                transform.localPosition = data.point; // - translation;
            }

            // Set orientation
            transform.localRotation = Quaternion.LookRotation(new Vector3(tan.x, tan.y, tan.z), up);

            // Check if we reached the end of the spline
            u = u + (speed * Time.deltaTime) / spline.Length;
            if (u >= 1 && !loop)
            {
                u = 0;
                playing = false;
            }
            else
            {
                u = u % 1.0f;
            }

            // If there is a line renderer, render the path
            if (lineRenderer != null)
            {
                lineRenderer.useWorldSpace = true;
                var t = arLocationRoot.GetComponent<Transform>();
                spline.DrawCurveWithLineRenderer(lineRenderer, p => t.TransformVector(p - translation));
            }
        }
    }
}
