using Godot;
using Tetris.scripts.logic;

namespace Tetris.scripts.hud;

public partial class Hud : CanvasGroup
{
	private PieceDisplay _held_display;
	public override void _Ready()
	{
		getNodes();
	}

	private void getNodes()
	{
		_held_display = GetNode<PieceDisplay>("HeldDisplay");
	}

	public void updateHeldDisplay(Piece.PieceType type, int atlas_source_id)
	{
		_held_display.updateDisplay(type, atlas_source_id);
	}

	public override void _Process(double delta)
	{
	}
}