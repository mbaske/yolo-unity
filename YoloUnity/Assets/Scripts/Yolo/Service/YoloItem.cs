using UnityEngine;

namespace Yolo
{
    public class YoloItem
    {
        public string Type { get; private set; }
        public float Confidence { get; private set; }
        public RectInt Rect { get; private set; }

        public YoloItem(string type, double confidence, int x, int y, int width, int height)
        {
            Type = type;
            Confidence = (float)confidence;
            Rect = new RectInt(x, y, width, height);
        }

        public void Expand(RectInt rect)
        {
            Rect.SetMinMax(
                Vector2Int.Min(Rect.min, rect.min), 
                Vector2Int.Max(Rect.max, rect.max)
            );
        }

        public override string ToString()
        {
            return string.Format("YoloItem Type:{0} Conf:{1} Rect:{2}", Type, Confidence, Rect);
        }
    }
}