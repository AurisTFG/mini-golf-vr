using UnityEngine;

public class Ball : RespawnableItem
{
    public float friction = 0.99f;
    public float trailDuration = 2f;  // Duration for the trail to fade
    public float startWidth = 0.1f; // Width of the trail
    public float endWidth = 0.0f; // Width of the trail

    private TrailRenderer trailRenderer;
    private Rigidbody rb;

    private void Awake()
    {
        offsetRight = 0.0f;
        respawnRotation = new Vector3(0, 0, 0);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();

        rb.drag = friction;

        trailRenderer.time = trailDuration; // Set how long the trail lasts
        trailRenderer.startWidth = startWidth; // Set the starting width of the trail
        trailRenderer.endWidth = endWidth; // Set the ending width of the trail

        AnimationCurve curve = new();
        curve.AddKey(0.0f, startWidth);
        curve.AddKey(trailDuration / 2, endWidth);
        trailRenderer.widthCurve = curve;

        // Ensure TrailRenderer uses a transparent material
        Material transparentMaterial = new(Shader.Find("Sprites/Default"));
        trailRenderer.material = transparentMaterial;

        trailRenderer.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Club"))
        {
            trailRenderer.enabled = true;
        }
    }

    void Update()
    {
    }
}
