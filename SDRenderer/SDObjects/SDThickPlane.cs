using System;

using SDRenderer.Lighting;
using SDRenderer.Util;

namespace SDRenderer.SDObjects
{
	public class SDThickPlane : SDObject
	{
		public Vec3f Pos { get; set; }

		public Vec3f Normal { get; private set; }

		public float Thickness { get; set; }

		public SDThickPlane(Vec3f pos, Vec3f normal, float thickness, Material mat = null)
			: base(mat)
		{
			if (normal.Magnitude == 0f)
				throw new ArgumentException("Normal must be a nonzero vector.");

			Pos = pos;
			SetNormal(normal);
			Thickness = thickness;

			base.Distance = GetDistance;
		}

		public void SetNormal(Vec3f direction)
		{
			Normal = direction.Normalized();
		}

		private float GetDistance(Vec3f v)
		{
			return MathF.Abs((v - Pos).Dot(Normal)) - (Thickness + RenderConfig.DistanceThreshold) / 2f;
		}
	}
}
