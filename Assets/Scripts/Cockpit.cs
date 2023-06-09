using UnityEngine;

public class Cockpit : MonoBehaviour, IInteractable
{
	public InteractionType Type => InteractionType.Hold;
	public InteractionType GetInteractionType()
	{
		return Type;
	}

	public string InteractionPrompt => $"{Type.ToString()} to interact"; //Promt will be either Press or Hold based on the type

	public bool Interact(Interactor interactor)
	{
		interactor.GetComponent<Player_OnFoot>().EnterShip();
		if (ScreenSpaceUIElement != null)
		{
			ScreenSpaceUIElement.target = null;
			ScreenSpaceUIElement.gameObject.SetActive(false);
		}
		return true; // Example return value
	}


	[Header("=== Cockpit Settings ===")]
	[SerializeField] SpaceShipMovement spaceShip;

    private Player_OnFoot player = null;
	private ScreenSpaceUIElement ScreenSpaceUIElement = null;

	private void Start()
	{
		spaceShip = GetComponentInParent<SpaceShipMovement>();
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
			player.AssignShip(spaceShip);
			if (ScreenSpaceUIElement != null)
			{
				ScreenSpaceUIElement.gameObject.SetActive(true);
				ScreenSpaceUIElement.target = this.transform;
			}
		}
		
	}

	public void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			player.UnAssignShip(spaceShip);
			player = null;
			if (ScreenSpaceUIElement != null)
			{
				ScreenSpaceUIElement.target = null;
				ScreenSpaceUIElement.gameObject.SetActive(false);
			}
		}
	}
}