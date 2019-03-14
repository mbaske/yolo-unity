using UnityEngine;
using UnityEngine.UI;

namespace Yolo
{
    public class Label : MonoBehaviour
    {
        Image frame;
        RectTransform frameRect;

        Text text;
        RectTransform textRect;
        Vector2 textOffset;
        float textHeight;

        public void OnUpdate(Size size, Color color, YoloItem item)
        {
            gameObject.SetActive(true);

            frame.color = color;
            text.color = color.grayscale > 0.5f ? Color.black : Color.white;
            
            RectInt r = item.Rect;
            frameRect.offsetMin = new Vector2(
                r.x * size.Factor, (size.Image.y - r.height - r.y) * size.Factor);
            frameRect.offsetMax = new Vector2(
                (r.x - (size.Image.x - r.width)) * size.Factor, -r.y * size.Factor);

            text.text = item.Type + " " + Mathf.Round(item.Confidence * 100) + "%";
            textRect.anchoredPosition = new Vector2(
                (r.width * size.Factor) / 2 + textOffset.x, textOffset.y);
            textRect.sizeDelta = new Vector2(r.width * size.Factor, textHeight);
        }

        public void OnUpdate()
        {
            gameObject.SetActive(false);
        }

        void Awake()
        {
            frame = transform.GetComponent<Image>();
            frameRect = transform.GetComponent<RectTransform>();

            text = transform.GetComponentInChildren<Text>();
            textRect = text.GetComponent<RectTransform>();
            textOffset = new Vector2(textRect.anchoredPosition.x - textRect.sizeDelta.x / 2, textRect.anchoredPosition.y);
            textHeight = textRect.sizeDelta.y;
        }
    }
}
