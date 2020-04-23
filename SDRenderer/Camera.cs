using System;

using SDRenderer.Util;

namespace SDRenderer
{
	public class Camera
	{

        int width;
        public int Width
		{
            get => width;
			set
			{
				width = value;
				UpdatePlaneHeight();
			}
		}

        int height;
        public int Height
		{
            get => height;
			set
			{
				height = value;
				UpdatePlaneHeight();
			}
		}

		public Vec3f Pos { get; set; }

        float yaw;
        public float Yaw
		{
            get => yaw;
			set
			{
				yaw = value;
				transform.Update(Yaw, Pitch, Roll);
			}
		}

        float pitch;
		public float Pitch
		{
            get => pitch;
			set
			{
				pitch = value;
				transform.Update(Yaw, Pitch, Roll);
			}
		}

        float roll;
		public float Roll
		{
            get => roll;
			set
			{
				roll = value;
				transform.Update(Yaw, Pitch, Roll);
			}
		}

        float fov;
		public float FOV
		{
            get => fov;
			set
			{
				fov = value;
				UpdatePlaneWidth();
			}
		}

        Transform transform;

        float planeWidth;
        float planeHeight;

        public Camera(int width, int height, float fov = MathF.PI / 2f)
		{
			Pos = default(Vec3f);
			Width = width;
			Height = height;
			FOV = fov;
			transform = new Transform(Yaw, Pitch, Roll);
		}

		private void UpdatePlaneWidth()
		{
			planeWidth = 2f * MathF.Tan(FOV / 2f);
			UpdatePlaneHeight();
		}

		private void UpdatePlaneHeight()
		{
			planeHeight = planeWidth * (float)Height / Width;
		}

		public void LookAt(Vec3f pos)
		{
			float xyDist = MathF.Sqrt(MathF.Pow(pos.x - Pos.x, 2f) + MathF.Pow(pos.y - Pos.y, 2f));

			Pitch = MathF.Atan((pos.z - Pos.z) / xyDist);

			Yaw = 0f - MathF.Atan((pos.x - Pos.x) / (pos.y - Pos.y));

			if (pos.y - Pos.y < 0f)
				Yaw -= MathF.PI;
		}

		public Vec3f RayDirection(int x, int y)
		{
			Vec3f v = new Vec3f(((float)x / Width - 0.5f) * planeWidth, 1f, 
                                (0.5f - (float)y / Height) * planeHeight);

			return transform.Apply(v).Normalized();
		}
	}
}
