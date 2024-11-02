using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

public class CameraControl : MonoBehaviour
{
    public static bool MovementEnabled = true;
	static Transform me; 
	Vector3 mouseOrigin;

	//Movement controls
	float flySpeed = 0.05f;
	float accelerationRatio = 0.75f;
	float slowDownRatio = 0.2f;

	//Mouse lookaround controls
	float rotateSpeed = 90f;
	float mouseX;
	float mouseY;
	float mouse;
	
    void Awake()
    {
		me = gameObject.transform;
	}
    
	void Update()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && MovementEnabled)
		{
			//Mouse look around
            if(Input.GetMouseButton(1))
            {
	            if(SettingsData.LockMouseOnCameraMovement)
		            Cursor.lockState = CursorLockMode.Locked;
	            if (!Input.GetKey(KeyCode.LeftShift))
	            {
		            mouseX = SettingsData.InvertCamRotX ? -Input.GetAxis("Mouse X") : Input.GetAxis("Mouse X");
		            mouseY = SettingsData.InvertCamRotY ? -Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
		            transform.RotateAround (Vector3.zero,transform.up,mouseX * rotateSpeed * Time.deltaTime);
		            transform.RotateAround(Vector3.zero, transform.right, mouseY * rotateSpeed * Time.deltaTime);
	            }
	            else
	            {
		            mouse = Input.GetAxis("Mouse X");
		            transform.RotateAround(Vector3.zero, transform.forward, mouse * rotateSpeed * 2 * Time.deltaTime);
	            }
            }
            if(Input.GetMouseButtonUp(1))
            {
            	Cursor.lockState = CursorLockMode.None;
            }

            //Mouse drag around
            if (Input.GetMouseButton(2))
            {
	            if(SettingsData.LockMouseOnCameraMovement)
		            Cursor.lockState = CursorLockMode.Locked;
	            float mouseX = SettingsData.InvertDrag ? -Input.GetAxis("Mouse X") : Input.GetAxis("Mouse X");
	            float mouseY = SettingsData.InvertDrag ? -Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
	            transform.Translate(Vector3.right * (flySpeed * mouseX));
	            transform.Translate(Vector3.up * (flySpeed * mouseY));
            }
            if (Input.GetMouseButtonUp(2))
            {
	            Cursor.lockState = CursorLockMode.None;
            }
    
            //Keyboard fly around movement
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
            
            if (Input.GetKey(KeyCode.E))
            {
            	transform.Translate(Vector3.down * flySpeed/2);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
            	transform.Translate(Vector3.up * flySpeed/2);
            }
            
            //Scroll wheel zoom
            if (Input.mouseScrollDelta.y != 0)
            {
	            float scroll = SettingsData.InvertScroll ? -Input.mouseScrollDelta.y : Input.mouseScrollDelta.y;
	            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	            transform.Translate((scroll * 0.4f * ray.direction), Space.World);
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
