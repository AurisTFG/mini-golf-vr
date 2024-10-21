using System.Collections;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GameObject nextLevelPosition;

    private GameObject _player;
    private Rigidbody _club;
    private Rigidbody _ball;
    private ParticleSystem _celebrationParticles;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
            Debug.LogError("Player not found in the scene. Make sure to tag the player GameObject with the 'Player' tag.");

        _club = GameObject.FindGameObjectWithTag("Club").GetComponent<Rigidbody>();
        if (_club == null)
            Debug.LogError("Club not found in the scene. Make sure to tag the club GameObject with the 'Club' tag.");

        _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
        if (_ball == null)
            Debug.LogError("Ball not found in the scene. Make sure to tag the ball GameObject with the 'Ball' tag.");

        _celebrationParticles = GetComponentInChildren<ParticleSystem>();
        if (_celebrationParticles == null)
            Debug.LogError("Celebration particles not found in the scene. Make sure to add a Particle System component as a child to the Trigger GameObject.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            if (_celebrationParticles != null)
                _celebrationParticles.Play();

            StartCoroutine(SpawnAfterDelay(nextLevelPosition.transform.position, 3.0f));
        }
    }

    IEnumerator SpawnAfterDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        _ball.gameObject.SetActive(false);
        _club.gameObject.SetActive(false);

        _ball.transform.position = position;
        _ball.velocity = Vector3.zero;
        _ball.angularVelocity = Vector3.zero;

        _club.transform.position = position + new Vector3(2f, 0f, 0f);
        _club.velocity = Vector3.zero;
        _club.angularVelocity = Vector3.zero;

        _player.transform.position = position + new Vector3(0f, -2f, 2f);
        _player.transform.rotation = new Quaternion(0, 90, 0, 0);

        _ball.gameObject.SetActive(true);
        _club.gameObject.SetActive(true);
    }
}
