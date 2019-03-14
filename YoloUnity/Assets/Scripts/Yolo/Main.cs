using UnityEngine;

// NOTE: Comment out BindService method in YoloServiceGrpc.cs, lines 91-94

namespace Yolo
{
    public class Main : MonoBehaviour
    {
        [SerializeField]
        [Range(0f, 1f)]
        float confidenceThreshold = 0;

        ClientManager clientManager;
        SizeConfig sizeConfig;
        Texture2D texture;
        Monitor monitor;
        Cam cam;

        public void Initialize()
        {
            sizeConfig = GetComponent<SizeConfig>();
            sizeConfig.RaiseResizeEvent += OnScreenResize;
            Size size = sizeConfig.Initialize();

            texture = new Texture2D(size.Image.x, size.Image.y, TextureFormat.RGB24, false);
            cam = GameObject.FindObjectOfType<Cam>();
            cam.Initialize(ref texture, size);

            monitor = GameObject.FindObjectOfType<Monitor>();
            monitor.Initialize(size, LabelColors.CreateFromJSON(Resources.Load<TextAsset>("LabelColors").text));

            clientManager = new ClientManager(ref texture);
            clientManager.RaiseDetectionEvent += OnDetection;
        }

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            clientManager.Update();
        }

        void OnDetection(object sender, DetectionEventArgs e)
        {
            monitor.UpdateLabels(e.Result.ToList(confidenceThreshold));
        }

        void OnScreenResize(object sender, ResizeEventArgs e)
        {
            texture.Resize(e.Size.Image.x, e.Size.Image.y);
            monitor.SetSize(e.Size);
            cam.SetSize(e.Size);
        }

        void OnApplicationQuit()
        {
            sizeConfig.RaiseResizeEvent -= OnScreenResize;
            clientManager.RaiseDetectionEvent -= OnDetection;
            clientManager.Dispose();
        }
    }
}