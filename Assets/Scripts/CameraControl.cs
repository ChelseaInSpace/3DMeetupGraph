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
	Vector3 targetPos;
	private bool isMoving = false;

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
		//Smoothly move to target position while player inputs are disabled
		if (isMoving)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 15f);
			if(transform.position == targetPos)
				isMoving = false;
		}
		
		else if (!EventSystem.current.IsPointerOverGameObject() && MovementEnabled)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			//Set current Node when Left Mouse Button is clicked
			if (Input.GetMouseButtonDown(0))
			{
				if (Physics.Raycast(ray, out RaycastHit hit))
				{
					Node hitNode = hit.collider.gameObject.GetComponent<Node>();
					if (hitNode)
					{
						NodeHandler.UpdateCurrentNode(hitNode);
					}
					else
					{
						NodeHandler.SetNoCurrentNode();
					}
				}
				else
				{
					NodeHandler.SetNoCurrentNode();
				}
			}
			
			//Mouse look around
            if(Input.GetMouseButton(1))
            {
	            if(SettingsData.LockMouseOnCameraMovement)
		            Cursor.lockState = CursorLockMode.Locked;

                Vector3 target = NodeHandler.HasCurrentNode() ? NodeHandler.GetCurrentNode().transform.position : GetPointInFrontOfCamera();
                if (!Input.GetKey(KeyCode.LeftShift))
	            {
		            mouseX = SettingsData.InvertCamRotX ? -Input.GetAxis("Mouse X") : Input.GetAxis("Mouse X");
		            mouseY = SettingsData.InvertCamRotY ? -Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
                    transform.RotateAround(target, transform.up, mouseX * rotateSpeed * Time.deltaTime);
                    transform.RotateAround(target, transform.right, mouseY * rotateSpeed * Time.deltaTime);
	            }
	            else
	            {
		            mouse = Input.GetAxis("Mouse X");
                    transform.RotateAround(target, transform.forward, mouse * rotateSpeed * 2 * Time.deltaTime);
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
            	transform.Translate(Vector3.forward * (flySpeed * Input.GetAxis("Vertical")));
            }
    
            if (Input.GetAxis("Horizontal") != 0)
            {
            	transform.Translate(Vector3.right * (flySpeed * Input.GetAxis("Horizontal")));
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
	            transform.Translate((scroll * 0.4f * ray.direction), Space.World);
            }
		}
	}
	
	public void SetCameraTarget(Vector3 pos)
	{
		targetPos = pos - me.forward.normalized * 2f;
		if (targetPos != transform.position)
			isMoving = true;
	}

	public Vector3 GetPointInFrontOfCamera()
    {
		Vector3 target = transform.position + transform.forward.normalized * 7f;
		return target;
    }

	public static Transform GetCameraTransform()
    {
		return me.transform;
    }
}
