using SFML.Graphics;

class RectangleButton : ButtonBase, IDrawable
{
	readonly public RectangleShape shape = new RectangleShape();
	public RenderStates shapeRender = RenderStates.Default;
	public sealed override FloatRect hitbox => shape.GetGlobalBounds();

	public RectangleButton() { this.Add(); }
	protected RectangleButton(RectangleButton clone) : base(clone)
	{
		this.Add();
		shape = new(clone.shape);
		shapeRender = new RenderStates(clone.shapeRender);
	}

	override public RectangleButton Clone() => new(this);
	virtual public void Draw(RenderTarget target) { if (enabled) shape?.Draw(target, shapeRender); }
}