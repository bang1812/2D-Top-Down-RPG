using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyVFX;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<DamageSource>() || col.gameObject.GetComponent<Projectile>())
        {
            GetComponent<PickUpSpawner>().DropItems();
            Instantiate(destroyVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
