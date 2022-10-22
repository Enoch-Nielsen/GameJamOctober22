using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float lerpSpeed;
    
    private void Update()
    {
        if (target == null)
            return;
        
        Vector2 lerpPos = Vector2.Lerp(transform.position, target.position, lerpSpeed * Time.deltaTime);
        transform.position = new Vector3(lerpPos.x, lerpPos.y, -10);
    }
}
