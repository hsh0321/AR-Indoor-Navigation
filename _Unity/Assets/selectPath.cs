using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectPath : MonoBehaviour
{
    public GameObject map;
    public bool isRed=false;
    public Camera cam;

    public void changeMat()
    {
        if (isRed == false)
        {
            map.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0));
            isRed = true;
        }
        else
        {
            map.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(194/255, 194/255, 194/255));
            isRed = false;
        }
    }
}
