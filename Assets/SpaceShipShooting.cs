using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipShooting : MonoBehaviour
{
    #region Private Variables
    [Header("=== Hardpoint Settings ===")]
    [SerializeField] Transform[] hardpoints;
    [SerializeField] LayerMask shootableMask;
    [SerializeField] float hardpointRange = 100f;
    bool targetInRange = false;

    [Header("=== Lazer Settings ===")]
    [SerializeField] LineRenderer[] lasers;
    [SerializeField] ParticleSystem laserHitParticles;
    [SerializeField] float miningPower = 1f;
    [SerializeField] float laserHeatThreshold = 2f;
    [SerializeField] float laserHeatRate = 0.25f;
    [SerializeField] float laserCoolRate = 0.5f;
    float currentLaserHeat = 0f;
    bool overHeated = false;
    #endregion

    #region Public Variables
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Private  Functions
    #endregion

    #region Public  Functions
    #endregion

}
