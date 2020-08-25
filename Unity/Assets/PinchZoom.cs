using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.5f;       // perspective mode.
    public float orthoZoomSpeed = 0.5f;        //  orthographic mode.
    public float moveSpeed;


    Vector2 prevPos = Vector2.zero;
    float prevDistance = 0f;
    public Camera c;
    public Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;        
    }

    void Update()
    {

        if(Input.touchCount == 1)
        {
            if(prevPos == Vector2.zero)
            {
                prevPos = Input.GetTouch(0).position;
                return;
            }
            Vector2 dir = (Input.GetTouch(0).position - prevPos).normalized;
            Vector3 vec = new Vector3(dir.x,0,dir.y);

            cam.position -= vec * moveSpeed * Time.deltaTime;
            prevPos = Input.GetTouch(0).position;
        }
        else if (Input.touchCount == 2) // 손가락으로 줌인/아웃의 경우 무조건 2손가락이 터치가 되어야 하기 때문에 Count = 2일 경우만 동작
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0); //첫번째 손가락 좌표
            Touch touchOne = Input.GetTouch(1); //두번째 손가락 좌표

            // deltaposition은 deltatime과 동일하게 delta만큼 시간동안 움직인 거리를 말한다.

            // 현재 position에서 이전 delta값을 빼주면 움직이기 전의 손가락 위치가 된다.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // 현재와 과거값의 움직임의 크기를 구한다.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // 두 값의 차이는 즉 확대/축소할때 얼만큼 많이 확대/축소가 진행되어야 하는지를 결정한다.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the c is orthographic...
            if (c.orthographic)
            {
                // ... change the orthographic size based on the change in distance between the touches.
                c.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                // Make sure the orthographic size never drops below zero.
                c.orthographicSize = Mathf.Max(c.orthographicSize, 0.1f);
            }
            else
            {
                // Otherwise change the field of view based on the change in distance between the touches.
                c.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                // Clamp the field of view to make sure it's between 0 and 180.
                c.fieldOfView = Mathf.Clamp(c.fieldOfView, 30.1f, 139.9f);
            }

        }
    }
}