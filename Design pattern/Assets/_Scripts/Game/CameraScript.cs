using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float shakeTime;
    float shakeMag;
    Vector3 BasePos; 
    void Start()
    {
        BasePos = transform.position;
    }

    void Update()
    {
        if (shakeTime > 0)
        {
            transform.localPosition = (Vector2)BasePos + (Random.insideUnitCircle * shakeMag);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            shakeTime -= Time.deltaTime; 
        }
        else
        {
            transform.position = BasePos; 
        }
    }

    public void ShakeCam(float Timer, float Magnitude)
    {
        shakeTime += Timer;
        shakeMag = Magnitude;
    }

    public void ResetShake()
    {
        shakeTime = 0f; 
    }
}
