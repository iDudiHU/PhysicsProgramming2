using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public InputAction interactAction;
    private float holdStartTime;

    private void Awake()
    {
        // Setup input action events
        interactAction.started += ctx => { holdStartTime = Time.time; };
        interactAction.performed += ctx => { OnInteractionPerformed(); };
        interactAction.canceled += ctx => { OnInteractionCancelled(); };
    }

    private void OnInteractionPerformed()
    {
        float holdDuration = Time.time - holdStartTime;
        Debug.Log($"Interact button was held for {holdDuration} seconds before being released.");

        // Perform your interaction here...
    }

    private void OnInteractionCancelled()
    {
        Debug.Log("Interact action was cancelled.");

        // Handle cancellation here...
    }

    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }
}
