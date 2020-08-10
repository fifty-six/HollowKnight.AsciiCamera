using System.IO;
using System.Linq;
using System.Reflection;
using Modding;
using UnityEngine;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Vasi;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace AsciiCamera
{
    [UsedImplicitly]
    public class AsciiCamera : Mod
    {
        private Shader _shader;

        private Settings _settings = new Settings();

        public override string GetVersion() => VersionUtil.GetVersion<AsciiCamera>();

        public override ModSettings GlobalSettings
        {
            get => _settings;
            set => _settings = (Settings) value;
        }

        public override void Initialize()
        {
            LoadShader();

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name != Constants.MENU_SCENE)
                return;
            
            ApplyCamera();
        }

        private void LoadShader()
        {
            var asm = Assembly.GetExecutingAssembly();

            string bundle = asm.GetManifestResourceNames().First(x => x.Contains("camera"));

            using Stream stream = asm.GetManifestResourceStream(bundle);

            AssetBundle ab = AssetBundle.LoadFromStream(stream);

            if (ab == null)
                return;

            Shader[] shaders = ab.LoadAllAssets<Shader>();

            if (shaders == null || shaders.Length == 0)
                return;

            _shader = shaders[0];
        }

        private void ApplyCamera()
        {
            tk2dCamera tk2dCam = GameCameras.instance.tk2dCam;
            Camera cam = Mirror.GetField<tk2dCamera, Camera>(tk2dCam, "_unityCamera");

            var fx = cam.gameObject.AddComponent<AsciiArtFx>();
            fx.Tint = ColorUtility.TryParseHtmlString(_settings.Tint, out Color c) ? c : Color.white;
            fx.BlendRatio = _settings.BlendRatio;
            fx.ScaleFactor = _settings.Scale;
            fx.Shader = _shader;
        }
    }
}