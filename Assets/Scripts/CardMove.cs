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
	public Transform originalParent;
	private int indicieOriginal;
	GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
	
    public void OnBeginDrag(PointerEventData eventData)
    {
      //  if (first ){
			 startPos = transform.position;
			 first = false;
			originalParent = transform.parent;
			indicieOriginal = this.transform.GetSiblingIndex();
		//}
		//else{
		//	return;
		//}
    }

    public void OnDrag(PointerEventData eventData)

    {
		this.tag = "cartaflotante";
		transform.SetParent(GameObject.Find("temporalParent").transform);
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
				if(this.gameObject.tag == "cartaflotante"){
					this.tag = "cartaCalculo";
					this.transform.SetParent(result.gameObject.transform.parent);
					this.transform.SetSiblingIndex(result.gameObject.transform.GetSiblingIndex());
					result.gameObject.transform.SetSiblingIndex(result.gameObject.transform.GetSiblingIndex()+1);
					return;
				}

				if(this.gameObject.name != result.gameObject.name ){

				indicieOriginal = this.transform.GetSiblingIndex();
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
			   if( result.gameObject.tag == "PanelCartasNumero"){
				  this.tag = "carta";
				  this.transform.SetParent(result.gameObject.transform);
				  return;
			  }
			  if( result.gameObject.name == "PanelCartasSigno"){
				  this.tag = "carta";
				  this.transform.SetParent(result.gameObject.transform);
				  return;
			  }
	
			  
		}
		this.transform.SetParent(originalParent);
		this.transform.SetSiblingIndex(indicieOriginal);
//StartCoroutine("Return"); 
    }

	private void Awake() {
			Canvas = GameObject.Find("CanvasCombate"); 
	}

	// Use this for initialization
	void Start () {
		 m_Raycaster = Canvas.GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = Canvas.GetComponent<EventSystem>();
		GetComponent<RectTransform>().localScale = new Vector3(0.75f, 0.75f, 1.0f);
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
