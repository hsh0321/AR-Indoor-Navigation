using GoogleARCore.Examples.HelloAR;
using GoogleARCore.HelloAR;
using UnityEngine;
using UnityEngine.EventSystems;

public class AndroidPlugin : MonoBehaviour
{
    public AndroidJavaObject ajo;

    public int a, b;

    private void Awake() {
        ajo = new AndroidJavaObject("com.example.unityplugintestaar.uPluginJavaAar");
    }

    public void SocketButton() {
        ClientSocketOpen("165.246.223.59", "9003");
    }

    public void ClientSocketOpen(string ip, string port)
    {
        ajo.Call("ClientSocketOpen", ip, port);
    }

    public void BlueScan()
    {
        ajo.Call("BluetoothScan");
    }
}
