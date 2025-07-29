using UnityEngine;
using UnityEngine.UI;

public class BackgroundMotion : MonoBehaviour
{
    public RawImage backgroundImage;
    public float amplitudeX = 0.01f; 
    public float amplitudeY = 0.005f;
    public float frequency = 0.5f;   

    private Rect originalUV;

    void Start()
    {
        originalUV = backgroundImage.uvRect;
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * frequency) * amplitudeX;
        float offsetY = Mathf.Cos(Time.time * frequency) * amplitudeY;

        backgroundImage.uvRect = new Rect(
            originalUV.x + offsetX,
            originalUV.y + offsetY,
            originalUV.width,
            originalUV.height
        );
    }
}
