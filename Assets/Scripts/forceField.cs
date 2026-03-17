using UnityEngine;

public class ForceField : MonoBehaviour
{
    public float pushForce = 50f;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Vector3 pushDir = collision.contacts[0].normal;
                Vector3 pushDir = (collision.transform.position - transform.position).normalized;
                rb.AddForce(pushDir * pushForce, ForceMode.Impulse);
                //rb.AddForce(pushDir * pushForce, ForceMode.Force);
            }
        }
    }
}