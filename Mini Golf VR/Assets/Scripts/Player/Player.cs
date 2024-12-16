using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject SpawnPosition; // Set this to level 1 spawn position in the editor, rest are handled automatically
    public uint CurrentLevel = 1;

    public static Player Instance { get; private set; }
    public GameObject Club { get; private set; }
    public GameObject Ball { get; private set; }

    public uint[] parValues = { 1, 2, 2 };
    private Dictionary<int, string> scoringTerms = new Dictionary<int, string>()
    {
        {-3, "Albatross" },
        {-2, "Eagle" },
        {-1, "Birdie" },
        {0, "Par" },
        {1, "Bogey" },
        {2, "Double Bogey" },
        {3, "Triple Bogey" },
    };

    private Dictionary<string, uint> termsScores = new Dictionary<string, uint>()
    {
        {"Hole-in-One", 6 },
        {"Albatross", 5 },
        {"Eagle", 4 },
        {"Birdie", 3 },
        {"Par", 2 },
        {"Bogey", 1 },
        {"Double Bogey", 0 },
        {"Triple Bogey", 0 },
    };
    public List<uint> allStrokes = new();
    public List<uint> finalResults = new();
    public uint finalScore = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (SpawnPosition == null)
            Debug.LogError("Spawn position not set. Make sure to assign a GameObject to the Spawn Position field in the Player script.");

        Club = GameObject.FindGameObjectWithTag("Club");
        if (Club == null)
            Debug.LogError("Club not found in the scene. Make sure to tag the club GameObject with the 'Club' tag.");

        Ball = GameObject.FindGameObjectWithTag("Ball");
        if (Ball == null)
            Debug.LogError("Ball not found in the scene. Make sure to tag the ball GameObject with the 'Ball' tag.");

        RespawnEverything();

        Ball ballScript = Ball.GetComponent<Ball>();
        ballScript.SetParText(parValues[0]);
        ballScript.SetStrokesText();
    }

    public void RespawnEverything()
    {
        Respawn();
        RespawnBall();
        RespawnClub();
    }

    public void Respawn()
    {
        var target = SpawnPosition.transform;

        Utils.StopObject(this.gameObject);
        this.transform.SetPositionAndRotation(target.position, target.rotation);
    }

    public void RespawnBall()
    {
        Ball ballScript = Ball.GetComponent<Ball>();
        if (ballScript == null)
        {
            Debug.LogError("Ball script not found. Make sure to attach the Ball script to the ball GameObject.");
            return;
        }

        ballScript.Respawn(SpawnPosition.transform);
    }

    public void RespawnClub()
    {
        Club clubScript = Club.GetComponent<Club>();
        if (clubScript == null)
        {
            Debug.LogError("Club script not found. Make sure to attach the Club script to the club GameObject.");
            return;
        }

        clubScript.Respawn(SpawnPosition.transform);
    }

    public void RecordStrokesForLevel()
    {
        Ball ballScript = Ball.GetComponent<Ball>();

        uint strokes = ballScript.GetStrokesAndReset();
        uint levelIndex = CurrentLevel - 1 + 1;
        if (levelIndex < parValues.Length)
        {
            ballScript.SetParText(parValues[CurrentLevel - 1 + 1]);
        }

        allStrokes.Add(strokes);
    }

    public void ComputeFinalResult()
    {
        if (parValues.Length != allStrokes.Count)
        {
            Debug.LogError($"Something went wrong, par values count: {parValues.Length} not equal to all strokes count: {allStrokes.Count}");
            return;
        }

        for (uint i = 0; i < allStrokes.Count; i++)
        {
            (_, uint score) = CompuneOneResult();

            finalResults.Add(score);
            finalScore += score;
        }
    }

    public (string, uint) CompuneOneResult()
    {
        uint stroke = allStrokes[(int)CurrentLevel - 1];
        uint par = parValues[(int)CurrentLevel - 1];

        uint score = 0;
        string scoreName = string.Empty;

        if (stroke == 1)
        {
            scoreName = "Hole-in-One";
        }
        else
        {
            int diff = (int)(stroke - par);
            if (diff > 3)
                diff = 3;
            else if (diff < -3)
                diff = -3;

            scoreName = scoringTerms[diff];
        }

        score = termsScores[scoreName];

        return (scoreName, score);
    }
}
