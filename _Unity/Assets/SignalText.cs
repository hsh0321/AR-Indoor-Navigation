using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalText : MonoBehaviour
{
    // Start is called before the first frame update

    public LocRefresh rf;
    string s;
    void Start()
    {
        s = "";
        this.GetComponent<TextMesh>().text = "a";
        rf = GameObject.Find("Refresh").GetComponent<LocRefresh>();
    }


    //Update is called once per frame
    void Update()
    {
        s = "("+rf.scanX.ToString()+ "," + rf.scanY.ToString()+")";
        this.GetComponent<TextMesh>().text = s;
    }
}
