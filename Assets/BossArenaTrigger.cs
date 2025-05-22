using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class BossArenaTrigger : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Camera mainCamera;
    public float transitionDuration = 1f; 
    public float defaultCameraSize = 10f;  

    private Coroutine sizeCoroutine;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Bounds bounds = GetComponent<Collider2D>().bounds;

            Vector3 center = bounds.center;
            float width = bounds.size.x;
            float height = bounds.size.y;

            cameraFollow.EnterBossArena(new Vector3(center.x, center.y, -10f));

            float targetSize = height / 2f;

            float requiredWidth = width / (2f * (Screen.width / (float)Screen.height));
            if (requiredWidth > targetSize)
                targetSize = requiredWidth;

            if (sizeCoroutine != null)
                StopCoroutine(sizeCoroutine);

            sizeCoroutine = StartCoroutine(SmoothResize(targetSize));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cameraFollow.ExitBossArena();

            if (sizeCoroutine != null)
                StopCoroutine(sizeCoroutine);

            sizeCoroutine = StartCoroutine(SmoothResize(defaultCameraSize));
        }
    }

    IEnumerator SmoothResize(float targetSize)
    {
        float startSize = mainCamera.orthographicSize;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / transitionDuration);
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
    }
}