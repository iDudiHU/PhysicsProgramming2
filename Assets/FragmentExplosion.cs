using UnityEngine;
using UnityEngine.Events;

public class FragmentExplosion: MonoBehaviour
{
    [Tooltip("Power of the force applied when the fragment unfreezes.")]
    public float forcePower = 10.0f;


    public void Unfreeze()
    {
        transform.SetParent(null);
        foreach (Rigidbody childRb in transform.GetComponentsInChildren<Rigidbody>())
        {
            UnfreezeChild(childRb);
        }
    }

    private void UnfreezeChild(Rigidbody rb)
    {
        rb.isKinematic = false;
        // Calculate the vector based on the current center of the object and the origin of the original object
        Vector3 forceDirection = (rb.worldCenterOfMass - transform.position).normalized;

        // Apply a random force in the calculated direction
        rb.AddForce(forceDirection * Random.Range(0.5f, 1.5f) * forcePower, ForceMode.Impulse);
    }
}
