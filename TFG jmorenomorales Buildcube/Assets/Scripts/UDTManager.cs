using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class UDTManager : MonoBehaviour, IUserDefinedTargetEventHandler
{
    UserDefinedTargetBuildingBehaviour udt_targetBuildingBehaviour;

    ObjectTracker objectTracker;
    DataSet dataSet;

    ImageTargetBuilder.FrameQuality udt_FrameQuality;

    public ImageTargetBehaviour targetBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        udt_targetBuildingBehaviour = GetComponent<UserDefinedTargetBuildingBehaviour>();
        if (udt_targetBuildingBehaviour)
        {
            udt_targetBuildingBehaviour.RegisterEventHandler(this);
        }
    }

    public void OnFrameQualityChanged(ImageTargetBuilder.FrameQuality frameQuality)
    {
        udt_FrameQuality = frameQuality;
        //throw new System.NotImplementedException();
    }

    public void OnInitialized()
    {
        objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        if(objectTracker != null)
        {
            dataSet = objectTracker.CreateDataSet();
            objectTracker.ActivateDataSet(dataSet);
        }
    }

    public void OnNewTrackableSource(TrackableSource trackableSource)
    {
        objectTracker.DeactivateDataSet(dataSet);

        dataSet.CreateTrackable(trackableSource, targetBehaviour.gameObject);

        objectTracker.ActivateDataSet(dataSet);

        udt_targetBuildingBehaviour.StartScanning();

        //throw new System.NotImplementedException();
    }

    public void BuildTarget()
    {
        if(udt_FrameQuality == ImageTargetBuilder.FrameQuality.FRAME_QUALITY_HIGH)
        {
            udt_targetBuildingBehaviour.BuildNewTarget("1", targetBehaviour.GetSize().x);
        }
    }
}