using SDRenderer.Util;
using System;
using System.Drawing;

namespace SDRenderer.Lighting
{
	public class GradientBackground : IBackground
	{
		public Color BottomColor { get; set; }

		public Color TopColor { get; set; }

		public float Strength { get; set; }

		public GradientBackground(Color bottomColor, Color topColor, float strength = 0f)
		{
			BottomColor = bottomColor;
			TopColor = topColor;
			Strength = strength;
		}

		private Color Gradient(float t)
		{
			int red = (int)(BottomColor.R * (1f - t) + TopColor.R * t + 0.5f);
			int green = (int)(BottomColor.G * (1f - t) + TopColor.G * t + 0.5f);
			int blue = (int)(BottomColor.B * (1f - t) + TopColor.B * t + 0.5f);

			return Color.FromArgb(red, green, blue);
		}

		public DirectionalColorData BackgroundColorData(Vec3f direction)
		{
            float pitch;

            if (direction.z > 1)
                pitch = MathF.PI / 2;
            else if (direction.z < -1)
                pitch = -MathF.PI / 2;
            else
			    pitch = MathF.Asin(direction.z);

            Color color = Gradient(pitch / MathF.PI + 0.5f);

            return new DirectionalColorData
            {
                Intensity = new ColorIntensity(color, Strength),
                RGB = color
            };
		}
	}
}
