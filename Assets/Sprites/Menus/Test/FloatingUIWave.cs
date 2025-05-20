using UnityEngine;

public class FloatingUIWave : MonoBehaviour
{
    public float amplitude = 10f;
    public float frequency = 1f;
    public float waveOffset = 0.5f;

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var floater = child.gameObject.AddComponent<FloatingUI>();
            floater.amplitude = amplitude;
            floater.frequency = frequency;
            floater.elementIndex = i;
            floater.waveOffset = waveOffset;
        }
    }
}