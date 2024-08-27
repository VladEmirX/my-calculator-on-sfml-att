using SFML.Graphics;

class TextButton: RectangleButton
{
	public readonly Text text = new();
	public RenderStates textRender = RenderStates.Default;

	public TextButton() { }
	protected TextButton(TextButton clone) : base(clone)
	{
		textRender = new(clone.textRender);
		text = new(clone.text);
	}
	public override TextButton Clone() => new TextButton(this);

	public override void Draw(RenderTarget target)
	{
		base.Draw(target);
		if (enabled) text.Draw(target, textRender);
	}
}
