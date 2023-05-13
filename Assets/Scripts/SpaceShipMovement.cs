using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent (typeof(Rigidbody))]
public class SpaceShipMovement : MonoBehaviour
{
    [Header("=== Ship Movement Settings ===")]
    [SerializeField] float zoneMaxSpeed = 50f;
    [SerializeField] float yawTorque = 300f;
    [SerializeField] float pitchTorque = 300f;
    [SerializeField] float rollTorque = 100f;
    [SerializeField] float thrust = 2000f;
    [SerializeField] float upThrust = 50f;
    [SerializeField] float strafeThrust = 50f;
    [SerializeField, Range(0.001f, 0.999f)] float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)] float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)] float leftRightGlideReduction = 0.111f;
    [SerializeField] bool inverted = false;

    [SerializeField] private CinemachineVirtualCamera shipCam;

    [Header("=== Boost Settings ===")]
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

    //Privates
    private bool isOccupied = false;
    private Player_OnFoot player;

    public delegate void OnRequestShipExit();
    public event OnRequestShipExit onRequestShipExit;

	#region Public
    public bool IsOccupied { get { return isOccupied; } }
    public float CurrentBoostAmount
    {
        get { return currentBoostAmount; }
    }
    public float MaxBoostAmount
    {
        get { return maxBoostAmount; }
    }

    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        currentBoostAmount = maxBoostAmount;
        if (GameObject.FindGameObjectWithTag("Player"))
		{
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_OnFoot>();
            player.onRequestShipEntry += PlayerEnteredShip;
		}
    }

	void OnEnable()
	{
        if (shipCam != null)
        {
            CinemachineCameraSwitcher.Register(shipCam);
        }
        else
        {
            Debug.Log("Ship camera is not assigned!");
        }
    }

    void OnDisable()
	{
        if (shipCam != null)
        {
            CinemachineCameraSwitcher.UnRegister(shipCam);
        }
        else
        {
            Debug.Log("Ship camera is not assigned!");
        }
    }

	// Update is called once per frame
	void FixedUpdate()
    {
		if (isOccupied)
		{
			HandleBoosting();
			HandleMovement();
		}
	}
	#endregion

	#region Private Methods
	void HandleBoosting()
    {
        if (boosting && currentBoostAmount > 0f)
        {
            currentBoostAmount -= boostDepricationRate;
            if (currentBoostAmount <= 0)
            {
                boosting = false;
            }
        }
        else
        {
            if (currentBoostAmount < maxBoostAmount)
            {
                currentBoostAmount += boostRechargeRate;
            }
        }
    }
    void HandleMovement()
    {
        //Roll
        rb.AddRelativeTorque(Vector3.back * roll1D * rollTorque * Time.deltaTime);
        //Thrust
        if (thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float currentThrust = (boosting) ? thrust * boostMultiplier : thrust;

            rb.AddRelativeForce(Vector3.forward * thrust1D * currentThrust * Time.deltaTime);
            thrustGlide = thrust;
        }
        else
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
            rb.AddRelativeForce(Vector3.right * horizontalGlide * Time.deltaTime);
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
            rb.AddRelativeForce(Vector3.up * verticalGlide * Time.deltaTime);
            verticalGlide *= upDownGlideReduction;
        }
        //Pitch
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp((inverted ? -1f : 1f) * pitchYaw.y, -1f, 1f) * pitchTorque * Time.deltaTime);
        //Yaw
        rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Time.deltaTime);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, zoneMaxSpeed);
    }

    void PlayerEnteredShip(SpaceShipMovement spacehip)
	{
        rb.isKinematic = false;
        CinemachineCameraSwitcher.SwitchCamera(shipCam);
        isOccupied = true;
	}

    void playerExitedShip()
	{
        rb.isKinematic = true;
        isOccupied = false;
        if (onRequestShipExit != null) onRequestShipExit();
    }
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
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (isOccupied && context.phase == InputActionPhase.Performed)
        {
            playerExitedShip();

        }
    }
    #endregion
}
