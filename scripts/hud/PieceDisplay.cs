using Godot;
using Tetris.scripts.logic;

namespace Tetris.scripts.hud;

public partial class PieceDisplay : TileMapLayer
{
	private Piece.PieceType _type;
	private int _atlas_source_id;
	
	private Vector2 _start_position;
	public void updateDisplay(Piece.PieceType type, int atlas_source_id)
	{
		Clear();
		
		_type = type;
		_atlas_source_id = atlas_source_id;
		
		if (type == Piece.PieceType.O)
		{
			Position = _start_position;
		}
		else if (type == Piece.PieceType.I)
		{
			Position = _start_position + new Vector2(0, 8);
		}
		else
		{
			Position = _start_position + new Vector2(8, 0) * Scale;
		}

		int[,] coordinates = PieceData.getCollisionCoordinates((int)type, 0);
		for (int i = 0; i < coordinates.GetLength(0); i++)
		{
			SetCell(new Vector2I(coordinates[i, 1], coordinates[i, 0]), atlas_source_id, new Vector2I((int)type + 1, 0));	
		}
	}
	
	public override void _Ready()
	{
		_start_position = Position;
	}

	public override void _Process(double delta)
	{
	}
}