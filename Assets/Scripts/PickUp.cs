using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private enum PickUpType
    {
        GoldCoin,
        StaminaGlobe,
        HealthGlobe,
    }

    [SerializeField] private PickUpType pickUpType;
    [SerializeField] private float pickUpDistance = 5;
    [SerializeField] private float accelarttionRate = .2f;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 1.5f;
    [SerializeField] private float popDuration = 1f;
    private Vector3 moveDir;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartCoroutine(AnimCurveSpawnRoutine());
    }
    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        if(Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelarttionRate;
        }
        else
        {
            moveDir = Vector3.zero;
            moveSpeed = 0;
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = moveDir * moveSpeed * Time.deltaTime;
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerController>())
        {
            DetectPickUpType();
            Destroy(gameObject);
        }
    }
    private IEnumerator AnimCurveSpawnRoutine()
    {
        Vector2 startPoint = transform.position;
        float randomX = transform.position.x + Random.Range(-2, 2);
        float randomY = transform.position.y + Random.Range(-1, 1);
        Vector2 endPoint=new Vector2(randomX, randomY);
        float timePassed = 0;
        while(timePassed < popDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / popDuration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0, heightY, heightT);
            transform.position = Vector2.Lerp(startPoint, endPoint, linearT) + new Vector2(0, height);
            yield return null;
        }
    }
    private void DetectPickUpType()
    {
        switch (pickUpType)
        {
            case PickUpType.GoldCoin:
                EconomyManager.Instance.UpdateCurrentGold();
                
                break;
            case PickUpType.StaminaGlobe:
                Stamina.Instance.RefreshStamina();
                
                break;
            case PickUpType.HealthGlobe:
                PlayerHealth.Instance.HealPlayer();
                
                break;
            
        }
    }
}
