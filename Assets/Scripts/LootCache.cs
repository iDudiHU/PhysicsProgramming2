using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCache : MonoBehaviour, IInteractable
{
	public InteractionType Type => InteractionType.Press;
	public InteractionType GetInteractionType()
	{
		return Type;
	}

	public string InteractionPrompt => $"{Type.ToString()} to interact"; //Promt will be either Press or Hold based on the type

	public bool Interact(Interactor interactor)
	{
		Debug.Log("LootCollected");
		if (ScreenSpaceUIElement != null)
		{
			ScreenSpaceUIElement.target = null;
			ScreenSpaceUIElement.gameObject.SetActive(false);
		}
		Destroy(this.gameObject);
		return true; // Example return value
	}


	[Header("=== Loot Settings ===")]

	private Player_OnFoot player = null;
	private ScreenSpaceUIElement ScreenSpaceUIElement = null;

	private void Start()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		player = null;
		ScreenSpaceUIElement = null;
		if (player == null)
		{
			player = other.GetComponentInParent<Player_OnFoot>();
		}
		if (player != null)
		{
			ScreenSpaceUIElement = player.interactUI;
			if (ScreenSpaceUIElement != null)
			{
				ScreenSpaceUIElement.gameObject.SetActive(true);
				ScreenSpaceUIElement.target = this.transform;
			}
		}

	}

	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			player = null;
			if (ScreenSpaceUIElement != null)
			{
				ScreenSpaceUIElement.target = null;
				ScreenSpaceUIElement.gameObject.SetActive(false);
			}
		}
	}
}
