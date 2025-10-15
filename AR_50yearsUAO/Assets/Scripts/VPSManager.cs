using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
//using Google.XR.ARCoreExtensions;
using System;

public class VPSManager : MonoBehaviour
{
    //private AREarthManager earthManager = new AREarthManager();

    [SerializeField]
    public struct GeospatialObject
    {
        public GameObject ObjectPrefab;
        public EarthPosition EarthPosition;
    }
    [SerializeField]
    public struct EarthPosition
    {
        public double latitude;
        public double longitude;
        public double altitude;
    }

    [SerializeField] private ARAnchorManager aRAnchorManager;
    [SerializeField] private List<GeospatialObject> geospatialObjects = new List<GeospatialObject>();

    void Start()
    {
        VerifyGeospatialSupport();
    }

    private void VerifyGeospatialSupport()
    {
        //var result = earthManager.IsGeospatialModeSupported(GeospatialMode.Enabled);
        //switch (result)
        //{
        //    case FeatureSupported.Supported:
        //        Debug.Log("Ready to use VPS");
        //        PlaceObjects();
        //        break;
        //    case FeatureSupported.Unknown:
        //        Debug.Log("Unknown...");
        //        Invoke("VerifyGeospatialSupport", 5.0f);
        //        break;
        //    case FeatureSupported.Unsupported:
        //        Debug.Log("VPS Unsupported");
        //        break;
        //}
    }

    private void PlaceObjects()
    {
        //if (earthManager.EarthTrackingState != TrackingState.Tracking)
        //{
        //    var geospatialPose = earthManager.CameraGeospatialPose;

        //    foreach (var obj in geospatialObjects)
        //    {
        //        var earthPosition = obj.EarthPosition;
        //        var objAnchor = ARAnchorManagerExtensions.AddAnchor(
        //            aRAnchorManager,
        //            earthPosition.latitude,
        //            earthPosition.longitude,
        //            earthPosition.altitude,
        //            Quaternion.identity);
        //        Instantiate(obj.ObjectPrefab, objAnchor.transform);
        //    }
        //}

        //else if (earthManager.EarthTrackingState == TrackingState.None)
        //{
        //    Invoke("PlaceObjects", 5.0f);
        //}
    }
}
