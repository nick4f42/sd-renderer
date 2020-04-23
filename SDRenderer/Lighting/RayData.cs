using SDRenderer.Util;

namespace SDRenderer.Lighting
{
	public struct RayData
	{
		public Vec3f Pos;

		public Vec3f RayOrigin;

		public Vec3f Normal;

		public Material Mat;

		public RayData(Vec3f pos, Vec3f rayOrigin, Vec3f normal, Material mat)
		{
			Pos = pos;
			RayOrigin = rayOrigin;
			Normal = normal;
			Mat = mat;
		}
	}
}
