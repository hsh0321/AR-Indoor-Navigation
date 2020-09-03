using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;

public class PathFinding : MonoBehaviour
{
    public Camera cam;
    public Camera arCam; // pose
    
    public NavMeshAgent agent;

    public GameObject spherePointer;
    public GameObject obj;
    public GameObject pointer;
    public GameObject button;

    
    GameObject arrive;
    //GameObject[] tempobj;
    Stack<GameObject> arrow;

    int k;
    bool isMoving;
    int cnt;

    float distanceTimer;
    void Start()
    {
        cnt = 0;
        k = -1;
        isMoving = false;
        distanceTimer = 0.0f;
        //tempobj = new GameObject[10000];
        arrow = new Stack<GameObject>();
    }

    void Update()
    {
        if (arCam.gameObject.active == false)
        {
            selectPath isR = GameObject.Find("Select").GetComponent<selectPath>();
            if (Input.touchCount == 1 && isR.isRed==true)
            {
                if (isMoving == false)
                {
                    if (!IsPointerOverUIObject())
                    {

                        Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            agent.transform.position = spherePointer.transform.position;
                            agent.SetDestination(hit.point);

                            isMoving = true;

                            Destroy(arrive);
                            while (arrow.Count > 0)
                            {
                                Destroy(arrow.Pop());
                            }
                        }
                    }
                }
            }
            else
            {
                if (isMoving == true)
                {
                    if (agent.velocity.x != 0 || agent.velocity.z != 0) // 이동중
                    {
                        if (agent.remainingDistance > 2 && distanceTimer > 2.5f) // 타이머
                        {
                            if (cnt > 0)
                            {
                                arrow.Push(Instantiate(obj, agent.transform.position, this.transform.rotation));
                                ChangeLayersRecursively(arrow.Peek().transform, "TrailRenderer");
                                arrow.Peek().transform.Rotate(0, 180, 0);
                            }
                            distanceTimer = 0.0f;
                            cnt++;
                        }
                    }
                    else if (!agent.hasPath) // 도착
                    {
                        arrive = Instantiate(pointer, agent.destination, Quaternion.identity); // 생성하는 부분
                        ChangeLayersRecursively(arrive.transform, "TrailRenderer");
                        StartCoroutine("TurnArrive"); // 회전 코루틴
                                                      //arrive.transform.Translate(0, 0.8f, 0);
                        isMoving = false;
                        cam.gameObject.SetActive(false);
                        arCam.gameObject.SetActive(true);
                        isR.changeMat();
                        button.gameObject.SetActive(false);
                        cnt = 0;
                    }
                    distanceTimer += agent.speed * Time.deltaTime; // 이동거리
                }
            }
        }
    }

    private IEnumerator TurnArrive() {
        while (arrive)
        {
            arrive.transform.Rotate(0, 2.0f, 0, Space.Self); // 회전
            yield return new WaitForSeconds(0.05f);
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public static void ChangeLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            ChangeLayersRecursively(child, name);
        }
    }
}
