using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject grapeProjectilePrefabs;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Attack()
    {
        animator.SetTrigger(ATTACK_HASH);
        if (transform.position.x - PlayerController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;

        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    public void SpawnProjectileAnimEvent()
    {
        Instantiate(grapeProjectilePrefabs, transform.position, Quaternion.identity);
    } 
}
