using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARLocation
{
    using Utils;

    /// <summary>
    /// This component places instances of a given prefab/GameObject along
    /// equally spaced positions in a LocationPath. Should be placed in 
    /// the ARLocationRoot GameObject.
    /// </summary>
    public class PlaceAlongPath : MonoBehaviour
    {

        /// <summary>
        /// The path to place the prefab instances on.
        /// </summary>
        [Tooltip("The path to place the prefab instances on.")]
        public LocationPath path;

        /// <summary>
        /// The prefab/GameObject to be palced along the path.
        /// </summary>
        [Tooltip("The prefab/GameObject to be palced along the path.")]
        public GameObject prefab;

        /// <summary>
        /// The number of object instances to be placed, excluding the endpoints. That is, 
        /// the total number of instances is equal to objectCount + 2
        /// </summary>
        [Tooltip("The number of object instances to be placed, excluding the endpoints. That is, the total number of instances is equal to objectCount + 2")]
        public int objectCount = 10;

        /// <summary>
        /// The size of the sample used to calculate the spline.
        /// </summary>
        [Tooltip("The size of the sample used to calculate the spline.")]
        public int splineSampleSize = 200;

        public PlaceAtLocation.PlaceAtOptions Options;

        Spline spline;

        Vector3[] points;

        List<GameObject> instances = new List<GameObject>();

        private void Start()
        {
            points = new Vector3[path.locations.Length];

            for (var i = 0; i < points.Length; i++)
            {
                points[i] = path.locations[i].ToVector3();
            }

            spline = Misc.BuildSpline(path.splineType, points, splineSampleSize, path.alpha);

            var sample = spline.SamplePoints(objectCount);


            for (var i = 0; i < sample.Length; i++)
            {
                PlaceAtLocation.CreatePlacedInstance(prefab, LocationData.FromLocation(new Location(sample[i].z, sample[i].x, sample[i].y)), Options);
            }
        }
    }
}
