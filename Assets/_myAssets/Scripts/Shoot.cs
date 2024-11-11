using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject webShot;
    [SerializeField] private float shotSpeed = 15f;
    [SerializeField] private float reloadTime = 1;
    [SerializeField] private InputActionProperty shootAction;
    private bool bCanFire = true;

    void Update()
    {
        if (shootAction.action.WasPressedThisFrame() && bCanFire)
        {
            ShootWebShot();
        }
    }

    private void ShootWebShot()
    {
        bCanFire = false;
        Instantiate(webShot, transform.position, transform.rotation);
        StartCoroutine(ReloadTimer());
    }
    private IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(reloadTime);
        bCanFire = true;
    }
}
