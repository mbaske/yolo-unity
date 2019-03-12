using UnityEngine;

namespace Yolo
{
    public class Cam : MonoBehaviour, IResizable
    {
        Camera cam;
        Texture2D tex;
        Rect rect;

        public void Initialize(ref Texture2D texture, Size size)
        {
            tex = texture;
            cam = GetComponent<Camera>();
            SetSize(size);
        }

        public void SetSize(Size size)
        {
            if (cam.targetTexture != null)
            {
                cam.targetTexture.Release();
            }

            rect = new Rect(0, 0, size.Image.x, size.Image.y);
            cam.targetTexture = new RenderTexture(
                size.Image.x, size.Image.y, 16, RenderTextureFormat.ARGB32);
        }

        void OnPostRender()
        {
            // TODO should only read texture when needed for request.
            tex.ReadPixels(rect, 0, 0, false);
            tex.Apply();
            cam.targetTexture.DiscardContents();
        }

        // void OnGUI()
        // {
        //     GUI.DrawTexture(rect, tex);
        // }
    }
}