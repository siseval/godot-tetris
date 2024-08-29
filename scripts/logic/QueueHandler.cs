using System;
using System.Collections.Generic;
using static Tetris.scripts.logic.Piece;

namespace Tetris.scripts.logic;

public class QueueHandler
{
	
	private const int _QUEUE_LENGTH = 4;
	private readonly Piece[] _queue = new Piece[_QUEUE_LENGTH];
	private readonly List<PieceType> _bag = new();
	
	private readonly Random _random = new();

	public QueueHandler()
	{
		fillBag();
		fillQueue();
	}

	public Piece pullFromQueue()
	{
		Piece piece = _queue[0];
		for (int i = 0; i < _queue.Length - 1; i++)
		{
			_queue[i] = _queue[i + 1];
		}
		_queue[_QUEUE_LENGTH - 1] = null;
		fillQueue();
		return piece;
	}

	private void fillQueue()
	{
		for (int i = 0; i < _QUEUE_LENGTH; i++)
		{
			_queue[i] ??= new Piece(drawFromBag());
		}
	}

	private void fillLastInQueue()
	{
		_queue[^1] ??= new Piece(drawFromBag());
	}

	private PieceType drawFromBag()
	{
		int i = _random.Next(_bag.Count);
		PieceType type = _bag[i];
		_bag.RemoveAt(i);
		if (_bag.Count <= 0)
		{
			fillBag();
		}
		return type;
	}
	private void fillBag()
	{
		for (int i = 0; i < (int)PieceType.NUM_TYPES; i++)
		{
			_bag.Add((PieceType)i);
		}
	}
}