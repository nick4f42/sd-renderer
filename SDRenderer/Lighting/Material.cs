using System.Drawing;

namespace SDRenderer.Lighting  
{
	public class Material
	{
		public float Shininess;

		public Color MatColor { get; set; }

        public float Diffuse { get; set; }

        public float Specular { get; set; }

        public float Ambient { get; set; }

        public ColorIntensity DiffuseIntensity => new ColorIntensity(MatColor, Diffuse / 255f);

		public ColorIntensity AmbientIntensity => new ColorIntensity(MatColor, Ambient / 255f);

		public static Material MissingMaterial = new Material(Color.Red, 100f, 0f, 0f, 100f);

		public Material(Color color, float diffuse, float specular, float shininess, float ambient)
		{
			MatColor = color;
			Diffuse = diffuse;
			Specular = specular;
			Shininess = shininess;
			Ambient = ambient;
		}
	}
}
