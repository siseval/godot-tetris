using Godot;
using Tetris.scripts.hud;

namespace Tetris.scripts.logic;

public partial class Main : Node
{
	private Board _board;
	
	private Piece _current_piece;
	private Vector2I _position;
	
	private const int _START_X = 3;
	private const int _START_Y = -1;

	private Timer _clock;
	private const float _DEFAULT_CLOCK_MS = 500;

	private bool _collides_next;

	private Timer _lock_timer;
	private int _moves_made;
	private const float _DEFAULT_LOCK_MS = 500;
	private const int _MAX_MOVES = 15;

	private bool _has_held;

	private readonly QueueHandler _queue_handler = new();
	
	private InputHandler _input_handler;	
	
	private Hud _hud;

	private int _current_atlas_source_id;


	public override void _Ready()
	{
		getNodes();
		setupClock();	
		
		updateLockTimerMs(_DEFAULT_LOCK_MS);
		
		resetPosition();
		setCurrentPieceFromQueue();
		
		_board.setCurrentPiece(_current_piece);
		_board.update(_position);
		
		startClock();
	}

	private void startClock()
	{
		_clock.Start();
	}
	private void setupClock()
	{
		updateClockMs(_DEFAULT_CLOCK_MS);	
	}

	private void updateClockMs(float clock_ms)
	{
		_clock.WaitTime = clock_ms / 1000;
	}

	private void updateLockTimerMs(float lock_ms)
	{
		_lock_timer.WaitTime = lock_ms / 1000;	
	}

	private void getNodes()
	{
		_board = GetNode<Board>("Board");
		_clock = GetNode<Timer>("Clock");
		_lock_timer = GetNode<Timer>("LockTimer");
		_input_handler = GetNode<InputHandler>("InputHandler");
		_hud = GetNode<Hud>("Hud");
	}

	public override void _Process(double delta)
	{
		if (_input_handler.handleInput(delta))
		{
			_board.update(_position);
		}
	}

	private void fall()
	{
		doUpdate();
		if (!_collides_next)
		{
			_position.Y += 1;
			_moves_made = 0;
		}
		else
		{
			tryStartLockTimer();
		}
	}

	private void doMoveRight()
	{
		moveRight();
	}
	private void moveRight(int dx = 1, bool check_collision = true)
	{
		doUpdate();
		if (!_board.collidesRight(_current_piece._rotation, dx) || !check_collision)
		{
			_position.X += dx;	
			incrementMoves();
		}
		tryStartLockTimer();
	}
	
	private void doMoveLeft()
	{
		moveLeft();
	}
	private void moveLeft(int dx = 1, bool check_collision = true)
	{
		doUpdate();
		if (!_board.collidesLeft(_current_piece._rotation, -dx) || !check_collision)
		{
			_position.X -= dx;
			incrementMoves();
		}
		tryStartLockTimer();
	}

	private void rotateLeft()
	{
		doUpdate();
		if (!_board.collidesLeft(_current_piece.getPreviousRotation()) &&
		    !_board.collidesRight(_current_piece.getPreviousRotation()))
		{
			_current_piece.rotateLeft();
			incrementMoves();
			return;
		}

		for (int i = 0; i < 3; i++)
		{
			if (!_board.collidesLeft(_current_piece.getPreviousRotation(), i) &&
				!_board.collidesRight(_current_piece.getPreviousRotation(), i))
			{
				moveRight(i);
				_current_piece.rotateLeft();
				incrementMoves();
				return;
			}
			if (!_board.collidesLeft(_current_piece.getPreviousRotation(), -i) &&
				!_board.collidesRight(_current_piece.getPreviousRotation(), -i))
			{
				moveLeft(i);
				_current_piece.rotateLeft();
				incrementMoves();
				return;
			}
		}
		tryStartLockTimer();
	}
	private void rotateRight()
	{
		doUpdate();
		if (!_board.collidesRight(_current_piece.getNextRotation()) &&
		    !_board.collidesLeft(_current_piece.getNextRotation()))
		{
			_current_piece.rotateRight();
			incrementMoves();
			return;
		}

		for (int i = 0; i < 3; i++)
		{
			if (!_board.collidesLeft(_current_piece.getNextRotation(), -i) &&
				!_board.collidesRight(_current_piece.getNextRotation(), -i))
			{
				moveLeft(i);	
				_current_piece.rotateRight();
				incrementMoves();
				return;
			}
			if (!_board.collidesLeft(_current_piece.getNextRotation(), i) &&
					 !_board.collidesRight(_current_piece.getNextRotation(), i))
			{
				moveRight(i);
				_current_piece.rotateRight();
				incrementMoves();
				return;
			}
		}
		tryStartLockTimer();
	}

	private void incrementMoves()
	{
		_moves_made += 1;
		if (!_collides_next)
		{
			return;
		}
		if (_moves_made >= _MAX_MOVES)
		{
			tryLockPiece();
		}
		else
		{
			startLockTimer();
		}
	}

	private void holdPiece()
	{
		if (_has_held)
		{
			return;
		}

		_hud.updateHeldDisplay(_current_piece.Type, _current_atlas_source_id);
		Piece piece = _queue_handler.holdPiece(_current_piece);
		if (piece != null)
		{
			setCurrentPiece(piece);	
		}
		else
		{
			setCurrentPieceFromQueue();
		}
		
		_has_held = true; 
	}

	private void startLockTimer()
	{
		_lock_timer.Stop();
		_lock_timer.Start();
	}

	private void tryStartLockTimer()
	{
		if (_collides_next && _lock_timer.TimeLeft == 0)
		{
			startLockTimer();
		}
	}

	private void tryLockPiece()
	{
		updateCollidesNext();
		if (_collides_next)
		{
			lockPiece();
		}
	}
	private void lockPiece()
	{
		_board.placeCurrentPiece();
		setCurrentPieceFromQueue();
		_collides_next = false;
	}

	private void setCurrentPieceFromQueue()
	{
		setCurrentPiece(_queue_handler.pullFromQueue());
		_has_held = false;
	}

	private void setCurrentPiece(Piece piece)
	{
		_current_piece = piece;
		_lock_timer.Stop();
		resetPosition();
		_board.setCurrentPiece(_current_piece);
	}
	private void doGameTick()
	{
		if (_collides_next)
		{
			return;
		}
		fall();
	}

	private void doUpdate()
	{
		_board.update(_position);
		updateCollidesNext();
	}

	private bool updateCollidesNext()
	{
		_collides_next = _board.collidesOnNext(_current_piece._rotation);
		return _collides_next;
	}

	private void resetPosition()
	{
		_position = new Vector2I(_START_X, _START_Y);
	}
	
	private void onClockTimeout()
	{
		doGameTick();
	}

	private void onLockTimerTimeout()
	{
		tryLockPiece();
	}


	public double getLockTime()
	{
		return _lock_timer.TimeLeft;
	}
}