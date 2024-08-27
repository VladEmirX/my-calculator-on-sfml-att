using SFML.Graphics;
using SFML.Window;

abstract class ButtonBase : ICloneable
{
	public bool enabled = false;
	abstract public FloatRect hitbox { get; } 
	public event EventHandler<MouseButtonEventArgs> Pressed = (_, _) => { };
	//public EventHandler<MouseButtonEventArgs>[] pressed { set => Pressed = Delegate.Combine(value) as EventHandler<MouseButtonEventArgs> ?? ((_, _) => { }); }

	protected ButtonBase() 
	{
		win.MouseButtonPressed += (sender, args) =>
		{
			if (enabled && hitbox.Contains(args.X, args.Y) == true)
				Pressed(this, args);
		};
	}
	protected ButtonBase(ButtonBase clone) : this() { Pressed = clone.Pressed; enabled = clone.enabled; }
	abstract public ButtonBase Clone();
	object ICloneable.Clone() => Clone();
}
