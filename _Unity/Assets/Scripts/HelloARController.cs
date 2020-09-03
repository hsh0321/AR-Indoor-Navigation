namespace GoogleARCore.HelloAR
{
    using UnityEngine;
    using GoogleARCore;
    using UnityEngine.UI;
    public class HelloARController : MonoBehaviour
    {
        public Text camPoseText,camPoseText2;
        //public Text testText;
        public GameObject myCam;
        public GameObject cameraTarget;   //SpherePointer
        private Vector3 m_prevARPosePosition;
        private bool trackingStarted = false;

        float sX, sY;

        public LocRefresh rf;

        public void Start()
        {
            m_prevARPosePosition = Vector3.zero;
            rf = GameObject.Find("Refresh").GetComponent<LocRefresh>();
        }
        public void Update()
        {
            _QuitOnConnectionErrors();


            if (rf.isOnClick == true) // Refresh 버튼 onClick 시
            {
                Debug.Log("Button is Onclick");
                rf.isOnClick = false;
                sX = rf.scanX; 
                sY = rf.scanY;
                // Native Android에서 측정한 RSSI Input으로 나온 좌표 Output
                cameraTarget.transform.position = new Vector3(sX, 0.0f, sY);
                myCam.transform.position = new Vector3(sX,0.0f,sY);
                Debug.Log("Hwang : After Send" + sX.ToString() + " " + sY.ToString());
            }

            if (Session.Status != SessionStatus.Tracking)
            {
                trackingStarted = false;              
                camPoseText2.gameObject.SetActive(true);
                const int LOST_TRACKING_SLEEP_TIMEOUT = 15;
                Screen.sleepTimeout = LOST_TRACKING_SLEEP_TIMEOUT;
                return;
            }
            else
            {
                // Clear camPoseText if no error
                //if(camPoseText != null)
                    //camPoseText.text = "CamPose: " + cameraTarget.transform.position;
                camPoseText.text = "" + cameraTarget.transform.position;
                camPoseText2.gameObject.SetActive(false);
            }

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            Vector3 currentARPosition = Frame.Pose.position;
            if (!trackingStarted)
            {
                trackingStarted = true;
                m_prevARPosePosition = Frame.Pose.position;
            }
            //Remember the previous position so we can apply deltas
            Vector3 deltaPosition = currentARPosition - m_prevARPosePosition;
            m_prevARPosePosition = currentARPosition;
            if (cameraTarget != null)
            {
                cameraTarget.transform.Translate(deltaPosition.x, 0.0f, deltaPosition.z,Space.World);

                //myCam.GetComponent<FollowTarget>().targetRot = Frame.Pose.rotation;
                //cameraTarget.GetComponent<FollowTarget>().targetRot = Frame.Pose.rotation;  
            }
        }
        private void _QuitOnConnectionErrors()
        {
            /*
            // Do not update if ARCore is not tracking.
            if (Session.ConnectionState == SessionConnectionState.DeviceNotSupported)
            {
                camPoseText.text = "This device does not support ARCore.";
                Application.Quit();
            }
            else if (Session.ConnectionState == SessionConnectionState.UserRejectedNeededPermission)
            {
                camPoseText.text = "Camera permission is needed to run this application.";
                Application.Quit();
            }
            else if (Session.ConnectionState == SessionConnectionState.ConnectToServiceFailed)
            {
                camPoseText.text = "ARCore encountered a problem connecting.  Please start the app again.";
                Application.Quit();
            }*/
        }
    }
}