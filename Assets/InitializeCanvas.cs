using UnityEngine;

public class InitializeCanvas : MonoBehaviour
{
    public RenderTexture paintingCanvasRenderTexture;

    void Start()
    {
        if (paintingCanvasRenderTexture == null)
        {
            Debug.LogError("paintingCanvasRenderTexture not assigned in InitializeCanvas!");
            return;
        }

        // Create a temporary texture for the white background
        Texture2D tempTexture = new Texture2D(paintingCanvasRenderTexture.width, paintingCanvasRenderTexture.height);
        Color[] pixels = new Color[tempTexture.width * tempTexture.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }
        tempTexture.SetPixels(pixels);
        tempTexture.Apply();

        // Store current active render texture
        RenderTexture previousActive = RenderTexture.active;

        // Set our render texture as active
        RenderTexture.active = paintingCanvasRenderTexture;

        // Clear it first
        GL.Clear(true, true, Color.white);

        // Draw the white texture
        Graphics.Blit(tempTexture, paintingCanvasRenderTexture);

        // Restore previous active render texture
        RenderTexture.active = previousActive;

        // Cleanup
        Destroy(tempTexture);

        Debug.Log("Canvas initialized with white background");
    }
}