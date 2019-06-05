using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !ARGPS_USE_VUFORIA
using UnityEngine.XR.ARFoundation;
#else
using Vuforia;
#endif

public class ARLocationPlaneManager
{
#if ARGPS_USE_VUFORIA
    PlaneFinderBehaviour m_PlaneFinder;

    float minDistance = 2.0f;

    List<Vector3> planes = new List<Vector3>();
    public ARLocationPlaneManager(PlaneFinderBehaviour planeFinder)
    {
        minDistance = ARLocation.config.VuforiaGroundHitTestDistance;

        planeFinder.OnAutomaticHitTest.AddListener(OnHitTest);
        planeFinder.OnInteractiveHitTest.AddListener(OnHitTest);
    }

    private bool ShouldAddHitTest(Vector3 pos)
    {
        foreach (var item in planes)
        {
            if (Vector3.Distance(pos, item) < minDistance)
            {
                return false;
            }
        }

        return true;
    }

    private void OnHitTest(HitTestResult arg0)
    {
        var hitPosition = arg0.Position;

        if (ShouldAddHitTest(hitPosition))
        {
            planes.Add(hitPosition);
        }
    }

    public float GetClosestPlaneY(Vector3 position)
    {
        if (planes.Count == 0)
        {
            return ARLocation.config.InitialGroundHeightGuess;
        }

        var distance = -1.0f;
        var y = 0.0f;
        foreach (var plane in planes)
        {
            var planeCenter = plane;

            var newDistance = Vector3.Distance(position, planeCenter);

            if (distance < 0 || newDistance < distance)
            {
                distance = newDistance;
                y = planeCenter.y;
            }
        }

        return y;
    }

    public float GetClosestPlaneHeight(Vector3 position, float min = -1.0f)
    {
        var y = GetClosestPlaneY(position);

        if (y < min)
        {
            return y;
        }

        return min;
    }

#else
    ARPlaneManager m_PlaneManager;

    List<ARPlane> planes = new List<ARPlane>();

    public ARLocationPlaneManager(ARPlaneManager planeManager)
    {
        m_PlaneManager = planeManager;

        m_PlaneManager.planesChanged += M_PlaneManager_planesChanged;
    }

    private void M_PlaneManager_planesChanged(ARPlanesChangedEventArgs args)
    {
        var added = args.added;
        var removed = args.removed;

        foreach (var plane in added)
        {
            planes.Add(plane);
        }

        foreach (var plane in removed)
        {
            Plane_removed(plane);
        }
    }

    private void Plane_removed(ARPlane obj)
    {
        planes.Remove(obj);
    }

    public float GetClosestPlaneY(Vector3 position)
    {
        if (planes.Count == 0)
        {
            return ARLocation.config.InitialGroundHeightGuess;
        }

        var distance = -1.0f;
        var y = 0.0f;
        foreach (var plane in planes)
        {
            var planeCenter = plane.center;

            var newDistance = Vector3.Distance(position, planeCenter);

            if (distance < 0 || newDistance < distance)
            {
                distance = newDistance;
                y = planeCenter.y;
            }
        }

        return y;
    }

    public float GetClosestPlaneHeight(Vector3 position, float min = -1.0f)
    {
        var y = GetClosestPlaneY(position);

        if (y < min)
        {
            return y;
        }

        return min;
    }
#endif

}
