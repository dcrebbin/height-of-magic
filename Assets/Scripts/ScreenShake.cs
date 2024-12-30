using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform camTransform;
    private float timer;
    private float shakeAmount { get; set; }
    private float shakeDuration { get; set; }

    public void TriggerShake(float inputShakeAmount, float inputShakeDuration)
    {
        shakeAmount = inputShakeAmount;
        shakeDuration = inputShakeDuration;
        StopCoroutine(nameof(shakeCoroutine));
        StartCoroutine(nameof(shakeCoroutine));
    }

    IEnumerator shakeCoroutine()
    {
        timer = 0;
        while (enabled)
        {
            timer += Time.deltaTime;
            camTransform.localPosition += Random.insideUnitSphere * shakeAmount;

            if (timer >= shakeDuration)
            {
                StopCoroutine(nameof(shakeCoroutine));
            }

            yield return null;
        }
    }
}
