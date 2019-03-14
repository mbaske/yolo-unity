using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yolo
{
    public class Monitor : MonoBehaviour, IResizable
    {
        Label labelTemplate;
        List<Label> labels;
        RectTransform rect;
        LabelColors labelColors;
        Size size;

        public void Initialize(Size size, LabelColors labelColors)
        {
            labelTemplate = transform.GetComponentInChildren<Label>();
            labels = new List<Label>() { labelTemplate };
            rect = GetComponent<RectTransform>();

            this.labelColors = labelColors;
            SetSize(size);
        }

        public void SetSize(Size size)
        {
            this.size = size;
            rect.sizeDelta = new Vector2(Screen.width, Screen.height);
        }

        public void UpdateLabels(List<YoloItem> list)
        {
            int diff = list.Count - labels.Count;
            if (diff > 0)
            {
                CreateMissingLabels(diff);
            }

            for (int i = 0; i < labels.Count; i++)
            {
                if (i < list.Count)
                {
                    labels[i].OnUpdate(size, labelColors.GetColor(list[i].Type), list[i]);
                }
                else
                {
                    labels[i].OnUpdate();
                }
            }
        }

        void CreateMissingLabels(int n)
        {
            for (int i = 0; i < n; i++)
            {
                labels.Add(Instantiate(labelTemplate, transform).GetComponent<Label>());
            }
        }
    }
}