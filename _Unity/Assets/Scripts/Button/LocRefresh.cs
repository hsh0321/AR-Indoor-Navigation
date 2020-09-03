using GoogleARCore.Examples.HelloAR;
using GoogleARCore.HelloAR;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocRefresh : MonoBehaviour,IPointerClickHandler
{
    public bool isOnClick = false;
    public float scanX, scanY;
    public AndroidPlugin anp;
    //public TrailRenderer tr

    public void OnPointerClick(PointerEventData eventData)
    {
        getMsg();
        isOnClick = true;
    }

    void Awake()
    {
        anp = GameObject.Find("connectButtonNull").GetComponent<AndroidPlugin>();
    }

    public void getMessage(string msg)
    {
        string s = msg;
        if (s.Equals(" ") == true)
        {
            scanX = 0;
            scanY = 0;

            Debug.Log("HWang, Data is Null.");
        }
        else
        {
            string[] result = s.Split(new char[] { ' ' });
            scanX = float.Parse(result[0]);
            scanY = float.Parse(result[1]);
            Debug.Log("Hwang, Current :" + scanX.ToString() + " " + scanY.ToString());
        }
    }
    public void getMsg()
    {
        anp.ajo.Call("getMsg");
    }
}
