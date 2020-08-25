using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bleScan : MonoBehaviour
{
    // Start is called before the first frame update
    public AndroidPlugin connectBtn;
    void Awake()
    {
        connectBtn = GameObject.Find("connectButton").GetComponent<AndroidPlugin>();
    }

    public void BlueScan()
    {
        connectBtn.ajo.Call("BluetoothScan");
     }

}
