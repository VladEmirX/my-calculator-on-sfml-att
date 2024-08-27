global using static MainLoop;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SFML.Window.Mouse;
static class MainLoop
{
	const uint offset = 80;

	public static readonly SFML.Graphics.RenderWindow win = new(
		new SFML.Window.VideoMode((uint)(offset * 4.5), (uint)(offset * 7.5)),
		"Супер-мега калькуль",
		SFML.Window.Styles.Close,
		new SFML.Window.ContextSettings()
		);

	static readonly LinkedList<IDrawable> ToDraw = [];
	private static void Main()
	{
		Init();
		Task.Run(DrawLoop);
		EventLoop();
	}
	private static void EventLoop()
	{
		
		while (win.IsOpen)
			win.WaitAndDispatchEvents();
	}

	private static void DrawLoop()
	{
		win.SetActive(true);
		while (win.IsOpen)
		{
			win.Clear(Color.Black);
			foreach (IDrawable drawed in ToDraw) win.Draw(drawed);
			win.Display();
		}
	}

	static UInt32 buffered = 0;
	static Func<uint, uint, uint> buffered_fn = static (_, n) => n;
	static bool pressed_op = true;

	static void Init()
	{

		win.SetActive(false);
		win.Closed += static (_, _) => win.Close();
		win.Resized += static (_, arg) => win.SetView(new View(win.GetView().Center, new(arg.Width, arg.Height)));


		const float size = offset * .75f;

		var button_template = new TextButton()
		{
			shape =
			{
				Size = new(size, size),
				Position = new(offset * 0.375f, offset * 0.375f),
				FillColor = Color.Cyan,
				OutlineColor = Color.Red,
				OutlineThickness = 3,
			},
			text =
			{
				DisplayedString = "0",
				CharacterSize = (uint)(size * 1.2f),
				Position = new(size * .20f + offset * 0.375f , offset * 0.12f),
				FillColor = Color.Red,
				Font = new("C:\\Windows\\Fonts\\UbuntuMono-B.ttf"),
			},
			enabled = true
		}.Add(false);

		TextButton display = button_template.Clone();
		display.shape.Position += new Vector2f(0, offset * 6);
		display.shape.Size = new Vector2f(3 * offset + size, size);
		display.text.Position += new Vector2f(0, offset * 6);
		display.text.DisplayedString = "00000000";

		for (uint i = 0; i < 16; ++i)
		{
			var button_n = button_template.Clone();
			button_n.text.DisplayedString = $"{i:X}";
			button_n.text.Position += new Vector2f(offset * (i % 4), offset * (i / 4));
			button_n.shape.Position += new Vector2f(offset * (i % 4), offset * (i / 4));
			var i_copy = i; //var capture works weird 
			button_n.Pressed += (_, _) => 
			{
				display.text.DisplayedString = (pressed_op ? "0000000" :display.text.DisplayedString[1..8]) + i_copy.ToString("X");
				pressed_op = false;
			};
		}

		foreach (var (text, offset_, fn) in (ReadOnlySpan<(char, Vector2f, Func<uint, uint, uint>?)>)
			[ 
				(
					'+',
					new(offset * 0, offset * 4),
					static (a, b) => a + b
				),
				(
					'-',
					new(offset * 1, offset * 4),
					static (a, b) => a - b
				),
				(
					'*',
					new(offset * 2, offset * 4),
					static (a, b) => a * b
				),
				(
					'=',
					new(offset * 3, offset * 4),
					null
				),
				( 
					'C',
					new(offset * 0, offset * 5),
					static (_, n) => n
				)
			])
		{
			var button_op = button_template.Clone();
			button_op.text.DisplayedString = text.ToString();
			button_op.text.Position += offset_;
			button_op.shape.Position += offset_;
			button_op.Pressed += (_, _) =>
			{
				UInt32.TryParse(
					display.text.DisplayedString,
					System.Globalization.NumberStyles.HexNumber,
					System.Globalization.NumberFormatInfo.InvariantInfo,
					out var n);
				var _buffered_fn = buffered_fn;
				buffered_fn = fn ?? _buffered_fn;
				buffered = pressed_op ? n : _buffered_fn(buffered, n);
				display.text.DisplayedString = text == 'C' ? "00000000" : $"{buffered:X8}";
				pressed_op = true;
			};
		}
		

		
	}

	public static TDrawable Add<TDrawable>(this TDrawable drawable, bool state = true) where TDrawable : IDrawable
	{
		if (state) ToDraw.AddLast(drawable); 
		else ToDraw.Remove(drawable);
		return drawable;
	}
	public static bool IsAdded(this IDrawable drawable)
	{
		return ToDraw.Contains(drawable);
	}
	
}
