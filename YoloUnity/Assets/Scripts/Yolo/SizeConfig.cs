using UnityEngine;
using System;

namespace Yolo
{
    public class SizeConfig : MonoBehaviour
    {
        [HideInInspector]
        public event EventHandler<ResizeEventArgs> RaiseResizeEvent;

        [SerializeField]
        int imageWidth = 416;

        Size size;

        public Size Initialize() 
        {
            size = new Size(Screen.width, Screen.height, imageWidth);
            return size;
        }

        void Update()
        {
            if (size.Screen.x != Screen.width || size.Screen.y != Screen.height)
            {
                size = new Size(Screen.width, Screen.height, imageWidth);
                OnRaiseResizeEvent(new ResizeEventArgs(size));
            }
        }

        void OnRaiseResizeEvent(ResizeEventArgs e)
        {
            EventHandler<ResizeEventArgs> handler = RaiseResizeEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public class Size
    {
        public Vector2 Screen { get; private set; }
        public Vector2Int Image { get; private set; }
        public float Factor { get; private set; }

        public Size(float screenWidth, float screenHeight, int imageWidth)
        {
            Screen = new Vector2(screenWidth, screenHeight);
            Factor = screenWidth / (float)imageWidth;
            Image = new Vector2Int(imageWidth, Mathf.RoundToInt(screenHeight / Factor));
        }

        public override string ToString()
        {
            return string.Format("Size Screen:{0} Image:{1} Factor:{2}", Screen, Image, Factor);
        }
    }

    public class ResizeEventArgs : EventArgs
    {
        public Size Size { get; private set; }

        public ResizeEventArgs(Size size)
        {
            Size = size;
        }
    }

    public interface IResizable
    {
        void SetSize(Size size);
    }
}