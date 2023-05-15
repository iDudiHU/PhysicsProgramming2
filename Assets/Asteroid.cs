using UnityEngine;

public class Asteroid : MonoBehaviour
{
    #region Private Variables
    [SerializeField] private float rotationSpeed = 10f; // Speed of the asteroid's rotation
    [SerializeField] private float moveForce = 10f; // Force applied to the asteroid for initial movement
    private Rigidbody rigidbody; // Reference to the asteroid's Rigidbody component
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        ApplyRandomRotation();
        ApplyRandomForce();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle collision logic here
    }

    private void OnDestroy()
    {
        // Destruction logic here
    }
    #endregion

    #region Private Functions
    private void ApplyRandomRotation()
    {
        Vector3 randomRotation = Random.insideUnitSphere * rotationSpeed;
        rigidbody.angularVelocity = randomRotation;
    }

    private void ApplyRandomForce()
    {
        Vector3 randomForce = Random.insideUnitSphere * moveForce;
        rigidbody.AddForce(randomForce, ForceMode.Impulse);
    }
    #endregion

    #region Public Functions
    // Add any public functions here
    #endregion
}