using Godot;

namespace Tetris.scripts.logic;

public partial class Board : TileMapLayer
{
	private int[,] _grid = new int[20, 10];
	private int _atlas_source_id;
	public override void _Ready()
	{
		_grid[0, 0] = 1;
		_grid[3, 0] = 2;
	}

	public override void _Process(double delta)
	{
		drawTiles();
	}

	private void drawTiles()
	{
		for (int i = 0; i < _grid.GetLength(0); i++)
		{
			for (int j = 0; j < _grid.GetLength(1); j++)
			{
				SetCell(new Vector2I(i, j), _atlas_source_id, new Vector2I(_grid[i, j], 0));
			}
		}
	}
}