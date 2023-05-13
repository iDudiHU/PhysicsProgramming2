using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Private Variables
    [SerializeField] Image boostImage;
    [SerializeField] Player_OnFoot player;
    [SerializeField] SpaceShipMovement currentSpaceship;
    [SerializeField] SpaceShipShooting currentSpaceshipShooting;
    #endregion

    #region Public Variables
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        if(player != null)
		{
            FindObjectOfType<Player_OnFoot>();
		}
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_OnFoot>();
        player.onRequestShipEntry += PlayerEnteredShip;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpaceship != null)
		{
            boostImage.fillAmount = currentSpaceship.CurrentBoostAmount / currentSpaceship.MaxBoostAmount;
		}
    }
    #endregion

    #region Private  Functions
    void PlayerEnteredShip(SpaceShipMovement spaceship)
	{
        currentSpaceship = spaceship;
        currentSpaceshipShooting = spaceship.GetComponent<SpaceShipShooting>();
	}
    #endregion

    #region Public  Functions
    #endregion

}
