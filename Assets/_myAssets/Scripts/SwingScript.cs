using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwingScript : MonoBehaviour
{
    [Header("Swing Variables")]
    [SerializeField] private Transform startSwingHand;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask swingableLayer;

    [Header("Prediction Point")]
    [SerializeField] private Transform predictionPoint;

    [Header("Input Actions")]
    [SerializeField] private InputActionProperty swingAction;

    private Vector3 swingPoint;
    private SpringJoint joint;
    private bool bHasHit;
    private void Update()
    {
        GetSwingPoint();

        if (swingAction.action.WasPressedThisFrame())
        {
            StartSwing();
        }
        else if(swingAction.action.WasReleasedThisFrame())
        {
            ReleaseSwing();
        }
    }

    public void StartSwing()
    {
        if (bHasHit)
        {
            joint = rb.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distance = Vector3.Distance(rb.position, swingPoint);
            joint.maxDistance = distance;

            joint.spring = 4.5f;
            joint.damper = 7;
            joint.massScale = 4.5f;
        }
    }
    public void ReleaseSwing()
    {
        Destroy(joint);
    }

    private void GetSwingPoint()
    {
        if (joint)
            return;

        RaycastHit hit;

        bHasHit = Physics.Raycast(startSwingHand.position, startSwingHand.forward, out hit, maxDistance, swingableLayer);

        if (bHasHit)
        {
            swingPoint = hit.point;
            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = swingPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }
    }
}
