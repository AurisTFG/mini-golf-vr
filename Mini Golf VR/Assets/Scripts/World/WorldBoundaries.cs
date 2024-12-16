using UnityEngine;

public class WorldBoundaries : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Player.Instance.RespawnBall();
        }

        if (other.CompareTag("Club"))
        {
            Player.Instance.RespawnClub();
        }

        if (other.CompareTag("Player"))
        {
            Player.Instance.RespawnEverything();
        }
    }
}
