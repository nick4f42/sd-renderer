using SDRenderer.Util;

namespace SDRenderer.Lighting
{
	public interface IBackground
	{
		DirectionalColorData BackgroundColorData(Vec3f direction);
	}
}
