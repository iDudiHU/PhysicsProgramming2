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
		transform.position = Vector3.Lerp(target.position, cam.transform.position, 0.25f);
		transform.LookAt(target);
    }
}
