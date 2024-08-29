using System.Collections.Generic;
using Godot;

namespace Tetris.scripts.logic;

public partial class InputHandler : Node
{
	private float ARR { get; set; } = 33.0f;
	private float DAS { get; set; } = 100.0f;
	
	private const int _NUM_TIMERS = 3;
	private static readonly Dictionary<string, int> _ACTIONS = new() { { "left", 0 }, { "right", 1 }, { "down", 2 } };

	private double[] _arr_timer = new double[_NUM_TIMERS];
	private double[] _das_timer = new double[_NUM_TIMERS];
	private readonly bool[] _arr_running = new bool[_NUM_TIMERS];
	private readonly bool[] _das_running = new bool[_NUM_TIMERS];
	
	private string _last_action;
	
	[Signal]
	public delegate void MoveLeftEventHandler();
	[Signal]
	public delegate void MoveRightEventHandler();
	[Signal]
	public delegate void MoveDownEventHandler();
	[Signal]
	public delegate void RotateLeftEventHandler();
	[Signal]
	public delegate void RotateRightEventHandler();
	[Signal]
	public delegate void HoldEventHandler();
	private bool checkMoveInput(string action, double dt)
	{
		double ms = dt * 1000.0f;
		
		int index = _ACTIONS[action];
		
		if (Input.IsActionJustPressed(action))
		{
			_arr_timer[index] = 0.0f;
			_das_timer[index] = 0.0f;
			_das_running[index] = true;	
			return true;
		}

		if (!Input.IsActionPressed(action))
		{
			_arr_running[index] = false;
			_das_running[index] = false;	
			return false;
		}

		if (_das_running[index])
		{
			_das_timer[index] += ms;
			if (_das_timer[index] >= DAS)
			{
				_das_running[index] = false;
				_arr_running[index] = true;
				return true;
			}
			return false;
		}

		if (_arr_running[index])
		{
			_arr_timer[index] += ms;
			if (_arr_timer[index] >= ARR)
			{
				_arr_timer[index] = 0.0f;
				return true;
			}
		}
		
		return false;
	}

	public bool handleInput(double dt)
	{
		bool did_input = false;
		if (checkMoveInput("left", dt))
		{
			EmitSignal(SignalName.MoveLeft);
			did_input = true;	
		}
		if (checkMoveInput("right", dt))
		{
			EmitSignal(SignalName.MoveRight);
			did_input = true;
		}
		if (checkMoveInput("down", dt))
		{
			EmitSignal(SignalName.MoveDown);
			did_input = true;
		}
		if (Input.IsActionJustPressed("rotate_left"))
		{
			EmitSignal(SignalName.RotateLeft);
			did_input = true;
		}
		if (Input.IsActionJustPressed("rotate_right"))
		{
			EmitSignal(SignalName.RotateRight);
			did_input = true;
		}

		if (Input.IsActionJustPressed("hold"))
		{
			EmitSignal(SignalName.Hold);
			did_input = true;
		}
		return did_input;
	}
}