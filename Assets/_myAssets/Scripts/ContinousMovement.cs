using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinousMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float inAirMoveSpeed;
    [SerializeField] private float groundSpeed;
    [SerializeField] private float turnSpeed = 60f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private bool bOnlyMoveWhenGrounded = false;
    [SerializeField] private bool bJumpingWithHands;
    [SerializeField] private float minJumpWithHandSpeed = 2f;
    [SerializeField] private float maxJumpWithHandSpeed = 7f;
    [SerializeField] private InputActionProperty moveInputSource;
    [SerializeField] private InputActionProperty turnInputSource;
    [SerializeField] private InputActionProperty jumpInputSource;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody leftHandRB;
    [SerializeField] private Rigidbody rightHandRB;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform directionSource;
    [SerializeField] private Transform turnSource;
    [SerializeField] private CapsuleCollider bodyCollider;

    private Vector2 inputMoveAxis;
    private float jumpVelocity = 7f;
    private float inputTurnAxis;
    private bool bIsGrounded;

    private void Update()
    {
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;

        bool jumpInput = jumpInputSource.action.WasPressedThisFrame();

        if (!bJumpingWithHands)
        {
            if(jumpInput && bIsGrounded)
            {
                jumpVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * jumpHeight);
                rb.velocity += Vector3.up * jumpVelocity;
            }
        }
        else
        {
            bool bInputJumpPressed = jumpInputSource.action.IsPressed();

            float handSpeed = ((leftHandRB.velocity - rb.velocity).magnitude + (rightHandRB.velocity - rb.velocity).magnitude);
            if(bInputJumpPressed && bIsGrounded && handSpeed > minJumpWithHandSpeed)
            {
                rb.velocity = Vector3.up * Mathf.Clamp(handSpeed, minJumpWithHandSpeed, maxJumpWithHandSpeed);
            }
        }
    }

    private void FixedUpdate()
    {
        bIsGrounded = CheckIfGrounded();

        if (!bOnlyMoveWhenGrounded || (bOnlyMoveWhenGrounded && bIsGrounded))
        {
            Quaternion yaw = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
            Vector3 direction = yaw * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

            Vector3 targetMovePOS = rb.position + direction * Time.fixedDeltaTime * speed;

            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis; 

            Quaternion q = Quaternion.AngleAxis(angle, axis);

            rb.MoveRotation(rb.rotation * q);

            Vector3 newPosition = q * (targetMovePOS - turnSource.position) + turnSource.position;
            rb.MovePosition(newPosition);
        }
    }
    
    public bool CheckIfGrounded()
    {
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height / 2 - bodyCollider.radius + 0.05f;

        bool hasHit = Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return hasHit;
    }
}
