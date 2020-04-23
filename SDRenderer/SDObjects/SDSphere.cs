using SDRenderer.Util;
using SDRenderer.Lighting;

namespace SDRenderer.SDObjects
{
	public class SDSphere : SDObject
	{
		public Vec3f Pos { get; set; }

		public float Radius { get; set; }

		public SDSphere(Vec3f pos, float radius, Material mat = null)
			: base(mat)
		{
			Pos = pos;
			Radius = radius;
			base.Distance = GetDistance;
		}

		private float GetDistance(Vec3f v)
		{
			return (v - Pos).Magnitude - Radius;
		}
	}
}
