using System;
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
	private TileMapLayer _ghost;
	private int AtlasSourceId { get; set; }
	
	[Signal]
	public delegate void AddScoreEventHandler(int score, bool level_mult);
	[Signal]
	public delegate void AddLinesEventHandler(int lines);

	public override void _Ready()
	{
		getNodes();
	}

	private void getNodes()
	{
		_ghost = GetNode<TileMapLayer>("Ghost");	
	}

	public override void _Process(double delta)
	{
	}

	public void setCurrentPiece(Piece piece)
	{
		_current_piece = piece;
	}

	public bool placeCurrentPiece()
	{
		for (int i = 0; i < _current_piece.CollisionCoordinates.GetLength(0); i++)
		{
			int[] coordinates =
			{
				_position.Y + _current_piece.CollisionCoordinates[_current_piece._rotation, i, 0],
				_position.X + _current_piece.CollisionCoordinates[_current_piece._rotation, i, 1]
			};
			if (coordinates[0] < 0)
			{
				continue;
			}
			_grid[coordinates[0], coordinates[1]] = (int)_current_piece.Type + 1;
		}

		EmitSignal(SignalName.AddScore, 1, false);	
		return checkLines();
	}

	public void updatePosition(Vector2I position)
	{
		_position = position;
	}
	public void updateScreen()
	{
		drawTiles();
		drawGhost();
	}

	public int getLowestHeight()
	{
		int height = 0;

		while (!collides(_current_piece._rotation, height + 1))
		{
			height += 1;
		}

		return _position.Y + height;
	}

	public bool collides(int rotation, int dy = 0, int dx = 0)
	{
		for (int i = 0; i < _current_piece.CollisionCoordinates.GetLength(0); i++)
		{
			int[] coordinates =
			{
				_position.Y + _current_piece.CollisionCoordinates[rotation, i, 0] + dy,
				_position.X + _current_piece.CollisionCoordinates[rotation, i, 1] + dx
			};
			if (coordinates[0] < 0 || coordinates[1] < 0 || coordinates[0] >= _HEIGHT || coordinates[1] >= _WIDTH)
			{
				return true;
			}
			if (_grid[coordinates[0], coordinates[1]] != 0)
			{
				return true;
			}
		}
		
		return false;
	}
	private bool checkLines()
	{
		var heights = new List<int>();
		for (int i = 0; i < _HEIGHT; i++)
		{
			bool broken = false;
			for (int j = 0; j < _WIDTH; j++)
			{
				if (_grid[i, j] != 0) continue;
				broken = true;
				break;
			}
			if (broken)
			{
				continue;
			}
			heights.Add(i);
		}

		if (heights.Count <= 0) return false;
		clearLines(heights);
		return true;
	}

	private void clearLines(List<int> heights)
	{
		foreach (int height in heights)
		{
			clearLine(height);	
		}
		EmitSignal(SignalName.AddLines, heights.Count);
		EmitSignal(SignalName.AddScore, Main._LINE_CLEAR_SCORES[heights.Count - 1], true);
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
		for (int i = 0; i < _current_piece.CollisionCoordinates.GetLength(0); i++)
		{
			int[] coordinates =
			{
				_position.Y + _current_piece.CollisionCoordinates[_current_piece._rotation, i, 0],
				_position.X + _current_piece.CollisionCoordinates[_current_piece._rotation, i, 1]
			};

			SetCell(new Vector2I(coordinates[1], coordinates[0]), AtlasSourceId, new Vector2I((int)_current_piece.Type + 1, 0));
		}
	}

	private void drawGhost()
	{
		_ghost.Clear();
		for (int i = 0; i < _current_piece.CollisionCoordinates.GetLength(0); i++)
		{
			int[] coordinates =
			{
				getLowestHeight() + _current_piece.CollisionCoordinates[_current_piece._rotation, i, 0],
				_position.X + _current_piece.CollisionCoordinates[_current_piece._rotation, i, 1]
			};

			_ghost.SetCell(new Vector2I(coordinates[1], coordinates[0]), AtlasSourceId, new Vector2I((int)_current_piece.Type + 1, 0));
		}
	}
}