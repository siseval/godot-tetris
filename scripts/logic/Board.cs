using System.Collections.Generic;
using Godot;

namespace Tetris.scripts.logic;

public partial class Board : TileMapLayer
{
	private Piece _current_piece;
	private Vector2I _position;


	private const int _HEIGHT = 20;
	private const int _WIDTH = 10;
	
	private readonly int[,] _grid = new int[_HEIGHT, _WIDTH];

	private int AtlasSourceId { get; set; }
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void setCurrentPiece(Piece piece)
	{
		_current_piece = piece;
	}

	public void placeCurrentPiece()
	{
		for (int i = 0; i < _current_piece.CollisionCoordinates.GetLength(0); i++)
		{
			int[] coordinates = { _position.Y + _current_piece.CollisionCoordinates[_current_piece._rotation, i, 0], _position.X + _current_piece.CollisionCoordinates[_current_piece._rotation, i, 1] };
			if (coordinates[0] < 0)
			{
				continue;
			}
			_grid[coordinates[0], coordinates[1]] = (int)_current_piece.Type + 1;
		}
		checkLines();
	}

	public void update(Vector2I position)
	{
		_position = position;
		drawTiles();
	}

	public bool collidesOnNext(int rotation)
	{
		for (int i = 0; i < _current_piece.CollisionCoordinates.GetLength(0); i++)
		{
			int[] coordinates =
			{
				_position.Y + _current_piece.CollisionCoordinates[rotation, i, 0],
				_position.X + _current_piece.CollisionCoordinates[rotation, i, 1]
			};
			if (coordinates[0] + 1 >= _HEIGHT || _grid[coordinates[0] + 1, coordinates[1]] != 0)
			{
				return true;
			}
		}
		
		return false;
	}

	public bool collidesLeft(int rotation, int dx = 0)
	{
		for (int i = 0; i < _current_piece.CollisionCoordinates.GetLength(1); i++)
		{
			int[] coordinates =
			{
				_position.Y + _current_piece.CollisionCoordinates[rotation, i, 0],
				_position.X + _current_piece.CollisionCoordinates[rotation, i, 1]
			};
			if (coordinates[0] < 0 || coordinates[0] > _HEIGHT - 1 || coordinates[1] + dx > _WIDTH - 1)
			{
				continue;
			}
			if (coordinates[1] + dx < 0 || _grid[coordinates[0], coordinates[1] + dx] != 0)
			{
				return true;
			}
		}

		return false;
	}
	public bool collidesRight(int rotation, int dx = 0)
	{
		for (int i = 0; i < _current_piece.CollisionCoordinates.GetLength(1); i++)
		{
			int[] coordinates =
			{
				_position.Y + _current_piece.CollisionCoordinates[rotation, i, 0],
				_position.X + _current_piece.CollisionCoordinates[rotation, i, 1]
			};
			if (coordinates[0] < 0 || coordinates[0] > _HEIGHT - 1 || coordinates[1] + dx < 0)
			{
				continue;
			}
			if (coordinates[1] + dx > _WIDTH - 1 || _grid[coordinates[0], coordinates[1] + dx] != 0)
			{
				return true;
			}
		}

		return false;
	}

	private void checkLines()
	{
		var heights = new List<int>();
		for (int i = 0; i < _HEIGHT; i++)
		{
			bool broken = false;
			for (int j = 0; j < _WIDTH; j++)
			{
				if (_grid[i, j] == 0)
				{
					broken = true;
					break;
				}
			}
			if (broken)
			{
				continue;
			}
			heights.Add(i);
		}
		clearLines(heights);
	}

	private void clearLines(List<int> heights)
	{
		foreach (int height in heights)
		{
			clearLine(height);	
		}
	}

	private void clearLine(int height)
	{
		for (int i = height; i > 1; i--)
		{
			for (int j = 0; j < _WIDTH; j++)
			{
				_grid[i, j] = _grid[i - 1, j];	
			}
		}
	}
	
	private void drawTiles()
	{
		for (int i = 0; i < _grid.GetLength(0); i++)
		{
			for (int j = 0; j < _grid.GetLength(1); j++)
			{
				SetCell(new Vector2I(j, i), AtlasSourceId, new Vector2I(_grid[i, j], 0));
			}
		}
		for (int i = 0; i < _current_piece.Matrix.GetLength(0); i++)
		{
			for (int j = 0; j < _current_piece.Matrix.GetLength(1); j++)
			{
				SetCell(new Vector2I(j + _position.X, i + _position.Y), AtlasSourceId, new Vector2I(_current_piece.Matrix[i, j] == 0 ? _grid[Mathf.Clamp(i + _position.Y, 0, _HEIGHT - 1), Mathf.Clamp(j + _position.X, 0, _WIDTH - 1)] : (int)_current_piece.Type + 1, 0));
			}
		}
	}
}