using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Navegation nav;
    

    private void Start()
    {
        nav = GetComponent<Navegation>();
    }

    // Update is called once per frame
    void Update () {
		
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("HAIL");
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 13)) //layer 13 click detection
            {
                Debug.Log("NAIN");
                nav.SetDestinationPlayer(hit.point);
            }
        }

	}
}
