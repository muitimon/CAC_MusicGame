using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Kinect;
using System.Text;

public class KinectManager : MonoBehaviour
{
    // Kinect 
    private KinectSensor kinectSensor;

    private BodyFrameReader bodyFrameReader;
    private int bodyCount;
    private Body[] bodies;

    /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
    private List<GestureDetector> gestureDetectorList = null;
    List<List<GestureDetector>> gestureList = null;

    [System.SerializableAttribute]
    public class gestureData
    {
        public string fileName;
        public string gestureName;
        public bool enable;
        public int gestureNumber = 0;
        private bool moving = false;

        public gestureData(string fn, string gn)
        {
            fileName = fn;
            gestureName = gn;
            enable = true;
        }
    }
    [SerializeField]
    //public List<gestureData> dataList = new List<gestureData>() { new gestureData("Seated.gbd", "jump3"), new gestureData("rolling.gbd", "rollingProgress_Right") };
    public List<gestureData> dataList = new List<gestureData>() { new gestureData("rolling.gbd", "rollingProgress_Right") };
    //public List<gestureData> dataList = new List<gestureData>() { new gestureData("Seated.gbd", "jump3") };

    // Use this for initialization
    void Start()
    {
        // get the sensor object
        this.kinectSensor = KinectSensor.GetDefault();

        if (this.kinectSensor != null)
        {
            this.bodyCount = this.kinectSensor.BodyFrameSource.BodyCount;
            
            // body data
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // body frame to use
            this.bodies = new Body[this.bodyCount];

            // initialize the gesture detection objects for our gestures
            this.gestureList = new List<List<GestureDetector>>();
            for (int gestureIndex = 0; gestureIndex < dataList.Count; gestureIndex++)
            {
                if (dataList[gestureIndex].enable == true)
                {
                    this.gestureDetectorList = new List<GestureDetector>();
                    for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
                    {
                        this.gestureDetectorList.Add(new GestureDetector(this.kinectSensor, dataList[gestureIndex].fileName, dataList[gestureIndex].gestureName));
                    }
                    this.gestureList.Add(new List<GestureDetector>(gestureDetectorList));
                    this.gestureDetectorList.Clear();                   ////////////////////////////////////////////////////////////////////////////////////////////////
                }
            }

            // start getting data from runtime
            this.kinectSensor.Open();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ensure the readers are valid
        if (this.bodyFrameReader != null)
        {
            // process bodies
            bool newBodyData = false;
            using (BodyFrame bodyFrame = this.bodyFrameReader.AcquireLatestFrame())
            {
                if (bodyFrame != null)
                {
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    newBodyData = true;
                }
            }

            if (newBodyData)
            {
                // update gesture detectors with the correct tracking id
                for (int bodyIndex = 0; bodyIndex < this.bodyCount; bodyIndex++)
                {
                    var body = this.bodies[bodyIndex];
                    if (body != null)
                    {
                        var trackingId = body.TrackingId;

                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        for (int gestureIndex = 0; gestureIndex < dataList.Count; gestureIndex++)///////////////////////////////////// not complate...
                        {
                            if (trackingId != this.gestureList[gestureIndex][bodyIndex].TrackingId)
                            {
                                this.gestureList[gestureIndex][bodyIndex].TrackingId = trackingId;

                                // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                                // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                                this.gestureList[gestureIndex][bodyIndex].IsPaused = (trackingId == 0);
                                this.gestureList[gestureIndex][bodyIndex].OnGestureDetected += CreateOnGestureHandler(bodyIndex, gestureList[gestureIndex][bodyIndex]);
                            }
                        }
                    }
                }
            }
        }
    }

    private EventHandler<GestureEventArgs> CreateOnGestureHandler(int bodyIndex, GestureDetector detector)
    {
        return (object sender, GestureEventArgs e) => OnGestureDetected(sender, e, bodyIndex, detector);
    }

    private void OnGestureDetected(object sender, GestureEventArgs e, int bodyIndex, GestureDetector detector)
    {
        var isDetected = e.IsBodyTrackingIdValid && e.IsGestureDetected;

        StringBuilder text = new StringBuilder(string.Format("Gesture Detected? {0}\n", isDetected));
        text.Append(string.Format("Confidence: {0}\n", e.DetectionConfidence));
        
        //print(" =  " + bodyIndex + " is : " + e.DetectionConfidence);
        detector.gestureCheck(e.DetectionConfidence);
    }

    void OnGUI()
    {
    }

    void OnApplicationQuit()
    {
        if (this.bodyFrameReader != null)
        {
            this.bodyFrameReader.Dispose();
            this.bodyFrameReader = null;
        }

        if (this.kinectSensor != null)
        {
            if (this.kinectSensor.IsOpen)
            {
                this.kinectSensor.Close();
            }

            this.kinectSensor = null;
        }
    }

}
