using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMove : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler {
	public Vector3 startPos;
	 public float smoothTime = 0.05F;
    private Vector3 velocity = Vector3.zero;
	private bool first = true;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (first ){
			 startPos = transform.position;
			 first = false;
		}
		else{
			return;
		}
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Vector3.SmoothDamp( transform.position,Input.mousePosition,ref velocity, smoothTime);
	
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       StartCoroutine("Return");
    }

	private void Awake() {
			
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Return() 
	{	


     float tiempoRetorno = 1f;
    
     float correcionTiempo = 0.0075f;
 
     Vector3 newPosition;
    
     while (true) {
         
         newPosition = Vector3.SmoothDamp(transform.position, startPos, ref velocity, tiempoRetorno);
        
         if (newPosition == startPos) {
			print ("finish drag");
             yield break;
		 }
        transform.position = newPosition;
        
        smoothTime = tiempoRetorno - correcionTiempo;
		print (gameObject);
        yield return null;

		}
		
	}
}
