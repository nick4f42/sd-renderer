using System.Drawing;

using SDRenderer;
using SDRenderer.Lighting;
using SDRenderer.SDObjects;
using SDRenderer.Util;

namespace ExampleScene
{
    class Program
    {
        const string ImgPath = "../../../";

        static void Main(string[] args)
        {

            ILight light = new PointLight(new Vec3f(4, -4, 2), // Light position
                                          Color.White, // Light color
                                          0.3f, // Diffuse intensity
                                          0.3f, // Specular intensity
                                          0.2f); // Softness

            IBackground bg = new SolidBackground();

            SDLighting lighting = new SDLighting(bg, // Lighting background
                                                 Color.White, // Ambient color
                                                 0.0001f, // Ambient color brightness
                                                 light); // Lights

            Material ballMat = new Material(Color.Blue,
                                            0.3f, // Diffuse constant
                                            0.9f, // Specular constant
                                            80f, // Shininess
                                            1f); // Ambient constant

            Material floorMat = new Material(Color.White,
                                             0.1f, // Diffuse constant
                                             0.05f, // Specular constant
                                             30f, // Shininess
                                             0.4f); // Ambient constant

            SDSphere ball = new SDSphere(new Vec3f(), // Postion
                                         1f, // Radius
                                         ballMat); // Material

            // Repeats ball infinitely in the X direction
            var ballField = SDObject.RepeatX(ball, 0f, 5f);
            // Repeats ball infinitely in the Y direction
            ballField = SDObject.RepeatY(ballField, 0f, 5f);

            SDObject floor = new SDDeepPlane(new Vec3f(0, 0, -1), // Position
                                             new Vec3f(0, 0, 1), // Outward plane normal
                                             floorMat); // Material

            Camera cam = new Camera(1920, 1080);

            cam.Pos = new Vec3f(0, -5, 2);
            cam.LookAt(new Vec3f());

            SDScene scene = new SDScene(cam, lighting, ballField, floor);

            Renderer renderer = new Renderer(scene);

            renderer.Render().Save(ImgPath + "example_img.png");
        }
    }
}
