using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    #region Private Variables
    [SerializeField] private GameObject[] asteroidObjects;
    [SerializeField] private int amountOfAsteroidsToSpawn = 100;
    [SerializeField] private float minimumRandomSpawnPosition = -100f;
    [SerializeField] private float maximumRandomSpawnPosition = 100f;
    #endregion

    #region Unity Functions
    // Start is called before the first frame update
    void Start()
    {
        SpawnAsteroids();
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic here
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(
            (maximumRandomSpawnPosition - minimumRandomSpawnPosition),
            (maximumRandomSpawnPosition - minimumRandomSpawnPosition),
            (maximumRandomSpawnPosition - minimumRandomSpawnPosition)
        ));
    }
    #endregion

    #region Private Functions
    private void SpawnAsteroids()
    {
        for (int i = 0; i < amountOfAsteroidsToSpawn; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(minimumRandomSpawnPosition, maximumRandomSpawnPosition),
                Random.Range(minimumRandomSpawnPosition, maximumRandomSpawnPosition),
                Random.Range(minimumRandomSpawnPosition, maximumRandomSpawnPosition)
            );

            int randomIndex = Random.Range(0, asteroidObjects.Length);
            GameObject asteroid = Instantiate(asteroidObjects[randomIndex], randomPosition, Quaternion.identity);
            asteroid.transform.SetParent(transform);
        }
    }
    #endregion

    #region Public Functions
    // Any public functions go here
    #endregion
}
