using System.Drawing;

using SDRenderer.Util;

namespace SDRenderer.Lighting
{
    public class SolidBackground : IBackground
    {
        public Color BgColor { get; set; }

        public float Strength { get; set; }

        public ColorIntensity Intensity => new ColorIntensity(BgColor, Strength);

        public SolidBackground()
        {
            BgColor = Color.Black;
        }

        public SolidBackground(Color color, float strength = 0f)
        {
            BgColor = color;
            Strength = strength;
        }

        public DirectionalColorData BackgroundColorData(Vec3f direction)
        {
            return new DirectionalColorData
            {
                Intensity = Intensity,
                RGB = BgColor
            };
        }
    }
}
