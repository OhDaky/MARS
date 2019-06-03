// Copyright (C) 2018 Daniel Fortes <daniel.fortes@gmail.com>
// All rights reserved.
//
// See LICENSE.TXT for more info


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if !ARGPS_USE_VUFORIA
using UnityEngine.XR.ARFoundation;
#endif

namespace ARLocation
{
    using Utils;

    /// <summary>
    /// This Component manages all positioned GameObjects, synchronizing their world position in the scene
    /// with their geographical coordinates. This is done by calculating their position relative to the device's position.
    /// 
    /// Should be placed in a GameObject called "ARLocationRoot", whose parent is the "AR Session Origin".
    /// </summary>
    [RequireComponent(typeof(ARLocationOrientation))]
    [RequireComponent(typeof(ARLocationProvider))]
    [DisallowMultipleComponent]
    [AddComponentMenu("AR+GPS/AR Location Manager")]
    public class ARLocationManager : Singleton<ARLocationManager>
    {
        public PlaneManager PlaneManager
        {
            get
            {
                return arLocationplaneManager;
            }
        }

        private PlaneManager arLocationplaneManager;

        // Use this for initialization
        void Start()
        {
            Application.targetFrameRate = 60;

            // Register callback for handling new camera frames
#if !ARGPS_USE_VUFORIA
            var arPlaneManager = GameObject.FindObjectOfType<ARPlaneManager>();

            if (arPlaneManager != null)
            {
                arLocationplaneManager = new PlaneManager(GameObject.FindObjectOfType<ARPlaneManager>());
            }
#else
        var planeFinder = GameObject.FindObjectOfType<Vuforia.PlaneFinderBehaviour>();

        if (planeFinder != null)
        {
            arLocationplaneManager = new ARLocationPlaneManager(planeFinder);
        }
#endif
        }
    }
}
