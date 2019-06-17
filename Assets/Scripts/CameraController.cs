using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    public GameObject point;

    public float rotationY = 0;
    
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void Update()
    {
        //transform.position = player.transform.position + offset;
//        Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        
        rotationY = Input.GetAxis("Mouse X") * 10.0f;
        //transform.Rotate(0.0f, rotationY, 0.0f);
        transform.RotateAround(point.transform.position, new Vector3(0, 1, 0), rotationY);
        
        rotationY = transform.localEulerAngles.y;
        transform.localEulerAngles = new Vector3(17, rotationY, 0);
    }
}
