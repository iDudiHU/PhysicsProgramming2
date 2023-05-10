using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Rigidbody))]
public class BasicFlight : MonoBehaviour
{
    [Header("=== Ship Movement Settings ===")]
    [SerializeField] float yawTorque = 100f;
    [SerializeField] float pitchTorque = 100f;
    [SerializeField] float rollTorque = 50f;
    [SerializeField] float thrust = 100f;
    [SerializeField] float upThrust = 50f;
    [SerializeField] float strafeThrust = 50f;
    [SerializeField, Range(0.001f, 0.999f)] float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)] float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)] float leftRightGlideReduction = 0.111f;
    [SerializeField] bool inverted = false;

    [Header("=== Ship Movement Settings ===")]
    [SerializeField] float maxBoostAmount = 2f;
    [SerializeField] float boostDepricationRate = 0.25f;
    [SerializeField] float boostRechargeRate = 0.5f;
    [SerializeField] float boostMultiplier = 5f;
    private float currentBoostAmount = 0f;
    private bool boosting = false;

    //Not Exposed Privates
    private float thrustGlide, horizontalGlide, verticalGlide = 0f;


    Rigidbody rb;

    //Input Values
    private float thrust1D;
    private float upDown1D;
    private float strafe1D;
    private float roll1D;
    private Vector2 pitchYaw;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentBoostAmount = maxBoostAmount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleBoosting();
        HandleMovement();
    }

    void HandleBoosting()
	{
        if (boosting && currentBoostAmount > 0f)
		{
            currentBoostAmount -= boostDepricationRate;
            if(currentBoostAmount <= 0)
			{
                boosting = false;
			}
		} else
		{
            if(currentBoostAmount < maxBoostAmount)
			{
                currentBoostAmount += boostRechargeRate;
			}
		}
	}
    void HandleMovement()
	{
        //Roll
        rb.AddRelativeTorque(Vector3.forward * roll1D * rollTorque * Time.deltaTime);
        //Thrust
        if (thrust1D > 0.1f || thrust1D < -0.1f)
		{
            float currentThrust = (boosting) ? thrust * boostMultiplier : thrust;

            rb.AddRelativeForce(Vector3.forward * thrust1D * currentThrust * Time.deltaTime);
            thrustGlide = thrust;
		} else
		{
            rb.AddRelativeForce(Vector3.forward * thrustGlide * Time.deltaTime);
            thrustGlide *= thrustGlideReduction;
		}
        //Strafe
        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.right * strafe1D * strafeThrust * Time.deltaTime);
            horizontalGlide = strafe1D * strafeThrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.right * strafe1D * Time.deltaTime);
            horizontalGlide *= leftRightGlideReduction;
        }
        //UpDown
        if (upDown1D > 0.1f || upDown1D < -0.1f)
        {
            rb.AddRelativeForce(Vector3.up * upDown1D * upThrust * Time.deltaTime);
            verticalGlide = upDown1D * upThrust;
        }
        else
        {
            rb.AddRelativeForce(Vector3.up * upThrust * Time.deltaTime);
            verticalGlide *= upDownGlideReduction;
        }
        //Pitch
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp((inverted ? -1f : 1f) * pitchYaw.y, -1f, 1f * pitchTorque * Time.deltaTime));
        //Yaw
        rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f * yawTorque * Time.deltaTime));
    }


	#region Private Methods
	#endregion

	#region Public Methods

    public void ToggleInverted()
	{
        inverted = !inverted;
	}

	#endregion

	#region Input Methods
	public void OnThrust(InputAction.CallbackContext context)
	{
        thrust1D = context.ReadValue<float>();
	}
    public void OnStrafe(InputAction.CallbackContext context)
    {
        strafe1D = context.ReadValue<float>();
    }
    public void OnUpDown(InputAction.CallbackContext context)
    {
        upDown1D = context.ReadValue<float>();
    }
    public void OnRoll(InputAction.CallbackContext context)
    {
        roll1D = context.ReadValue<float>();
    }
    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
    }
    public void OnBoost(InputAction.CallbackContext context)
	{
        boosting = context.performed;
	}
    #endregion
}
