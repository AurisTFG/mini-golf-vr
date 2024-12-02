using UnityEngine;

public class Ball : RespawnableItem
{
    public float friction = 0.99f;
    public float trailDuration = 2f;
    public float startWidth = 0.1f;
    public float endWidth = 0.0f;

    private Rigidbody rb;
    private TrailRenderer trailRenderer;
    private GameObject teleportAnchor;

    private void Awake()
    {
        offsetRight = 0.0f;
        respawnRotation = new Vector3(0, 0, 0);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
        teleportAnchor = GameObject.Find("Teleport Anchor");

        rb.drag = friction;

        trailRenderer.time = trailDuration;
        trailRenderer.startWidth = startWidth;
        trailRenderer.endWidth = endWidth;
        AnimationCurve curve = new();
        curve.AddKey(0.0f, startWidth);
        curve.AddKey(trailDuration / 2, endWidth);
        trailRenderer.widthCurve = curve;
        trailRenderer.material = new(Shader.Find("Sprites/Default"));
        trailRenderer.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Club"))
        {
            OnStopMoving();
        }
    }

    private void OnStartMoving()
    {
        trailRenderer.enabled = true;

        teleportAnchor.SetActive(false);
    }

    private void OnStopMoving()
    {
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.identity;

        teleportAnchor.SetActive(true);
    }

    private void Update()
    {
        if (rb.velocity.magnitude <= 0.1f)
        {
            OnStopMoving();
        }
        else
        {
            OnStartMoving();
        }
    }

    private void LateUpdate() 
    {
        // just in case, make sure that anchor's rotation is not inherited from the parent (ball)
        teleportAnchor.transform.rotation = Quaternion.identity;
    }
}
