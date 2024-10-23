using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] positions;
    [SerializeField] private Rigidbody rb;

    private Vector3 targetPOS;

    private void Start()
    {
        targetPOS = positions[0].position;
    }

    private void FixedUpdate()
    {
        
    }
}
