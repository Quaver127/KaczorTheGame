using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform Target;

    public bool isInBossArena = false;
    public Vector3 bossArenaCameraPosition;

    private Transform camTransform;
    
    public float shakeDuration = 0f;
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        Cursor.visible = false;
        if (camTransform == null)
        {
            camTransform = GetComponent<Transform>();
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition;

        if (isInBossArena)
        {
            targetPosition = bossArenaCameraPosition;
        }
        else
        {
            if (Target == null) return;

            targetPosition = new Vector3(Target.position.x, Target.position.y, -10f);
            
        }

        
        transform.position = Vector3.Slerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);

        
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
    }

    public void ShakeCamera()
    {
        originalPos = camTransform.localPosition;
        shakeDuration = 0.2f;
    }

    public void EnterBossArena(Vector3 staticPosition)
    {
        isInBossArena = true;
        bossArenaCameraPosition = staticPosition;
    }

    public void ExitBossArena()
    {
        isInBossArena = false;
    }
}