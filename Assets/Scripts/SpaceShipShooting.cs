using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceShipShooting : MonoBehaviour
{

    #region Private Variables
    [Header("=== Spaceship Settings ===")]
    [SerializeField] SpaceShipMovement spaceship;

    [Header("=== Hardpoint Settings ===")]
    [SerializeField] Transform[] hardpoints;
    [SerializeField] Transform hardpointMiddle;
    [SerializeField] LayerMask shootableMask;
    [SerializeField] float hardpointRange = 100f;
    bool targetInRange = false;

    [Header("=== Lazer Settings ===")]
    [SerializeField] LineRenderer[] lasers;
    [SerializeField] ImpactSurfaceFX impactFX;
    [SerializeField] float miningPower = 1f;
    [SerializeField] float timeBetweenDamage = 0.25f;
    float currentTimeBetweenDamage;
    [SerializeField] float laserHeatThreshold = 10f;
    [SerializeField] float laserHeatRate = 1f;
    [SerializeField] float laserCoolRate = 2f;
    float currentLaserHeat = 0f;
    private bool firing;
    bool overHeated = false;
    Camera cam;
	#endregion

	#region Public Variables
    public float CurrentLaserHeat
	{
		get { return currentLaserHeat; }
	}
    public float LaserHeatThreshold
	{
		get { return laserHeatThreshold; }
	}
	#endregion

	#region Unity Functions
	private void Awake()
	{
        spaceship = GetComponent<SpaceShipMovement>();
        impactFX = GetComponent<ImpactSurfaceFX>();
        cam = Camera.main;
	}
	private void Update()
    {
        if (spaceship.IsOccupied)
		{
            HandleLaserFiring();
		}
    }
    #endregion

    #region Private  Functions
    void HandleLaserFiring()
	{
        if (firing && !overHeated)
        {
            FireLasers();
            currentLaserHeat += laserHeatRate * Time.deltaTime;
        }
        else
        {
            DeactivateLasers();
        }

        currentLaserHeat = Mathf.Clamp(currentLaserHeat, 0f, laserHeatThreshold);
    }
    void ApplyDamage(HealthComponent healthComponent)
	{
        currentTimeBetweenDamage += Time.deltaTime;
        if (currentTimeBetweenDamage > timeBetweenDamage)
		{
            healthComponent.TakeDamage(miningPower);
            currentTimeBetweenDamage = 0;
		}
	}
    private void FireLasers()
    {
        RaycastHit hitInfo;

        if(TargetInfo.IsTargetInRange(hardpointMiddle.transform.position, cam.transform.forward, out hitInfo, hardpointRange, shootableMask))
		{
            if (hitInfo.collider.GetComponentInParent<HealthComponent>())
            {
                ApplyDamage(hitInfo.collider.GetComponentInParent<HealthComponent>());
                var dfx = hitInfo.collider.GetComponentInParent<DamageFX>();
                if (impactFX != null && dfx != null)
				{
                    impactFX.ApplyVFX(dfx, hitInfo);
				}
            }
            foreach (LineRenderer laser in lasers)
            {
                Vector3 localHitPosition = laser.transform.InverseTransformPoint(hitInfo.point);
                laser.gameObject.SetActive(true);
                laser.SetPosition(1, localHitPosition);
                laser.GetComponent<Laser>().targetInRange = true;
            } 
		} else
		{
            foreach (LineRenderer laser in lasers)
            {
                laser.gameObject.SetActive(true);
                laser.SetPosition(1, new Vector3(0, 0, hardpointRange));
                laser.GetComponent<Laser>().targetInRange = false;
            }
        }

        HeatLaser();

    }
    private void DeactivateLasers()
    {
        foreach (LineRenderer laser in lasers)
        {
            laser.gameObject.SetActive(false);
        }

        CoolLaser();
    }

    void HeatLaser()
	{
        if(firing && currentLaserHeat < laserHeatThreshold)
		{
            currentLaserHeat += laserHeatRate * Time.deltaTime;

            if(currentLaserHeat >= laserHeatThreshold)
			{
                overHeated = true;
                firing = false;
			}
		}
	}
    void CoolLaser()
	{
        if (overHeated)
		{
            if(currentLaserHeat / laserHeatThreshold <= 0.5f)
			{
                overHeated = false;
			}
		}

        if (currentLaserHeat > 0f)
		{
            currentLaserHeat -= laserCoolRate * Time.deltaTime;
		}
	}
    #endregion

    #region Public  Functions
    #endregion

    #region Input
    public void OnFire(InputAction.CallbackContext context)
    {
        firing = context.performed;
    }
    #endregion

}
