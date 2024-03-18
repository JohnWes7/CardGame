using UnityEngine;
using UnityEngine.UI;

namespace CG.MagicMenu 
{
    public abstract class Menu<T> : Menu where T : Menu<T>
    {
        private static T instance;
        private static T Instance { get { return instance; } }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T)this;
            }
        }

        protected virtual void OnDestroy() => instance = null;

        public static void Open()
        {
            if (MagicMenuInstance != null && Instance != null)
            {
                MagicMenuInstance.OpenMenu(Instance);
            }
        }

        public static void OpenOnTop()
        {
            if (MagicMenuInstance != null && Instance != null)
            {
                MagicMenuInstance.OpenMenu(Instance, true);
            }
        }

        public static void Close()
        {
            if (MagicMenuInstance != null && Instance != null)
            {
                MagicMenuInstance.CloseMenu();
            }
        }
    }

    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class Menu : MonoBehaviour
    {
        [SerializeField] bool showFirst;
        public bool ShowFirst => showFirst;

        private void Reset()
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
            //canvas.vertexColorAlwaysGammaSpace = true;

            CanvasScaler scaler = GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }

        public static MagicMenu MagicMenuInstance { get; private set; }

        public virtual void OnBackPressed() => MagicMenuInstance.CloseMenu();

        public void Initialize(MagicMenu magicMenu) => MagicMenuInstance = magicMenu;
    }
}


