using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraIsMoving : MonoBehaviour {
    private Vector3 prevPosition;
    public bool isMoving;

	// Use this for initialization
	void Start () {
        prevPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        isMoving = false;
    }
	
	// Update is called once per frame
	void Update () {        
        if (prevPosition != Camera.main.transform.position)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        prevPosition = Camera.main.transform.position;        
    }
}
