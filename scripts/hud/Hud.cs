using Godot;
using Tetris.scripts.logic;

namespace Tetris.scripts.hud;

public partial class Hud : CanvasGroup
{
	private PieceDisplay _held_display;
	private PieceDisplay[] _queue_displays = new PieceDisplay[5];
	public override void _Ready()
	{
		getNodes();
	}

	private void getNodes()
	{
		_held_display = GetNode<PieceDisplay>("HeldDisplay");
		for (int i = 0; i < _queue_displays.Length; i++)
		{
			_queue_displays[i] = GetNode<PieceDisplay>("QueueDisplay" + (i + 1));
		}
	}

	public void updateHeldDisplay(Piece.PieceType type, int atlas_source_id)
	{
		_held_display.updateDisplay(type, atlas_source_id);
	}

	public void updateQueueDisplay(Piece.PieceType[] types, int atlas_source_id)
	{
		for (int i = 0; i < _queue_displays.Length; i++)
		{
			_queue_displays[i].updateDisplay(types[i], atlas_source_id);
		}
	}

	public override void _Process(double delta)
	{
	}
}