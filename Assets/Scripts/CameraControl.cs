using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    public static bool MovementEnabled = true;
	private static Transform me;

	//Movement controls
	float flySpeed = 0.05f;
	float accelerationRatio = 0.75f;
	float slowDownRatio = 0.2f;

	//Mouse lookaround controls
	public float mouseSensitivity = 200;
	public float clampAngle = 60.0f;
	private float rotY = 0.0f; //rotation around the up/y axis
	private float rotX = 0.0f; //rotation around the right/x axis

    void Awake()
    {
		me = gameObject.transform;
	}

    void Start()
	{
		Vector3 rot = transform.localRotation.eulerAngles;
		rotY = rot.y;
		rotX = rot.x;
	}

	void Update()
	{
		if (!EventSystem.current.IsPointerOverGameObject() && MovementEnabled)
		{
			//Mouse lookaround
            if(Input.GetKey(KeyCode.Mouse1))
            {
            	Cursor.lockState = CursorLockMode.Locked;
            	float mouseX = Input.GetAxis("Mouse X");
            	float mouseY = -Input.GetAxis("Mouse Y");
            	rotY += mouseX * mouseSensitivity * Time.deltaTime;
            	rotX += mouseY * mouseSensitivity * Time.deltaTime;
            	rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
            	Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            	transform.rotation = localRotation;
            }
    
            if(Input.GetKeyUp(KeyCode.Mouse1))
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
    
            transform.Translate(Input.mouseScrollDelta.y * 0.2f * Vector3.forward);
    
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