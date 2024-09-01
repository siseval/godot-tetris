using System.Globalization;
using Godot;
using Tetris.scripts.logic;

namespace Tetris.scripts.debug;

public partial class DebugUi : Control
{
	private Main _main;
	public override void _Ready()
	{
		_main = GetParent<Main>();
	}

}