using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicLaser : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2;
    private bool isGrowing = true;
    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }
    private void Start()
    {
        LaserFaceMouse();
    }
    public void UpdateLaserRange(float laserRange)
    {
        this.laserRange = laserRange;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }
    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0;
        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;
            spriteRenderer.size = new Vector2(Mathf.Lerp(1, laserRange, linearT), 1);

            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1, laserRange, linearT), capsuleCollider2D.size.y);
            capsuleCollider2D.offset = new Vector2((Mathf.Lerp(1, laserRange, linearT)) / 2, capsuleCollider2D.offset.y);
            yield return null;

        }
        StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Indestructible>() && !col.isTrigger)
        {
            isGrowing = false;
        }
    }
    private void LaserFaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = transform.position - mousePos;
        transform.right = -dir;
    }
}
