using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    public static bool MovementEnabled = true;
	static Transform me;
	Plane plane = new(Vector3.forward, 0);


	//Movement controls
	float flySpeed = 0.05f;
	float accelerationRatio = 0.75f;
	float slowDownRatio = 0.2f;

	//Mouse lookaround controls
	float rotateSpeed = 90f;
	float mouseX;
	float mouseY;
    void Awake()
    {
		me = gameObject.transform;
	}
    
	void Update()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && MovementEnabled)
		{
			//Mouse lookaround
            if(Input.GetMouseButton(1))
            {
            	Cursor.lockState = CursorLockMode.Locked;
            	mouseX = SettingsData.InvertCamRotX ? -Input.GetAxis("Mouse X") : Input.GetAxis("Mouse X");
            	mouseY = SettingsData.InvertCamRotY ? -Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
	            if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
		            transform.RotateAround (Vector3.zero,new Vector3(0.0f, mouseX, 0.0f),rotateSpeed * Time.deltaTime);
	            //TODO: figure out rotation stuff
            }
            if(Input.GetMouseButtonUp(1))
            {
            	Cursor.lockState = CursorLockMode.None;
            }

            if (Input.GetMouseButton(2))
            { Debug.Log(Cursor.lockState.ToString());
	            Cursor.lockState = CursorLockMode.Locked;
	            Debug.Log(Cursor.lockState.ToString());
	            transform.Translate(Vector3.forward * flySpeed * Input.GetAxis("Mouse X"));
	            transform.Translate(Vector3.right * flySpeed * Input.GetAxis("Mouse Y"));
            }
            if (Input.GetMouseButtonUp(2))
            {
	            Cursor.lockState = CursorLockMode.None;
            }
    
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
            	flySpeed *= accelerationRatio;
            }
    
            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
            	flySpeed /= accelerationRatio;
            }
    
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
            	flySpeed *= slowDownRatio;
            }
    
            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
            	flySpeed /= slowDownRatio;
            }
    
            if (Input.GetAxis("Vertical") != 0)
            {
            	transform.Translate(Vector3.forward * flySpeed * Input.GetAxis("Vertical"));
            }
    
            if (Input.GetAxis("Horizontal") != 0)
            {
            	transform.Translate(Vector3.right * flySpeed * Input.GetAxis("Horizontal"));
            }

            if (Input.mouseScrollDelta.y != 0)
            {
	            if (Input.mouseScrollDelta.y > 0)
	            {
		            Debug.Log("scrolling");
		            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		            float distance1;
		            Vector3 point1 = new Vector3();
		            Vector3 point2 = new Vector3();
		            if (plane.Raycast(ray, out distance1))
		            {
			            point1 = ray.GetPoint(distance1);
		            }
		            transform.Translate(Input.mouseScrollDelta.y * 0.2f * Vector3.forward);
		            Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
		            float distance2;
		            if (plane.Raycast(ray2, out distance2))
		            {
			            point2 = ray.GetPoint(distance2);
		            }
		            Vector3 difference = point1 - point2;
		            transform.position += difference;
		            //transform.Translate(difference);
	            }
	            else
	            {
		            transform.Translate(Input.mouseScrollDelta.y * 0.2f * Vector3.forward);
	            }
            }
    
            if (Input.GetKey(KeyCode.E))
            {
            	transform.Translate(Vector3.up * flySpeed/5);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
            	transform.Translate(Vector3.down * flySpeed/5);
            }
		}
	}
	
	public static void SetCameraGlobally(Vector3 pos)
	{
		//TODO: LERP and fix stuff
		//me.position = new Vector3(pos.x, pos.y, pos.z-10);
	}

	public static Transform GetCameraTransform()
    {
		return me.transform;
    }
}
