using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject camera;
    public GameObject player;
    
    private Vector3 offset;
    
    public void Start()
    {
        offset = transform.position - player.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.Euler(0.0f, camera.transform.rotation.eulerAngles.y, 0.0f);
    }
}
