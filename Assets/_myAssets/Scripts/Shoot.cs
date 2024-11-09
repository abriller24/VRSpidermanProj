using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject webShot;
    [SerializeField] private float shotSpeed = 15f;
    [SerializeField] private InputActionProperty shootAction;

    void Update()
    {
        if (shootAction.action.WasPressedThisFrame())
        {
            ShootWebShot();
        }
    }

    private void ShootWebShot()
    {
        Instantiate(webShot, transform.position, transform.rotation);
    }
}
