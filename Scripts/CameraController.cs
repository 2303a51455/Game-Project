using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public float cameraDistance = 2.0f;
    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = target.transform.position - target.transform.forward * cameraDistance;
        transform.LookAt (target.transform.position);
        transform.position = new Vector3 (transform.position.x, transform.position.y + 3.5f, transform.position.z);
  
        //transform.rotation = target.rotation;

    }
}
