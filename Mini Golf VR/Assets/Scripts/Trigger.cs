using System.Collections;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Rigidbody ball;
    public Rigidbody club;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Ball"))
        {
            System.Console.WriteLine("LYGIS PEREITAS");
            StartCoroutine(RespawnAfterDelay(other, new Vector3(1.54f, 0.148f, 1.832f), 1.0f));
        }

        if (other.CompareTag("Club"))
        {
            StartCoroutine(RespawnAfterDelay(other, new Vector3(2.05f, 0.6f, 1.66f), 1.0f));
        }

        if (other.CompareTag("Player"))
        {
            StartCoroutine(RespawnAfterDelay(other, Vector3.zero, 1.0f));
        }
    }

    IEnumerator RespawnAfterDelay(Collider collider, Vector3 position, float delay)
    {
        collider.gameObject.SetActive(false);

        yield return new WaitForSeconds(delay);

        collider.transform.position = position;

        Rigidbody rb = collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        collider.gameObject.SetActive(true);
    }
}
