using SDRenderer.Lighting;
using SDRenderer.SDObjects;
using SDRenderer.Util;
using System.Collections.Generic;

using static SDRenderer.RenderConfig;

namespace SDRenderer
{
	public class SDScene
	{
		public List<SDObject> Objects { get; set; }

		public SDLighting Lighting { get; }

		public Camera Cam { get; }

		public SDScene(Camera cam, SDLighting lighting, params SDObject[] objects)
		{
			Objects = new List<SDObject>(objects);

			Cam = cam;

			Lighting = lighting;
			Lighting.Scene = this;
		}

		public DirectionalColorData DirectionalColor(Vec3f start, Vec3f direction, int reflections)
		{
			Vec3f pos = start;

			for (int i = 0; i < MaxMarches; i++)
			{
				var objData = ClosestObject(pos);

				if (objData.Distance <= DistanceThreshold)
				{
					Vec3f normal = Gradient(pos);

					pos += 2f * DistanceThreshold * normal;

					var data = new RayData(pos, start, normal, objData.SDObj.Mat);

					ColorIntensity intensity = Lighting.RayIntensity(data);

					if (reflections > 0 && objData.SDObj.Mat.Specular > 0f)
					{
						Vec3f reflectionDir = direction - 2f * normal.Dot(direction) * normal;
						intensity += DirectionalColor(pos, reflectionDir, reflections - 1).Intensity
                            * objData.SDObj.Mat.Specular;
					}

                    return new DirectionalColorData
                    {
                        Intensity = intensity
                    };
				}

				if ((pos - Cam.Pos).Magnitude > MaxRayDistance)
				{
					return Lighting.Background.BackgroundColorData(direction);
				}

				pos += direction * objData.Distance;
			}

			return Lighting.Background.BackgroundColorData(direction);
		}

		public Vec3f Gradient(Vec3f pos)
		{
			float dist = ClosestDistance(pos);

			return new Vec3f(
                (ClosestDistance(pos + new Vec3f(DerivativeDelta, 0f, 0f)) - dist) / DerivativeDelta,
                (ClosestDistance(pos + new Vec3f(0f, DerivativeDelta, 0f)) - dist) / DerivativeDelta,
                (ClosestDistance(pos + new Vec3f(0f, 0f, DerivativeDelta)) - dist) / DerivativeDelta
                ).Normalized();
		}

		public float ClosestDistance(Vec3f pos)
		{
			return ClosestObject(pos).Distance;
		}

		public SDObjectDistance ClosestObject(Vec3f pos)
		{
			if (Objects.Count == 0)
			{
                return new SDObjectDistance { Distance = float.PositiveInfinity };
			}

			SDObject obj = Objects[0];
			float minDist = Objects[0].Distance(pos);

			for (int i = 1; i < Objects.Count; i++)
			{
				float dist = Objects[i].Distance(pos);

				if (dist < minDist)
				{
					minDist = dist;
					obj = Objects[i];
				}
			}

            return new SDObjectDistance
            {
                SDObj = obj,
                Distance = minDist
            };
		}
	}
}
