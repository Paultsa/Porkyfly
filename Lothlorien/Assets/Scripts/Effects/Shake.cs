using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    Vector3 originalPos;
    Coroutine shake;

    public static Shake shaker;

    private void Awake()
    {
        shaker = this;
    }
    private IEnumerator ObjectShake(float duration, float magnitude)
    {
        originalPos = transform.localPosition;
        float originalMagnitude = magnitude;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * (magnitude);
            float y = Random.Range(-1f, 1f) * (magnitude);
            magnitude = Mathf.Lerp(originalMagnitude, 0, elapsed / duration);
            //Debug.Log("TEST " + magnitude + " " + elapsed + " " + duration);

            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;

            yield return null;

        }
        //Debug.Log("STOP SHAKE " + magnitude + " " + elapsed + " " + duration);
        transform.localPosition = originalPos;
    }

    public void StartShake(float duration, float magnitude)
    {
        shake = StartCoroutine(ObjectShake(duration, magnitude));
    }

    public void StopShake()
    {
        if (shake != null)
            StopCoroutine(shake);
        transform.localPosition = originalPos;
    }

}
