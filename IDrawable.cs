using SFML.Graphics;

interface IDrawable
{
	void Draw(RenderTarget target);
}

static class RenderTargetIDrawableExtension
{
	static public void Draw(this RenderTarget target, IDrawable obj) => obj.Draw(target);
}