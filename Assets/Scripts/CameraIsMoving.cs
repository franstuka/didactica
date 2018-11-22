using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIsMoving : MonoBehaviour {
    private Vector3 prevPosition;
    private Quaternion prevRotation;
    public bool isMoving;

	// Use this for initialization
	void Start () {
        prevPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        prevRotation = Quaternion.Euler(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z);
        isMoving = false;
    }
	
	// Update is called once per frame
	void Update () {        
        if (prevPosition != Camera.main.transform.position || prevRotation != Camera.main.transform.rotation)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        prevPosition = Camera.main.transform.position;
        prevRotation = Camera.main.transform.rotation;
    }
}
