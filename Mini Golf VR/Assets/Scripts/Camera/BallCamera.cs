using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCamera : MonoBehaviour
{
    public Vector3 offset = new(0, 2, -5);

    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Ball").transform;
    }

    void Update()
    {
        transform.position = target.position + offset;
        transform.LookAt(target);
    }
}
