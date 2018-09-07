using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoShake : MonoBehaviour {
    public Transform camTransform;

    public float shake = 0f;
    
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 2.0f;

    Vector3 originalPos;

    public bool ShakeUse = false;
    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shake > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shake = 0f;
            camTransform.localPosition = originalPos;
            shake = 10f;
        }
    }
    public void Shake()
    {
        shake = 10f;
        if (shake > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shake = 0f;
            camTransform.localPosition = originalPos;
            ShakeUse = false;
        }
    }
}
