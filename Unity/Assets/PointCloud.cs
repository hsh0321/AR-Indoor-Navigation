using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCloud : MonoBehaviour
{
    private Mesh mMesh;
    private Vector3[] mPoints = new Vector3[3000];

    // Start is called before the first frame update
    void Start()
    {
        mMesh = FindObjectOfType<MeshFilter>().mesh;
        mMesh.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Frame.PointCloud.IsUpdatedThisFrame)
        {
            for(int i = 0; i < Frame.PointCloud.PointCount; i++)
            {
                mPoints[i] = Frame.PointCloud.GetPoint(i);
            }

            int[] indices = new int[Frame.PointCloud.PointCount];
            for(int i = 0; i < Frame.PointCloud.PointCount; i++)
            {
                indices[i] = i;
            }

            mMesh.Clear();
            mMesh.vertices = mPoints;
            mMesh.SetIndices(indices, MeshTopology.Points, 0);
        }

    }
}
