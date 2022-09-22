using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForCam : MonoBehaviour
{
    public GameObject MainCam;
    public Transform PlayerTransform;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerTransform.position;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.transform.Rotate(Vector3.up,45);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.transform.Rotate(Vector3.up, -45);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(MainCam.GetComponent<Camera>().fieldOfView > 15)
            MainCam.GetComponent<Camera>().fieldOfView--;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (MainCam.GetComponent<Camera>().fieldOfView < 35)
                MainCam.GetComponent<Camera>().fieldOfView++;
        }
    }
}
