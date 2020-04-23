using System;

namespace SDRenderer.Util
{
	public static class Tools
	{
		public static float CubicInterpolation(float x1, float y1, float x2, float y2, float x)
		{
			float denom = MathF.Pow(x1 - x2, 3f);

			float a = -2f * (y1 - y2) / denom;
			float b = 3f * (x1 + x2) * (y1 - y2) / denom;
			float c = -6f * x1 * x2 * (y1 - y2) / denom;
			float d = (MathF.Pow(x1, 3f) * y2 - 3f * MathF.Pow(x1, 2f) * x2 * y2
                       + (3f * x1) * MathF.Pow(x2, 2f) * y1 - MathF.Pow(x2, 3f) * y1)
                      / denom;

			return a * MathF.Pow(x, 3f) + b * MathF.Pow(x, 2f) + (c * x) + d;
		}

		public static double CubicInterpolation(double x1, double y1, double x2, double y2, double x)
		{
			double denom = Math.Pow(x1 - x2, 3f);

			double a = -2.0 * (y1 - y2) / denom;
			double b = 3.0 * (x1 + x2) * (y1 - y2) / denom;
			double c = -6.0 * x1 * x2 * (y1 - y2) / denom;
			double d = (Math.Pow(x1, 3.0) * y2 - 3.0 * Math.Pow(x1, 2.0) * x2 * y2
                        + 3.0 * x1 * Math.Pow(x2, 2.0) * y1 - Math.Pow(x2, 3.0) * y1)
                       / denom;

			return a * Math.Pow(x, 3.0) + b * Math.Pow(x, 2.0) + c * x + d;
		}

		public static double CubicInterpolation(double y1, double y2, double t)
		{
			return 2.0 * (y1 - y2) * Math.Pow(t, 3.0) + 3.0 * (y2 - y1) * Math.Pow(t, 2.0) + y1;
		}

		public static float CubicInterpolation(float y1, float y2, float t)
		{
			return 2f * (y1 - y2) * MathF.Pow(t, 3f) + 3f * (y2 - y1) * MathF.Pow(t, 2f) + y1;
		}
	}
}
