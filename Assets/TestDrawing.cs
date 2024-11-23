using UnityEngine;

public class TestDrawing : MonoBehaviour
{
    public RenderTexture paintingCanvasRenderTexture;

    void Start()
    {
        // Wait a frame to ensure InitializeCanvas has completed
        Invoke("DrawCircle", 0.1f);
    }

    void DrawCircle()
    {
        if (paintingCanvasRenderTexture == null)
        {
            Debug.LogError("paintingCanvasRenderTexture not assigned in TestDrawing!");
            return;
        }

        Debug.Log($"Drawing circle on texture of size {paintingCanvasRenderTexture.width}x{paintingCanvasRenderTexture.height}");

        // Create a temporary texture to read the current content
        RenderTexture.active = paintingCanvasRenderTexture;
        Texture2D currentContent = new Texture2D(paintingCanvasRenderTexture.width, paintingCanvasRenderTexture.height);
        currentContent.ReadPixels(new Rect(0, 0, paintingCanvasRenderTexture.width, paintingCanvasRenderTexture.height), 0, 0);
        currentContent.Apply();

        // Draw the circle
        int radius = 100; // Made larger for better visibility
        int centerX = currentContent.width / 2;
        int centerY = currentContent.height / 2;

        Color circleColor = Color.red;
        circleColor.a = 1f; // Ensure full opacity

        for (int y = centerY - radius; y <= centerY + radius; y++)
        {
            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                if (x >= 0 && x < currentContent.width && y >= 0 && y < currentContent.height)
                {
                    float distance = Mathf.Sqrt(Mathf.Pow(x - centerX, 2) + Mathf.Pow(y - centerY, 2));
                    if (distance < radius)
                    {
                        currentContent.SetPixel(x, y, circleColor);
                    }
                }
            }
        }
        currentContent.Apply();

        // Copy back to render texture
        Graphics.Blit(currentContent, paintingCanvasRenderTexture);

        // Cleanup
        Destroy(currentContent);

        Debug.Log("Circle drawing completed");
    }
}