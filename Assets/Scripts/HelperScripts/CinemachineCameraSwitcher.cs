using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CinemachineCameraSwitcher
{
	static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

	public static void SwitchCamera(CinemachineVirtualCamera camera)
	{
		camera.Priority = 10;
		foreach(CinemachineVirtualCamera cam in cameras)
		{
			if(cam != camera)
			{
				cam.Priority = 0;
			}
		}
	}
	public static void Register(CinemachineVirtualCamera camera)
	{
		cameras.Add(camera);
	}
	public static void UnRegister(CinemachineVirtualCamera camera)
	{
		cameras.Remove(camera);
	}
}
