using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRig : MonoBehaviour
{
    [SerializeField] private Transform playerHead;
    [SerializeField] private Transform rightHandController;
    [SerializeField] private Transform leftHandController;

    [SerializeField] private ConfigurableJoint headJoint;
    [SerializeField] private ConfigurableJoint rightHandJoint;
    [SerializeField] private ConfigurableJoint leftHandJoint;

    [SerializeField] private CapsuleCollider bodyCollider;

    [SerializeField] private float bodyHeightMin = 0.5f;
    [SerializeField] private float bodyHeightMax = 2f;

    void FixedUpdate()
    {
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height/2, playerHead.localPosition.z);  
        
        leftHandJoint.targetPosition = leftHandController.localPosition;
        leftHandJoint.targetRotation = leftHandController.localRotation;
        
        rightHandJoint.targetPosition = rightHandController.localPosition;
        rightHandJoint.targetRotation = rightHandController.localRotation;

        headJoint.targetPosition = playerHead.localPosition;
    }
}
