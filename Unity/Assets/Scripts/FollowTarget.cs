using UnityEngine;
using System.Collections;
using GoogleARCore;

public class FollowTarget : MonoBehaviour
{
    public Transform targetToFollow;
    public Quaternion targetRot;
    public GameObject cameraTarget;
    public float distanceToTargetXZ = 7.0f;    
    public float heightOverTarget = 4.0f;
    public float heightSmoothingSpeed = 2.0f;
    public float rotationSmoothingSpeed = 2.0f;
    
    void LateUpdate()
    {
        if (!targetToFollow)
            return;
        Vector3 targetEulerAngles = targetRot.eulerAngles;

        float rotationToApplyAroundY = targetEulerAngles.y;
        float heightToApply = targetToFollow.position.y + heightOverTarget;
        
        float newCamRotAngleY = Mathf.LerpAngle(transform.eulerAngles.y, rotationToApplyAroundY, rotationSmoothingSpeed * Time.deltaTime);
        float newCamHeight = Mathf.Lerp(transform.position.y, heightToApply, heightSmoothingSpeed * Time.deltaTime);
        Quaternion newCamRotYQuat = Quaternion.Euler(0, newCamRotAngleY, 0);
        
        transform.position = targetToFollow.position;
        transform.position -= newCamRotYQuat * Vector3.forward * distanceToTargetXZ;
        transform.position = new Vector3(transform.position.x, newCamHeight, transform.position.z);
        transform.LookAt(targetToFollow);

        cameraTarget.transform.rotation = newCamRotYQuat; 
    }
}