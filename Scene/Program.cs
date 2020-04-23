using System;
using System.Drawing;
using System.IO;

using SDRenderer;
using SDRenderer.Lighting;
using SDRenderer.SDObjects;
using SDRenderer.Util;

using static System.MathF;

namespace Scene
{
    internal class Program
    {
        private const string SceneFolder = "../../../Scenes/";

        private static void Main(string[] args)
        {
            //Scene1();

            //Scene2();

            //Scene3();

            Scene4();

            Scene5();
        }

        private static void Scene1()
        {
            RenderConfig.MaxMarches = 3000;
            RenderConfig.DistanceThreshold = 0.001f;
            RenderConfig.DerivativeDelta = 0.0001f;
            RenderConfig.MaxLightMarches = 3000;
            RenderConfig.MaxReflections = 2;
            RenderConfig.MaxRayDistance = 10000f;
            RenderConfig.Exposure = 1f;

            Material ballMat = new Material(Color.Orange, 0.7f, 0.4f, 30f, 1f);
            Material floorMat = new Material(Color.Blue, 0.02f, 0.9f, 50f, 1f);

            SDObject sDObject = new SDObject((Vec3f pos) =>
            {
                return new Vec3f(Abs(pos.x % 6f) - 3f, Abs(pos.y % 6f) - 3f, pos.z).Magnitude - 2f;
            }, ballMat);

            SDObject sDObject2 = new SDObject((Vec3f pos) => 
                (pos - new Vec3f(0f, 0f, 0f)).Magnitude - 2f, ballMat);

            SDObject floor = new SDObject((Vec3f pos) => pos.z + 2f, floorMat);

            SunLight light = new SunLight(new Vec3f(0.5f, 0.5f, -1f), Color.White, 0.005f, 0.005f, 0.4f);

            Camera cam = new Camera(1920, 1080);

            cam.Pitch = -MathF.PI / 10f;

            SolidBackground bg = new SolidBackground(Color.Black, 0.0001f);

            SDLighting lighting = new SDLighting(bg, Color.White, 0.001f);
            lighting.Lights.Add(light);

            SDScene scene = new SDScene(cam, lighting, sDObject, floor);

            Renderer renderer = new Renderer(scene);

            int frames = 30;

            Directory.CreateDirectory("../../../Scenes/Scene1/anim");

            for (int i = 0; i < frames; i++)
            {
                cam.Pos = new Vec3f(0f, -13f + 6f * (float)i / (float)frames, 6f);
                Image img = renderer.Render();
                img.Save(string.Format("{0}Scene1/anim/frame_{1}.png", "../../../Scenes/", i));
                Console.WriteLine($"Frame {i + 1} out of {frames} complete.");
            }
        }

        private static void Scene2()
        {
            RenderConfig.MaxMarches = 1000;
            RenderConfig.DistanceThreshold = 0.001f;
            RenderConfig.DerivativeDelta = 0.0001f;
            RenderConfig.MaxLightMarches = 3000;
            RenderConfig.MaxReflections = 150;
            RenderConfig.MaxRayDistance = 10000f;
            RenderConfig.Exposure = 1f;

            Material mirrorMat = new Material(Color.White, 0f, 1f, 100f, 0.2f);
            Material floorMat = new Material(Color.White, 0.3f, 0.01f, 5f, 1f);

            Material ballMat = new Material(Color.Blue, 1f, 0.3f, 30f, 1f);

            SDSphere ball = new SDSphere(default(Vec3f), 1f, ballMat);

            float wallDist = 5f;

            SDObject sDObject = SDObject.Union(
                new SDPlane(new Vec3f(0f, wallDist, 0f), new Vec3f(0f, 1f, 0f)),
                new SDPlane(new Vec3f(0f, 0f - wallDist, 0f), new Vec3f(0f, 1f, 0f)),
                new SDPlane(new Vec3f(wallDist, 0f, 0f), new Vec3f(1f, 0f, 0f)),
                new SDPlane(new Vec3f(0f - wallDist, 10f, 0f), new Vec3f(1f, 0f, 0f)),
                new SDPlane(new Vec3f(0f, 0f, wallDist), new Vec3f(0f, 0f, 1f)));

            sDObject.Mat = mirrorMat;

            SDPlane floor = new SDPlane(new Vec3f(0f, 0f, 0f - wallDist), new Vec3f(0f, 0f, 1f), floorMat);

            Camera cam = new Camera(1920, 1080);

            cam.Pos = new Vec3f(-3f, -4f, 2f);
            cam.LookAt(default(Vec3f));

            PointLight light = new PointLight(new Vec3f(1f, -4f, 3f), Color.White, 0.1f, 0.1f);

            SolidBackground bg = new SolidBackground();
            SDLighting lighting = new SDLighting(bg, Color.White, 0.001f, light);

            SDScene scene = new SDScene(cam, lighting, ball, sDObject, floor);

            Renderer renderer = new Renderer(scene);

            Directory.CreateDirectory("../../../Scenes/Scene2");
            string imgDir = "../../../Scenes/Scene2/";

            renderer.Render().Save(imgDir + "img.png");
        }

        private static void Scene3()
        {
            RenderConfig.MaxMarches = 1000;
            RenderConfig.DistanceThreshold = 0.001f;
            RenderConfig.DerivativeDelta = 0.0001f;
            RenderConfig.MaxLightMarches = 3000;
            RenderConfig.MaxReflections = 2;
            RenderConfig.MaxRayDistance = 10000f;
            RenderConfig.Exposure = 1f;

            Material mat = new Material(Color.Blue, 1f, 0.3f, 30f, 1f);
            Material floorMat = new Material(Color.White, 0.1f, 0.15f, 50f, 0.3f);

            SDSphere ball = new SDSphere(new Vec3f(-0.5f, 0f, 0f), 1f, mat);
            SDSphere ball2 = new SDSphere(new Vec3f(0.5f, 0f, 0f), 1f, mat);
            SDSphere ball3 = new SDSphere(new Vec3f(0f, 0f, 0f), 0.7f);
            SDObject obj = SDObject.Subtract(SDObject.Intersect(ball, ball2), ball3);

            SDPlane floor = new SDPlane(new Vec3f(0f, 0f, -Sqrt(3) / 2f), 
                new Vec3f(0f, 0f, 1f), floorMat);

            Camera cam = new Camera(1920, 1080);

            PointLight light1 = new PointLight(
                new Vec3f(5f * Cos(0), 5f * Sin(0), 2f),
                Color.Green, 0.4f, 0.4f, 0.1f);

            PointLight light2 = new PointLight(
                new Vec3f(5f * Cos(PI * 2 / 3), 5f * Sin(PI * 2 / 3), 2f),
                Color.Red, 0.4f, 0.4f, 0.1f);

            PointLight light3 = new PointLight(
                new Vec3f(5f * Cos(PI * 4 / 3), 5f * Sin(PI * 4 / 3), 2f),
                Color.Blue, 0.4f, 0.4f, 0.1f);

            SolidBackground bg = new SolidBackground();

            SDLighting lighting = new SDLighting(bg, Color.White, 0.001f, light1, light2, light3);

            SDScene scene = new SDScene(cam, lighting, obj, floor);

            Renderer renderer = new Renderer(scene);

            Directory.CreateDirectory("../../../Scenes/Scene3/anim/");

            string animDir = "../../../Scenes/Scene3/anim/";

            int frames = 60;
            for (int i = 0; i < frames; i++)
            {
                float angle = PI * 2f * i / frames;

                cam.Pos = new Vec3f(4f * Cos(angle), 4f * Sin(angle), 2f);
                cam.LookAt(new Vec3f());

                renderer.Render().Save(animDir + $"frame_{i + 1}.png");
            }
        }

        private static void Scene4()
        {
            RenderConfig.MaxMarches = 1000;
            RenderConfig.DistanceThreshold = 0.001f;
            RenderConfig.DerivativeDelta = 0.0001f;
            RenderConfig.MaxLightMarches = 3000;
            RenderConfig.MaxReflections = 3;
            RenderConfig.MaxRayDistance = 10000f;
            RenderConfig.Exposure = 1f;

            Material floorMat = new Material(Color.White, 0.01f, 0.7f, 100f, 0.2f);
            Material ballMat = new Material(Color.Blue, 1f, 0.3f, 50f, 1f);

            SDSphere ball1 = new SDSphere(new Vec3f(0f, -0.5f, 0f), 1f, ballMat);
            SDSphere ball2 = new SDSphere(new Vec3f(0f, 0.5f, 0f), 1f, ballMat);
            SDSphere ball3 = new SDSphere(new Vec3f(0f, 0f, 0f), 0.6f);

            SDObject obj = SDObject.Subtract(SDObject.Intersect(ball1, ball2), ball3);

            obj = SDObject.RepeatX(obj, 0f, 2f);
            obj = SDObject.RepeatY(obj, 0f, 2f);

            SDPlane floor = new SDPlane(new Vec3f(0f, 0f, -Sqrt(3) / 2f),
                new Vec3f(0f, 0f, 1f), floorMat);

            Camera cam = new Camera(1920, 1080);

            SunLight light1 = new SunLight(
                new Vec3f(5f * Cos(0), 5f * (float)Sin(0), -20f), 
                Color.Green, 0.01f, 0.01f, 0.1f);

            SunLight light2 = new SunLight(
                new Vec3f(5f * (float)Cos(PI * 2 / 3), 5f * (float)Sin(PI * 2 / 3), -20f), 
                Color.Red, 0.01f, 0.01f, 0.1f);

            SunLight light3 = new SunLight(
                new Vec3f(5f * (float)Cos(PI * 4 / 3), 5f * (float)Sin(PI * 4 / 3), -20f),
                Color.Blue, 0.01f, 0.01f, 0.1f);

            GradientBackground bg = new GradientBackground(
                Color.FromArgb(217, 34, 34),
                Color.FromArgb(176, 179, 36), 0.005f);

            SDLighting lighting = new SDLighting(bg, Color.White, 0.001f, light1, light2, light3);

            SDScene scene = new SDScene(cam, lighting, obj, floor);

            Renderer renderer = new Renderer(scene);

            Directory.CreateDirectory("../../../Scenes/Scene4/anim");

            string animPath = "../../../Scenes/Scene4/anim/";

            float fallStartY = -15f;
            float fallEndY = -1f;

            int frameCount = 0;

            int fallFrames = 90;
            for (int i = 0; i < fallFrames; i++)
            {
                float t = (float)i / (float)fallFrames;

                float x = 0f;
                float y = fallStartY + (fallEndY - fallStartY) * t;
                float z = Tools.CubicInterpolation(10f, 0f, t);

                cam.Pos = new Vec3f(x, y, z);
                cam.LookAt(new Vec3f(0f, fallEndY + 3f, 0f));

                renderer.Render().Save(animPath + $"frame_{++frameCount}.png");

                Console.WriteLine(string.Format("{0,3}/{1} frames complete", frameCount, 300));
            }

            cam.Pitch = 0f;
            cam.Yaw = 0f;

            int moveFrames = 30 * 3;

            float moveDist = (fallEndY - fallStartY) / fallFrames * moveFrames;

            for (int j = 0; j < moveFrames; j++)
            {
                float t = (float)j / (float)moveFrames;
                float x2 = 0f;
                float y2 = fallEndY + moveDist * t;
                float z2 = 0f;

                cam.Pos = new Vec3f(x2, y2, z2);

                renderer.Render().Save(animPath + $"frame_{++frameCount}.png");

                Console.WriteLine(string.Format("{0,3}/{1} frames complete", frameCount, 300));
            }
        }

        private static void Scene5()
        {
            RenderConfig.MaxMarches = 1000;
            RenderConfig.DistanceThreshold = 0.001f;
            RenderConfig.DerivativeDelta = 0.0001f;
            RenderConfig.MaxLightMarches = 3000;
            RenderConfig.MaxReflections = 3;
            RenderConfig.MaxRayDistance = 10000f;
            RenderConfig.Exposure = 1f;

            Material floorMat = new Material(Color.White, 0.01f, 0.7f, 100f, 0.2f);
            Material ballMat = new Material(Color.Blue, 1f, 0.3f, 50f, 1f);

            SDSphere ball1 = new SDSphere(new Vec3f(0f, -0.5f, 0f), 1f, ballMat);
            SDSphere ball2 = new SDSphere(new Vec3f(0f, 0.5f, 0f), 1f, ballMat);
            SDSphere ball3 = new SDSphere(new Vec3f(0f, 0f, 0f), 0.6f);

            SDObject obj = SDObject.Subtract(SDObject.Intersect(ball1, ball2), ball3);

            obj = SDObject.RepeatX(obj, 0f, 2f);
            obj = SDObject.RepeatY(obj, 0f, 2f);

            SDPlane floor = new SDPlane(new Vec3f(0f, 0f, (float)(0.0 - Sqrt(3)) / 2f),
                new Vec3f(0f, 0f, 1f), floorMat);

            SunLight light1 = new SunLight(
                new Vec3f(5f * (float)Cos(0), 5f * Sin(0), -20f),
                Color.Green, 0.01f, 0.01f, 0.1f);

            SunLight light2 = new SunLight(
                new Vec3f(5f * (float)Cos(PI * 2 / 3), 5f * Sin(PI * 2 / 3), -20f),
                Color.Red, 0.01f, 0.01f, 0.1f);

            SunLight light3 = new SunLight(
                new Vec3f(5f * (float)Cos(PI * 4 / 3), 5f * Sin(PI * 4 / 3), -20f),
                Color.Blue, 0.01f, 0.01f, 0.1f);

            GradientBackground bg = new GradientBackground(
                Color.FromArgb(217, 34, 34),
                Color.FromArgb(205, 30, 79), 0.005f);

            SDLighting lighting = new SDLighting(bg, Color.White, 0.001f, light1, light2, light3);

            Camera cam = new Camera(1920/2, 1080/2);

            SDScene scene = new SDScene(cam, lighting, obj, floor);

            Renderer renderer = new Renderer(scene);

            Directory.CreateDirectory("../../../Scenes/Scene5/");

            string imgPath = "../../../Scenes/Scene5/";

            cam.Pos = new Vec3f(3, 3, 3);
            cam.LookAt(new Vec3f(0, 0, 2));

            renderer.Render().Save(imgPath + "img.png");
        }
    }
}
