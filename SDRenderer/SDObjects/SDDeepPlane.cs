using System;

using SDRenderer.Lighting;
using SDRenderer.Util;

namespace SDRenderer.SDObjects
{
	public class SDDeepPlane : SDObject
	{
		public Vec3f Pos { get; set; }

		public Vec3f Normal { get; private set; }

		public SDDeepPlane(Vec3f pos, Vec3f normal, Material mat = null)
			: base(mat)
		{
			if (normal.Magnitude == 0f)
				throw new ArgumentException("Normal must be a nonzero vector.");

			Pos = pos;
			SetNormal(normal);
			base.Distance = GetDistance;
		}

		public void SetNormal(Vec3f direction)
		{
			Normal = direction.Normalized();
		}

		private float GetDistance(Vec3f v)
		{
			return (v - Pos).Dot(Normal);
		}
	}
}
