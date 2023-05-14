using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Interactor : MonoBehaviour
{
	#region Private Variables
	Player_OnFoot player;
	[SerializeField] Transform interactionPoint;
	[SerializeField] float interactionRange = 2;
	[SerializeField] float interactionPointRadious = 1f;
	[SerializeField] LayerMask interactibleMask;
	[SerializeField] Camera cam;

	readonly Collider[] colliders = new Collider[3];
	IInteractable lastTarget;
	int numFound;
	InteractionType interactionType;
	//Cloud crash the game don't do this;
	float timeSinceLastInteract;
	bool correctInputRecieved = false;

	#endregion

	#region Public Variables
	public ScreenSpaceUIElement interactUI;
	#endregion

	#region Unity Functions
	// Start is called before the first frame update
	void Start()
	{
		player = GetComponent<Player_OnFoot>();
		cam = Camera.main;
	}

	// Update is called once per frame
	void Update()
	{
		int numFound = Physics.OverlapBoxNonAlloc(interactionPoint.position, interactionPoint.localScale / 2, colliders, Quaternion.identity, interactibleMask);
		RaycastHit hitInfo;
		float closestDistance = float.MaxValue;
		IInteractable closestInteractable = null;

		if (numFound > 0)
		{
			foreach (Collider collider in colliders)
			{
				if (collider == null)
				{
					continue;
				}
				IInteractable interactable = collider.GetComponentInChildren<IInteractable>();
				if (interactable == null)
				{
					continue;
				}
				if (!TargetInfo.IsTargetInRange(cam.transform.position, cam.transform.forward, out hitInfo, interactionRange, interactibleMask))
				{
					continue;
				}
				float distance = Vector3.Distance(cam.transform.position, hitInfo.point);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestInteractable = interactable;
				}
			}
			if (closestInteractable != null)
			{
				interactionType = closestInteractable.Type;
				lastTarget = closestInteractable;
			}
		}
		else
		{
			if (closestInteractable == null)
			{
				interactionType = 0;
				lastTarget = null;
			}
		}


	}

	#endregion

	#region Private  Functions
	#endregion

	#region Public  Functions
	#endregion
	#region Input Functions
	public void OnInteract(InputAction.CallbackContext context)
	{
		if (lastTarget != null)
		{
			switch (context.phase)
			{
				case InputActionPhase.Started:
					if (interactionType == InteractionType.Press)
					{
						lastTarget.Interact(this);
					}
					else
					{
						interactUI.StartSliderAnimation();
					}
					break;

				case InputActionPhase.Performed:
					if (interactionType == InteractionType.Hold)
					{
						lastTarget.Interact(this);
						interactUI.StopSliderAnimation();
					}
					break;

				case InputActionPhase.Canceled:
					interactUI.StopSliderAnimation();
					break;
			}
		}
	}

	#endregion

	#region Debug  Functions
#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		if (cam != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(cam.transform.position, cam.transform.forward.normalized * interactionRange);
		}
		else
		{
			Debug.Log("Cam is not assigned for debug");
		}
	}
#endif
	#endregion
}
