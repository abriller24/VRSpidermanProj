using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float health;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"took {damage} amount of damage");
        if (health <= 0) Invoke(nameof(DestroyPlayer), 0.5f);
    }
    private void DestroyPlayer()
    {
        Destroy(gameObject);
    }
}
