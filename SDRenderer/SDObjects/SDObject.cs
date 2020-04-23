using System;

using SDRenderer.Util;
using SDRenderer.Lighting;

namespace SDRenderer.SDObjects
{
	public class SDObject
	{
		public Func<Vec3f, float> Distance { get; set; }

		private Material mat;
		public Material Mat
		{
			get => mat ?? Material.MissingMaterial;
            set => mat = value;
		}

		public SDObject(Func<Vec3f, float> distanceFunc, Material mat = null)
		{
			Distance = distanceFunc;
			Mat = mat;
		}

		protected SDObject(Material mat)
		{
			Mat = mat;
		}

		public static SDObject Union(params SDObject[] objects)
		{
			if (objects.Length == 0)
				return new SDObject((Vec3f pos) => float.PositiveInfinity);

			Func<Vec3f, float> distanceFunc = delegate(Vec3f pos)
			{
				float minDist = float.PositiveInfinity;

				foreach (SDObject obj in objects)
				{
					float val = obj.Distance(pos);
					minDist = MathF.Min(minDist, val);
				}
				return minDist;
			};
			return new SDObject(distanceFunc, objects[0].Mat);
		}

		public static SDObject Subtract(SDObject a, SDObject b)
		{
			Func<Vec3f, float> distanceFunc = (Vec3f pos) => 
                MathF.Max(a.Distance(pos), 0f - b.Distance(pos));

			return new SDObject(distanceFunc, a.Mat);
		}

		public static SDObject Intersect(SDObject a, SDObject b)
		{
			Func<Vec3f, float> distanceFunc = (Vec3f pos) => 
                MathF.Max(a.Distance(pos), b.Distance(pos));

			return new SDObject(distanceFunc, a.Mat);
		}

		public static SDObject Expand(SDObject obj, float amount)
		{
			Func<Vec3f, float> distanceFunc = (Vec3f pos) => 
                obj.Distance(pos) - amount;

			return new SDObject(distanceFunc, obj.Mat);
		}

		public static SDObject Shrink(SDObject obj, float amount)
		{
			Func<Vec3f, float> distanceFunc = (Vec3f pos) => 
                obj.Distance(pos) + amount;

			return new SDObject(distanceFunc, obj.Mat);
		}

		public static SDObject RepeatX(SDObject obj, float center, float repeatWidth)
		{
			Func<Vec3f, float> distanceFunc = delegate(Vec3f pos)
			{
				float x = pos.x - (center - repeatWidth / 2f);
				x = x - repeatWidth * MathF.Floor(x / repeatWidth) - repeatWidth / 2f;

				return obj.Distance(new Vec3f(x, pos.y, pos.z));
			};

			return new SDObject(distanceFunc, obj.Mat);
		}

		public static SDObject RepeatY(SDObject obj, float center, float repeatWidth)
		{
			Func<Vec3f, float> distanceFunc = delegate(Vec3f pos)
			{
				float y = pos.y - (center - repeatWidth / 2f);
				y = y - repeatWidth * MathF.Floor(y / repeatWidth) - repeatWidth / 2f;

				return obj.Distance(new Vec3f(pos.x, y, pos.z));
			};

			return new SDObject(distanceFunc, obj.Mat);
		}

		public static SDObject RepeatZ(SDObject obj, float center, float repeatWidth)
		{
			Func<Vec3f, float> distanceFunc = delegate(Vec3f pos)
			{
				float z = pos.z - (center - repeatWidth / 2f);
				z = z - repeatWidth * MathF.Floor(z / repeatWidth) - repeatWidth / 2f;

				return obj.Distance(new Vec3f(pos.x, pos.y, z));
			};
			return new SDObject(distanceFunc, obj.Mat);
		}

		public static SDObject RepeatXYZ(SDObject obj, Vec3f center, Vec3f repeatWidths)
		{
			Func<Vec3f, float> distanceFunc = delegate(Vec3f pos)
			{
				float x = pos.x - (center.x - repeatWidths.x / 2f);
				float y = pos.y - (center.y - repeatWidths.y / 2f);
				float z = pos.z - (center.z - repeatWidths.z / 2f);

				x = x - repeatWidths.x * MathF.Floor(x / repeatWidths.x) - repeatWidths.x / 2f;
				y = y - repeatWidths.y * MathF.Floor(y / repeatWidths.y) - repeatWidths.y / 2f;
				z = z - repeatWidths.z * MathF.Floor(z / repeatWidths.z) - repeatWidths.z / 2f;

				return obj.Distance(new Vec3f(x, y, z));
			};

			return new SDObject(distanceFunc, obj.Mat);
		}
	}
}
