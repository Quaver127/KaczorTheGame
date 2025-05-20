using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public float amplitude = 10f;       // Height of the float
    public float frequency = 1f;        // Speed of the float
    public int elementIndex = 0;        // Index in the wave
    public float waveOffset = 0.5f;     // How staggered the wave is

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float phase = elementIndex * waveOffset;
        float yOffset = Mathf.Sin(Time.time * frequency + phase) * amplitude;
        transform.localPosition = startPos + new Vector3(0, yOffset, 0);
    }
}