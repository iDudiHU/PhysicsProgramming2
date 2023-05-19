using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private PayloadListScriptableObject payloadList;
    private int selectedPayloadIndex;
    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        ApplyRandomRotation();
        ApplyRandomForce();
        SelectPayload();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle collision logic here
    }

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

    private void SelectPayload()
    {
        if (payloadList != null && payloadList.PayloadList.Count > 0)
        {
            float totalWeight = 0f;
            foreach (var payload in payloadList.PayloadList)
            {
                totalWeight += payload.weight;
            }

            float randomWeightPoint = Random.value * totalWeight;

            float accumulatedWeight = 0f;
            for (int i = 0; i < payloadList.PayloadList.Count; i++)
            {
                var payload = payloadList.PayloadList[i];
                accumulatedWeight += payload.weight;

                if (randomWeightPoint <= accumulatedWeight)
                {
                    selectedPayloadIndex = i;
                    return;
                }
            }
        }
    }

    private void SpawnSelectedPayload()
    {
        if (selectedPayloadIndex >= 0 && selectedPayloadIndex < payloadList.PayloadList.Count)
        {
            var selectedPayload = payloadList.PayloadList[selectedPayloadIndex];
			for (int i = 0; i < Random.Range(selectedPayload.minSpawns, selectedPayload.maxSpawns); i++)
			{
                GameObject go = Instantiate(selectedPayload.asteroid, transform.position, transform.rotation, null);
                go.GetComponent<Payload>().Initialize(selectedPayload);
			}
        }
    }

    public void Kill()
	{
        SpawnSelectedPayload();
        Destroy(this.gameObject);
    }
}
