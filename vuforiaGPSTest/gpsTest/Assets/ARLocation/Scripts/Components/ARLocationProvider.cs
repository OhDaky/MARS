using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARLocationProvider : Singleton<ARLocationProvider> {

    public ILocationProvider provider { get; private set; }

    [Tooltip("The options for the Location Provider.")]
    public LocationProviderOptions LocationOptions;

    [Tooltip("The maximum number of location updates. The updates will be paused after this amount. Zero means there is no limit and " +
        "the updates won't be paused automatically.")]
    public uint MaxNumberOfMeasurements = 10;

    [Tooltip("A mock location for use inside the editor.")]
    public Location MockLocation;

    [Tooltip("The data of mock location. If presend, overrides the Mock Location above.")]
    public LocationData MockLocationData;

    [Tooltip("The maximum wait time to wait for location initialization.")]
    public uint MaxWaitTime = 20;

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

    public DVector3 currentDisplacement
    {
        get
        {
            return provider.currentDisplacement;
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
            MockLocation = MockLocationData.location;
        }

        (provider as MockLocationProvider).mockLocation = MockLocation;
#else
        provider = new UnityLocationProvider();
#endif

        Debug.Log("[ARLocationProvider]: Using provider " + provider.name);

        provider.options = LocationOptions;

        provider.LocationUpdated += Provider_LocationUpdated;
        provider.CompassUpdated += Provider_CompassUpdated;
    }

    IEnumerator Start () {
        yield return StartCoroutine(provider.Start(MaxWaitTime));
    }

    private void Provider_CompassUpdated(HeadingReading heading, HeadingReading lastReading)
    {
        if (onCompassUpdated != null)
        {
            onCompassUpdated(heading, lastReading);
        }
    }

    private void Provider_LocationUpdated(LocationReading currentLocation, LocationReading lastLocation, DVector3 displacement)
    {
        measurementCount++;

        if ((MaxNumberOfMeasurements > 0) && (measurementCount >= MaxNumberOfMeasurements))
        {
            provider.Pause();
        }

        if (onLocationUpdated != null)
        {
            onLocationUpdated(currentLocation, lastLocation, displacement);
        }
    }

    void Update () {
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
