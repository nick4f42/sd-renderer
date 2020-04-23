using System;
using System.Drawing;

namespace SDRenderer.Lighting
{
    public struct ColorIntensity
    {
        public readonly float R;

        public readonly float G;

        public readonly float B;

        public ColorIntensity(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public ColorIntensity(Color rgbColor, float strength)
        {
            R = rgbColor.R * strength;
            G = rgbColor.G * strength;
            B = rgbColor.B * strength;
        }

        public ColorIntensity AtDistance(float distance)
        {
            return this / (1f + MathF.Pow(distance, 2f));
        }

        public Color ToRGB(float exposure)
        {
            byte red = (byte)(255.0 * MathF.Atan(exposure * R) * 2.0 / MathF.PI + 1.0);
            byte green = (byte)(255.0 * MathF.Atan(exposure * G) * 2.0 / MathF.PI + 1.0);
            byte blue = (byte)(255.0 * MathF.Atan(exposure * B) * 2.0 / MathF.PI + 1.0);

            return Color.FromArgb(red, green, blue);
        }

        public static ColorIntensity operator +(ColorIntensity a, ColorIntensity b)
        {
            return new ColorIntensity(a.R + b.R, a.G + b.G, a.B + b.B);
        }

        public static ColorIntensity operator *(ColorIntensity a, ColorIntensity b)
        {
            return new ColorIntensity(a.R * b.R, a.G * b.G, a.B * b.B);
        }

        public static ColorIntensity operator *(dynamic scalar, ColorIntensity color)
        {
            return new ColorIntensity(color.R * (float)scalar, 
                color.G * (float)scalar, 
                color.B * (float)scalar);
        }

        public static ColorIntensity operator *(ColorIntensity color, dynamic scalar)
        {
            return scalar * color;
        }

        public static ColorIntensity operator /(ColorIntensity color, dynamic scalar)
        {
            return 1 / scalar * color;
        }

        public override string ToString()
        {
            return "<" + R + ", " + G + ", " + B + ">";
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(R, G, B);
        }

        public override bool Equals(object obj)
        {
            return obj is ColorIntensity intensity
                   && R == intensity.R
                   && G == intensity.G
                   && B == intensity.B;
        }
    }
}
