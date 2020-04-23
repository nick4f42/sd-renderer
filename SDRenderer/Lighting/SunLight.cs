using SDRenderer.Util;
using System;
using System.Drawing;

namespace SDRenderer.Lighting
{
	public class SunLight : ILight
	{
		public Vec3f SunDirection { get; private set; }

		public Color LightColor { get; set; }

		public float Diffuse { get; set; }

		public float Specular { get; set; }

		public float Softness { get; set; }

		public ColorIntensity DiffuseIntensity => new ColorIntensity(LightColor, Diffuse);

		public ColorIntensity SpecularIntensity => new ColorIntensity(LightColor, Specular);

		public SunLight(Vec3f direction, Color color, float diffuse, float specular, float softness = 0f)
		{
			SetLightDirection(direction);

			LightColor = color;

			Diffuse = diffuse;
			Specular = specular;

			Softness = softness;
		}

		public void SetLightDirection(Vec3f direction)
		{
			SunDirection = -direction.Normalized();
		}

		private float ClosestLightPass(Func<Vec3f, float> sdf, Vec3f pos)
		{
			Vec3f originalPos = pos;

			float[] dists = new float[3]
			{
				float.PositiveInfinity,
				float.NegativeInfinity,
				float.NegativeInfinity
			};

			float localMin = float.PositiveInfinity;

			for (int i = 0; i < RenderConfig.MaxLightMarches; i++)
			{
				dists[0] = sdf(pos);

				if (dists[0] <= RenderConfig.DistanceThreshold)
					return RenderConfig.DistanceThreshold;

				if ((pos - originalPos).Magnitude > RenderConfig.MaxRayDistance)
					return localMin;

				if (dists[2] > dists[1] && dists[0] > dists[1] && dists[1] < localMin)
					localMin = dists[1];

				pos += dists[0] * SunDirection;

				dists[2] = dists[1];
				dists[1] = dists[0];
			}

			return RenderConfig.DistanceThreshold;
		}

		private float LightPassValue(Func<Vec3f, float> sdf, Vec3f pos)
		{
			float localMin = ClosestLightPass(sdf, pos);

            if (Softness < RenderConfig.DistanceThreshold)
                return (localMin > RenderConfig.DistanceThreshold) ? 1 : 0;

            if (localMin > Softness)
				return 1f;

			return Tools.CubicInterpolation(RenderConfig.DistanceThreshold, 0f, Softness, 1f, localMin);
		}

		public ColorIntensity RayIntensity(SDScene scene, RayData data)
		{
			ColorIntensity color = new ColorIntensity();

			float lightPass = LightPassValue(scene.ClosestDistance, data.Pos);

			if (lightPass == 0f)
				return color;

			float diffuseDot = SunDirection.Dot(data.Normal);
			if (diffuseDot > 0f)
			{
				color += data.Mat.DiffuseIntensity * DiffuseIntensity * diffuseDot * lightPass;

				Vec3f reflectionDir = 2f * SunDirection.Dot(data.Normal) * data.Normal - SunDirection;
				Vec3f viewerDir = (data.RayOrigin - data.Pos).Normalized();
				float specularDot = reflectionDir.Dot(viewerDir);
				if (specularDot > 0f)
				{
					color += SpecularIntensity * data.Mat.Specular
                        * MathF.Pow(specularDot, data.Mat.Shininess) * lightPass;
				}
			}
			return color;
		}
	}
}
