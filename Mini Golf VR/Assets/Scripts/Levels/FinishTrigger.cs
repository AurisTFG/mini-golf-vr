using System.Collections;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public uint currentLevel;
    public GameObject nextLevelPosition;

    private ParticleSystem _celebrationParticles;
    private AudioSource _celebrationAudio;
    private float celebrationDuration = 5.0f;

    private uint totalScore = 0;

    private void Start()
    {
        _celebrationParticles = GetComponentInChildren<ParticleSystem>();
        if (_celebrationParticles == null)
            Debug.LogError("Celebration particles not found in the scene. Make sure to add a Particle System component as a child to the Trigger GameObject.");

        _celebrationAudio = GetComponentInChildren<AudioSource>();
        if (_celebrationAudio == null)
            Debug.LogError("AudioClip not found in the scene. Make sure to add an AudioClip component to the Trigger GameObject.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            StartCoroutine(TransitionToNextLevel());
            if (_celebrationParticles != null)
                _celebrationParticles.Play();
        }
    }

    private IEnumerator TransitionToNextLevel()
    {
        if (_celebrationParticles != null)
            _celebrationParticles.Play();

        if (_celebrationAudio != null)
            _celebrationAudio.Play();

        Player.Instance.RecordStrokesForLevel();
        (string scoreName, uint score) = Player.Instance.CompuneOneResult();
        totalScore++;
        LevelFinishUI.Instance.ShowLevelCompleteUI($"{scoreName}!\nScore: {score}\n\nLevel {currentLevel} Completed!");

        if (nextLevelPosition == null)
        {
            LevelFinishUI.Instance.ShowLevelCompleteUI($"{scoreName}!\nScore: {score}\n\nLevel {currentLevel} Completed!\n\nGame Finished!!!\nFinal Score:{totalScore}");
            yield break;
        }

        Player.Instance.SpawnPosition = nextLevelPosition;
        Player.Instance.CurrentLevel = currentLevel + 1;

        yield return new WaitForSeconds(celebrationDuration);

        Player.Instance.RespawnEverything();

        _celebrationParticles?.Stop();

        _celebrationAudio?.Stop();

        LevelFinishUI.Instance.HideLevelCompleteUI();
    }
}
