using System.Collections;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GameObject position;          // The position where you want to spawn the objects
    public Rigidbody ball;            // Reference to the ball's Rigidbody
    public Rigidbody club;            // Reference to the club's Rigidbody
    public GameObject player;         // Reference to the player GameObject
    private bool isTriggered = false; // To prevent multiple triggers
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && !isTriggered) // Check for Ball tag and prevent multiple triggers
        {
            
            StartCoroutine(SpawnAfterDelay(ball, club, player, position.transform.position, 1.0f));

        }
    }

    IEnumerator SpawnAfterDelay(Rigidbody ball, Rigidbody club, GameObject player, Vector3 position, float delay)
    {
        // Deactivate objects
        ball.gameObject.SetActive(false);
        club.gameObject.SetActive(false);
       

        yield return new WaitForSeconds(delay);

        // Move them to the new position and reset velocities
        ball.transform.position = position;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;

        club.transform.position = position + new Vector3(2f, 0f,0f);
        club.velocity = Vector3.zero;
        club.angularVelocity = Vector3.zero;

        player.transform.position = position + new Vector3(0f, -2f, 2f);
        player.transform.rotation = new Quaternion(0, 90, 0, 0);

        // Reactivate the objects after repositioning
        ball.gameObject.SetActive(true);
        club.gameObject.SetActive(true);
        

       
    }
}
