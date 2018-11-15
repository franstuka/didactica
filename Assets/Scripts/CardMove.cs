using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMove : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler {
	public GameObject Canvas;
	public Vector3 startPos;
	public float smoothTime = 0.05F;
    private Vector3 velocity = Vector3.zero;
	private bool first = true;

	GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
	
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
        transform.position = Vector3.SmoothDamp( transform.position,Input.mousePosition,ref velocity, 0.1f);
	
    }

    public void OnEndDrag(PointerEventData eventData)
    {
     
	
	
	    m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;
		List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);
		foreach (RaycastResult result in results)
        {		
			if(result.gameObject.tag == "cartaCalculo"){
				if(this.gameObject.tag == "carta"){
					this.tag = "cartaCalculo";
					this.transform.SetParent(result.gameObject.transform.parent);
					this.transform.SetSiblingIndex(result.gameObject.transform.GetSiblingIndex());
					result.gameObject.transform.SetSiblingIndex(result.gameObject.transform.GetSiblingIndex()+1);
					return;
				}

				if(this.gameObject.name != result.gameObject.name ){

				int indicieOriginal = this.transform.GetSiblingIndex();
				this.transform.SetSiblingIndex(result.gameObject.transform.GetSiblingIndex());
				result.gameObject.transform.SetSiblingIndex(indicieOriginal);
				return;
				}
				
			}
				
              if( result.gameObject.tag == "PanelCalculo"){
				  this.tag = "cartaCalculo";
				  this.transform.SetParent(result.gameObject.transform);
				  return;
			  }
			  
			  
			  
		}
		StartCoroutine("Return"); 
    }

	private void Awake() {
			
	}

	// Use this for initialization
	void Start () {
		 m_Raycaster = Canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = Canvas.GetComponent<EventSystem>();
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
	
        yield return null;

		}
		
	}
}
