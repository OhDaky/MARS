using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This scriptable object holds the global configuration data for the AR + GPS
/// Location plugin.
/// </summary>
[CreateAssetMenu(fileName = "ARLocationConfig", menuName = "ARLocation/ARLocationConfig")]
public class ARLocationConfig : ScriptableObject {

    public static string Version
    {
        get
        {
            return "v2.8.0";
        }
    }

    public enum ARLocationDistanceFunc {
        Haversine,
        PlaneSpherical,
        PlaneEllipsoidalFCC
    };

    [Tooltip("The earth radius, in kilometers, to be used in distance calculations.")]
    public double EarthRadiusInKM = 6372.8;

    [Tooltip("The distance function used to calculate geographical distances.")]
    public ARLocationDistanceFunc DistanceFunction = ARLocationDistanceFunc.Haversine;

    [Tooltip("The initial ground height guess, relative from the device position.")]
    public float InitialGroundHeightGuess = -1.4f;

    [Tooltip("The distance between Vuforia ground plane hit tests. Lower will be more precise but will affect performance.")]
    public float VuforiaGroundHitTestDistance = 4.0f;

    [Tooltip("If true, use a native location module instead of Unity's builtin location services.")]
    public bool UseNativeLocationModule = false;

    [Tooltip("If true, use Vuforia instead of ARFoundation.")]
    public bool UseVuforia = false;
}
