// Description : VirtualController_Pc : use for mobile inputs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class VirtualController_Pc : MonoBehaviour, IDragHandler, IPointerDownHandler {

	public Image 				backgroundImage;
	public Image 				virtualCenter;
	public Vector3 				inputVector;

	public PointerEventData 	eventData;

	void Start()
	{
		backgroundImage = GetComponent<Image> ();
	}
		
	public virtual void OnDrag(PointerEventData data)
	{
		eventData = data;
	}

	public virtual void OnPointerDown(PointerEventData data)
	{
		eventData = data;
	}
}
