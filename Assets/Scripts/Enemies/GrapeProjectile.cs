using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3;
    [SerializeField] private GameObject grapeProjectileShadow;
    [SerializeField] private GameObject splatterPrefab;
    private void Start()
    {
        GameObject grapeShadow = Instantiate(grapeProjectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 grapeShadowStartPosition = grapeShadow.transform.position;
        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
        StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, playerPos));

    }
    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0, heightY, heightT);
            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0, height);
            yield return null;
        }
        Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0;
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
            yield return null;
        }
        Destroy(grapeShadow);
    }
}
