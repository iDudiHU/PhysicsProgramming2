using UnityEngine;

public class Cockpit : MonoBehaviour, IInteractable
{
    [SerializeField] SpaceShip spaceShip;

    private Player_OnFoot player = null;
	private ScreenSpaceUIElement ScreenSpaceUIElement = null;

	private void Start()
	{
		spaceShip = GetComponentInParent<SpaceShip>();
	}
	public void Interact(Player_OnFoot player)
    {

    }

	public void OnTriggerEnter(Collider other)
	{
		if (player == null)
		{
			player = other.GetComponentInParent<Player_OnFoot>();
		}
		if (player != null)
		{
			ScreenSpaceUIElement = player.interactUI;
			player.AssignShip(spaceShip);
		}
		if (ScreenSpaceUIElement == null)
		{
			ScreenSpaceUIElement = player.interactUI;
		}

		if (ScreenSpaceUIElement != null)
		{
			ScreenSpaceUIElement.gameObject.SetActive(true);
			ScreenSpaceUIElement.target = this.transform;
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