using System;

namespace SDRenderer.Util
{
	public struct Vec3f
	{
		public readonly float x;

		public readonly float y;

		public readonly float z;

		public static Vec3f I => new Vec3f(1f, 0f, 0f);

		public static Vec3f J => new Vec3f(0f, 1f, 0f);

		public static Vec3f K => new Vec3f(0f, 0f, 1f);

		public float Magnitude => MathF.Sqrt(x * x + y * y + z * z);

		public Vec3f(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public float Dot(Vec3f other)
		{
			return Dot(this, other);
		}

		public static float Dot(Vec3f a, Vec3f b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		public Vec3f Cross(Vec3f other)
		{
			return Cross(this, other);
		}

		public static Vec3f Cross(Vec3f a, Vec3f b)
		{
			return new Vec3f(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
		}

		public Vec3f Normalized()
		{
			return this / Magnitude;
		}

		public static Vec3f operator *(float scalar, Vec3f v)
		{
			return new Vec3f(v.x * scalar, v.y * scalar, v.z * scalar);
		}

		public static Vec3f operator *(Vec3f v, float scalar)
		{
			return scalar * v;
		}

		public static Vec3f operator /(Vec3f v, float scalar)
		{
			return 1f / scalar * v;
		}

		public static Vec3f operator -(Vec3f v)
		{
			return -1f * v;
		}

		public static Vec3f operator +(Vec3f a, Vec3f b)
		{
			return new Vec3f(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Vec3f operator -(Vec3f a, Vec3f b)
		{
			return a + -b;
		}

		public override string ToString()
		{
			return "<" + x + ", " + y + ", " + z + ">";
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y, z);
		}

        public override bool Equals(object obj)
        {
            return obj is Vec3f v
                   && x == v.x
                   && y == v.y
                   && z == v.z;
        }
    }
}
