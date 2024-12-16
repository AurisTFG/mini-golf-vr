using System.Collections;
using UnityEngine;

enum BallState
{
    Moving,
    Stopped
}

public class Ball : RespawnableItem
{
    public float friction = 0.99f;
    public float trailDuration = 2f;
    public float startWidth = 0.1f;
    public float endWidth = 0.0f;

    private Rigidbody ballRigidbody;
    private TrailRenderer trailRenderer;

    private BallState state = BallState.Stopped;

    private int clubLayerId = -1;
    private int ballLayerId = -1;

    private float lastVelocity;
    private float currentVelocity;
    private float changeInVelocity;

    private void Awake()
    {
        offsetRight = 0.0f;
        respawnRotation = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();

        ballRigidbody.drag = friction;
        trailRenderer.time = trailDuration;
        trailRenderer.startWidth = startWidth;
        trailRenderer.endWidth = endWidth;
        AnimationCurve curve = new();
        curve.AddKey(0.0f, startWidth);
        curve.AddKey(trailDuration / 2, endWidth);
        trailRenderer.widthCurve = curve;
        trailRenderer.material = new(Shader.Find("Sprites/Default"));
        trailRenderer.enabled = false;

        clubLayerId = LayerMask.NameToLayer("Club");
        ballLayerId = LayerMask.NameToLayer("Ball");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Club"))
        {
            if (state == BallState.Stopped)
            {
                OnCollisionWithClub();
            }
        }
    }

    private void FixedUpdate()
    {
        currentVelocity = ballRigidbody.velocity.magnitude;
        changeInVelocity = currentVelocity - lastVelocity;

        if (currentVelocity < 0.1f &&
            changeInVelocity <= 0.0f && changeInVelocity >= -1.0f &&
            state == BallState.Moving
            )
        {
            StopMovingBall();
        }

        lastVelocity = ballRigidbody.velocity.magnitude;
    }

    private void OnCollisionWithClub()
    {
        StartMovingBall();
    }

    private void StartMovingBall()
    {
        trailRenderer.enabled = true;

        ModifyCollision(false);

        state = BallState.Moving;
    }

    private void StopMovingBall()
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.rotation = Quaternion.identity;

        ModifyCollision(true);

        state = BallState.Stopped;
    }

    private void ModifyCollision(bool enable)
    {
        Physics.IgnoreLayerCollision(ballLayerId, clubLayerId, !enable);
    }
}
