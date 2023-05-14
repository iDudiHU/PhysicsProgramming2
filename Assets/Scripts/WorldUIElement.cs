using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIElement : MonoBehaviour
{
	public Transform target;
	public Camera cam;

	private void Start()
	{
		cam = Camera.main;
	}

	public void LateUpdate()
	{
		if (target)
		{
			UpdateCanvasPositionAndRotation();
		}
	}
	private void UpdateCanvasPositionAndRotation()
    {
		var rotation = cam.transform.rotation;
		transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }
}
