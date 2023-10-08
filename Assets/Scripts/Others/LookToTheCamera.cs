using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToTheCamera : MonoBehaviour
{
    private Transform mainCamBody;

    // Start is called before the first frame update
    void Start()
    {
        mainCamBody = GameObject.Find("Main Camera").transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamBody.rotation * Vector3.back, mainCamBody.rotation * Vector3.up);
        //transform.LookAt(mainCamBody.position);
    }
}
