using System.Drawing;

namespace SDRenderer.Lighting
{
    public interface ILight
    {
        Color LightColor { get; set; }

        float Diffuse { get; set; }

        float Specular { get; set; }

        ColorIntensity DiffuseIntensity { get; }

        ColorIntensity SpecularIntensity { get; }

		ColorIntensity RayIntensity(SDScene scene, RayData data);
	}
}
