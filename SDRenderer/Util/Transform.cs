using System;

namespace SDRenderer.Util
{
	internal class Transform
	{
		private float[,] matrix = new float[3, 3];

		public Transform()
		{
			Update(0f, 0f, 0f);
		}

		public Transform(float yaw, float pitch, float roll)
		{
			Update(yaw, pitch, roll);
		}

		public void Update(float yaw, float pitch, float roll)
		{
			matrix[0, 0] = (MathF.Cos(yaw) * MathF.Cos(roll) + MathF.Sin(yaw) * MathF.Sin(pitch) * MathF.Sin(roll));
			matrix[0, 1] = ((0f - MathF.Sin(yaw)) * MathF.Cos(pitch));
			matrix[0, 2] = (MathF.Sin(yaw) * MathF.Sin(pitch) * MathF.Cos(roll) - MathF.Cos(yaw) * MathF.Sin(roll));
			matrix[1, 0] = (MathF.Sin(yaw) * MathF.Cos(roll) - MathF.Cos(yaw) * MathF.Sin(pitch) * MathF.Sin(roll));
			matrix[1, 1] = (MathF.Cos(yaw) * MathF.Cos(pitch));
			matrix[1, 2] = ((0f - MathF.Cos(yaw)) * MathF.Sin(pitch) * MathF.Cos(roll) - MathF.Sin(yaw) * MathF.Sin(roll));
			matrix[2, 0] = (MathF.Cos(pitch) * MathF.Sin(roll));
			matrix[2, 1] = MathF.Sin(pitch);
			matrix[2, 2] = (MathF.Cos(pitch) * MathF.Cos(roll));
		}

		public Vec3f Apply(Vec3f v)
		{
			return new Vec3f(matrix[0, 0] * v.x + matrix[0, 1] * v.y + matrix[0, 2] * v.z, matrix[1, 0] * v.x + matrix[1, 1] * v.y + matrix[1, 2] * v.z, matrix[2, 0] * v.x + matrix[2, 1] * v.y + matrix[2, 2] * v.z);
		}
	}
}
