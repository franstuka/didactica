using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrpit : MonoBehaviour {

	public Transform player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.position.x,transform.position.y, player.position.z);
		

	}
}
