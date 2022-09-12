using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;


    // Update is called once per frame
    private void Update()
    {
        transform.position = target.position + offset;
    }
}
