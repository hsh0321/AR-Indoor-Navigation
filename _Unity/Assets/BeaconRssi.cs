using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class BeaconRssi : MonoBehaviour
{
    public AndroidPlugin anp;
    public UnityEngine.UI.Slider beacon1;
    public UnityEngine.UI.Slider beacon2;
    public UnityEngine.UI.Slider beacon3;
    public UnityEngine.UI.Slider beacon4;

    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;

    void Awake()
    {
        anp = GameObject.Find("connectButtonNull").GetComponent<AndroidPlugin>();
    }

    private void Update()
    {
        //beacon1.value -= 1;
        getAndroidRssi();
    }

    public void getRssi(string msg)
    {
        Debug.Log("unity rssi" + msg);
        string s = msg;
        if (!s.Equals(""))
        {
            if (s.Contains("50:51:A9:7B:33:CD"))
            {
                text1.text = msg.Substring(19);
                beacon1.value = int.Parse(msg.Substring(19));
            }
            else if (s.Contains("F8:30:02:25:14:8C"))
            {
                text2.text = msg.Substring(19);
                beacon2.value = int.Parse(msg.Substring(19));
            }
            else if (s.Contains("50:51:A9:7B:05:F5"))
            {
                text3.text = msg.Substring(19);
                beacon3.value = int.Parse(msg.Substring(19));
            }
            else if (s.Contains("F8:30:02:29:74:5C"))
            {
                text4.text = msg.Substring(19);
                beacon4.value = int.Parse(msg.Substring(19));
            }
        }
    }
    public void getAndroidRssi()
    {
        anp.ajo.Call("getRssi");
    }
}
