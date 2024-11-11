using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private int damage;
    void Start()
    {
        damage = Random.Range(15, 30);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerScript player = other.gameObject.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
            return;
        else
            Destroy(gameObject);
    }
}
