using System;
using Modding;

namespace AsciiCamera
{
    [Serializable]
    public class Settings : ModSettings
    {
        public float Scale = 1;

        public float BlendRatio = 1;

        public string Tint = "#FFFFFF";
    }
}