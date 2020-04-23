using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

using SDRenderer.Lighting;
using SDRenderer.Util;

namespace SDRenderer
{
	public class Renderer
	{
		public SDScene Scene { get; }

		public Renderer(SDScene scene)
		{
			Scene = scene;
		}

		public Image Render()
		{
			Bitmap bitmap = new Bitmap(Scene.Cam.Width, Scene.Cam.Height);

			Graphics graphics = Graphics.FromImage(bitmap);

			byte[] countLock = new byte[0];

			int count = 0;

			for (int i = 0; i < Scene.Cam.Width; i++)
			{
				for (int j = 0; j < Scene.Cam.Height; j++)
				{
					int _x = i;
					int _y = j;

					Task task = new Task(() =>
					{
						Color color = MarchRay(_x, _y);
						SolidBrush brush = new SolidBrush(color);

						lock (graphics)
							graphics.FillRectangle(brush, _x, _y, 1, 1);
						lock (countLock)
                            count++;
					});

					task.Start();
				}
			}
			while (count < Scene.Cam.Width * Scene.Cam.Height - 1)
				Thread.Sleep(200);

			return bitmap;
		}

		private Color MarchRay(int x, int y)
		{
			Vec3f pos = Scene.Cam.Pos;

			Vec3f direction = Scene.Cam.RayDirection(x, y);

			var data = Scene.DirectionalColor(pos, direction, RenderConfig.MaxReflections);

			if (data.RGB == Color.Empty)
				return data.Intensity.ToRGB(RenderConfig.Exposure);

			return data.RGB;
		}
	}
}
