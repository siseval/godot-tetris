using Godot;
using Tetris.scripts.hud;
using static Tetris.scripts.logic.Piece;

namespace Tetris.scripts.logic;

public partial class Main : Node
{
	private Board _board;
	
	private Piece _current_piece;
	private Vector2I _position;
	
	private const int _START_X = 3;
	private const int _START_Y = 0;

	private double _clock_timer;
	private double _clock_ms;
	private bool _clock_running;

	private double _lock_timer;
	private double _lock_ms;
	private bool _lock_running;
	
	private const int _MAX_MOVES = 15;
	private int _moves_made;

	private bool _has_held;

	private readonly QueueHandler _queue_handler = new();
	
	private InputHandler _input_handler;	
	
	private Hud _hud;
	
	private int _score;
	private int _level;
	private int _lines;
	private double _time;

	public static readonly int[] _LINE_CLEAR_SCORES = { 100, 300, 500, 800 };
	private int _combo_count;
	private const int _COMBO_SCORE = 50;

	private const int _LINES_PER_LEVEL = 10;
	private int _lines_this_level;

	private static readonly double[] _CLOCK_TIMES =
		{ 480, 410, 365, 325, 285, 250, 215, 190, 175, 145, 125, 105, 90, 75, 60, 45, 35, 25, 12, 0 };

	private int _current_atlas_source_id;


	public override void _Ready()
	{
		getNodes();
		
		levelUp(20);
		updateLockTimerMs(_CLOCK_TIMES[0]);
		setCurrentPieceFromQueue();
		
		_board.updateScreen();
		
		startClock();
	}

	private void startClock()
	{
		_clock_running = true;
	}

	private void updateClockMs(double clock_ms)
	{
		_clock_ms = clock_ms;
		_clock_timer = 0;
	}

	private void updateLockTimerMs(double lock_ms)
	{
		_lock_ms = lock_ms;
		_lock_timer = 0;
	}

	private void getNodes()
	{
		_board = GetNode<Board>("Board");
		_input_handler = GetNode<InputHandler>("InputHandler");
		_hud = GetNode<Hud>("Hud");
	}

	public override void _PhysicsProcess(double delta)
	{
		_input_handler.handleInput(delta);

		_time += delta;
		_hud.updateLabels(_score, _level, _lines, _time);

		handleClockTimer(delta);
		handleLockTimer(delta);
		
		updatePosition();
	}

	private void handleClockTimer(double delta)
	{
		if (!_clock_running)
		{
			return;
		}
		_clock_timer += delta * 1000;
		if (_clock_timer >= _clock_ms)
		{
			_clock_timer = 0;
			doGameTick();
		}
	}

	private void handleLockTimer(double delta)
	{
		if (!_lock_running)
		{
			return;
		}
		_lock_timer += delta * 1000;
		if (_lock_timer >= _lock_ms)
		{
			_lock_timer = 0;
			_lock_running = false;
			tryLockPiece();
		}
	}

	private void fall()
	{
		move(1);
	}

	private void fallToBottom()
	{
		move(_board.getLowestHeight() - _position.Y);
	}

	private void slam()
	{
		fallToBottom();
		lockPiece();
		addScore(1, false);
	}

	private void move(int dy = 0, int dx = 0, bool check_collision = true)
	{
		updatePosition();
		if (!check_collision || !_board.collides(_current_piece._rotation, dy, dx))
		{
			_position.Y += dy;
			_position.X += dx;	
			incrementMoves();
		}
		updatePosition(); 
		updateScreen();
		
		tryStartLockTimer();
	}
	
	private void doMoveLeft()
	{
		move(0, -1);
	}
	private void doMoveRight()
	{
		move(0, 1);
	}
	private void rotateLeft()
	{
		updatePosition();
		if (!_board.collides(_current_piece.getPreviousRotation()))
		{
			_current_piece.rotateLeft();
			incrementMoves();
			updateScreen();
			return;
		}
		
		int[,] coordinates = _current_piece.getCollisionChecks(_current_piece._rotation, 0);
		for (int i = 0; i < 4; i++) 
		{
			if (_board.collides(_current_piece.getPreviousRotation(), coordinates[i, 0], coordinates[i, 1])) continue;
			move(coordinates[i, 0], coordinates[i, 1], false);
			_current_piece.rotateLeft();
			incrementMoves();
			updateScreen();
			return;
		}

		for (int i = 0; i < 2; i++)
		{
			if (_board.collides(_current_piece.getPreviousRotation(), i)) continue;
			move(i, 0, false);
			_current_piece.rotateLeft();
			incrementMoves();
			updateScreen();
			return;
		}
		tryStartLockTimer();
	}
	private void rotateRight()
	{
		updatePosition();
		if (!_board.collides(_current_piece.getNextRotation()))
		{
			_current_piece.rotateRight();
			incrementMoves();
			updateScreen();
			return;
		}

		int[,] coordinates = _current_piece.getCollisionChecks(_current_piece._rotation, 1);
		for (int i = 0; i < 4; i++)
		{
			if (_board.collides(_current_piece.getNextRotation(), coordinates[i, 0], coordinates[i, 1])) continue;
			move(coordinates[i, 0], coordinates[i, 1], false);	
			_current_piece.rotateRight();
			incrementMoves();
			updateScreen();
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			if (_board.collides(_current_piece.getPreviousRotation(), i)) continue;
			move(i, 0, false);
			_current_piece.rotateRight();
			incrementMoves();
			updateScreen();
			return;
		}
		tryStartLockTimer();
	}

	private void incrementMoves()
	{
		if (_lock_running)
		{
			_moves_made += 1;
		}
		if (!_board.collides(_current_piece._rotation, 1))
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
		_lock_timer = 0;
		_lock_running = true;
	}

	private void stopLockTimer()
	{
		_lock_running = false;
	}

	private void tryStartLockTimer()
	{
		if (_board.collides(_current_piece._rotation, 1) && !_lock_running)
		{
			startLockTimer();
		}
	}

	private void tryLockPiece()
	{
		if (_board.collides(_current_piece._rotation, 1))
		{
			lockPiece();
		}
	}
	private void lockPiece()
	{
		_moves_made = 0;
		if (_board.placeCurrentPiece())
		{
			_combo_count += 1;
			_score += _COMBO_SCORE * _combo_count;
		}
		else
		{
			_combo_count = 0;
		}
		setCurrentPieceFromQueue();
	}

	private void setCurrentPieceFromQueue()
	{
		setCurrentPiece(_queue_handler.pullFromQueue());
		_hud.updateQueueDisplay(_queue_handler.getQueueTypes(), _current_atlas_source_id);
		_has_held = false;
	}

	private void setCurrentPiece(Piece piece)
	{
		_current_piece = piece;
		stopLockTimer();
		resetPosition();
		updatePosition();
		_board.setCurrentPiece(_current_piece);
		_current_piece.setRotation(0);
		_clock_timer = 0;
		_lock_timer = 0;
	}
	private void doGameTick()
	{
		if (_board.collides(_current_piece._rotation, 1))
		{
			return;
		}

		if (_clock_ms > 0)
		{
			fall();
			return;
		}
		fallToBottom();	
	}

	private void updatePosition()
	{
		_board.updatePosition(_position);
	}
	private void updateScreen()
	{
		_board.updateScreen();
	}

	private void resetPosition()
	{
		int y = _current_piece.Type is PieceType.I or PieceType.O ? _START_Y - 1 : _START_Y;
		_position = new Vector2I(_START_X, y);
	}
	
	private void addScore(int score, bool level_mult)
	{
		_score += score * (level_mult ? _level : 1);
	}
	private void addLines(int lines)
	{
		_lines += lines;
		_lines_this_level += lines;

		if (_lines_this_level >= _LINES_PER_LEVEL * _level)
		{
			levelUp();
		}
	}

	private void levelUp(int levels = 1)
	{
		_level += levels;
		_lines_this_level = 0;
		if (_level <= 20)
		{
			updateClockMs(_CLOCK_TIMES[_level - 1]);
		}
		else
		{
			updateLockTimerMs(_CLOCK_TIMES[_level - 1]);	
		}
	}
}