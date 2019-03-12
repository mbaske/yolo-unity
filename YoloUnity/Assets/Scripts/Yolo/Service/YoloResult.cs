using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Yolo
{
    public class YoloResult
    {
        public int ElapsedMilliseconds;
        public Dictionary<string, List<YoloItem>> Dict { get; private set; }

        public YoloResult()
        {
            Dict = new Dictionary<string, List<YoloItem>>();
        }

        public List<YoloItem> ToList(float confidenceThreshold = 0)
        {
            return Dict.Values.SelectMany(x => x).Where(x => x.Confidence >= confidenceThreshold).ToList();
        }

        public void Clear()
        {
            Dict.Clear();
        }

        public void Add(YoloItem item)
        {
            if (!Dict.ContainsKey(item.Type))
            {
                Dict.Add(item.Type, new List<YoloItem>());
            }
            Dict[item.Type].Add(item);
        }
    }
}