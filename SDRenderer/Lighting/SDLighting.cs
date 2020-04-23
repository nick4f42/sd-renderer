using System;
using System.Collections.Generic;
using System.Drawing;

namespace SDRenderer.Lighting
{
    public class SDLighting
    {
        public List<ILight> Lights { get; set; }

        public SDScene Scene { get; internal set; }

		public Color AmbientColor { get; set; }

		public float AmbientStrength { get; set; }
		public ColorIntensity AmbientIntensity => new ColorIntensity(AmbientColor, AmbientStrength);

		public IBackground Background { get; set; }

		public SDLighting(IBackground background, params ILight[] lights)
			: this(background, Color.Black, 0f, lights)
		{
		}

		public SDLighting(IBackground background, Color ambientColor, float ambientStrength, params ILight[] lights)
		{
			AmbientColor = ambientColor;
			AmbientStrength = ambientStrength;

			Background = background;

			Lights = new List<ILight>(lights);
		}

		public ColorIntensity RayIntensity(RayData data)
		{
			if (Scene == null)
			{
				throw new MemberAccessException("A scene must be stored in Lighting.Scene");
			}

			ColorIntensity color = new ColorIntensity();
			foreach (ILight light in Lights)
			{
				color += light.RayIntensity(Scene, data);
			}

			return color + data.Mat.AmbientIntensity * AmbientIntensity;
		}
	}
}
