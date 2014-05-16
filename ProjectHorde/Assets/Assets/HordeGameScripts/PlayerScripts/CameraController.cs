using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	//public
	public Camera camera;

	public float fYSens;
	public float fXSens;
	public float fMinimumY;
	public float fMaximumY;

	//private variables
	bool bLockCamera = true;
	Vector3 v3Temp = Vector3.zero;
	float fRotateX;
	float fRotationY = 0F;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0, Input.GetAxis("Mouse X") * fXSens, 0);

		fRotationY += Input.GetAxis("Mouse Y") * fYSens;
		fRotationY = Mathf.Clamp (fRotationY, fMinimumY, fMaximumY);
		
		camera.transform.localEulerAngles = new Vector3(-fRotationY, transform.localEulerAngles.y, 0);
	}
}
