using System;
using UnityEngine;

public class WebShooters : MonoBehaviour
{
    [SerializeField] private float forceFactor;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * forceFactor);
    }
}
