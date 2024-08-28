using System.Globalization;
using Godot;
using Tetris.scripts.logic;

namespace Tetris.scripts.debug;

public partial class DebugUi : Control
{
	private Main _main;
	Label _lock_time_label;
	public override void _Ready()
	{
		_main = GetParent<Main>();
		_lock_time_label = GetNode<Label>("LockTimeLabel");	
	}

	public override void _Process(double delta)
	{
		_lock_time_label.Text = "LOCK TIME: " + _main.getLockTime().ToString(CultureInfo.InvariantCulture);
	}
}