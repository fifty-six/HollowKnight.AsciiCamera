using UnityEngine;

namespace AsciiCamera
{
    [RequireComponent(typeof(Camera))]
    public class AsciiArtFx : MonoBehaviour
    {
        private static readonly int ALPHA = Shader.PropertyToID("_Alpha");
        private static readonly int SCALE = Shader.PropertyToID("_Scale");

        public Color Tint;

        [Range(0, 1)]
        public float BlendRatio;

        [Range(0.5f, 10.0f)]
        public float ScaleFactor;

        public Shader Shader { get; set; }

        private Material _material;

        private Material material
        {
            get
            {
                if (_material != null)
                    return _material;

                return _material = new Material(Shader)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
            }
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            material.color = Tint;

            material.SetFloat(ALPHA, BlendRatio);
            material.SetFloat(SCALE, ScaleFactor);

            Graphics.Blit(source, destination, material);
        }

        private void OnDisable()
        {
            if (_material)
                DestroyImmediate(_material);
        }
    }
}