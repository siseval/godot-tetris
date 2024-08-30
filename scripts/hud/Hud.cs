using System;
using Godot;
using Tetris.scripts.logic;

namespace Tetris.scripts.hud;

public partial class Hud : CanvasGroup
{
	private PieceDisplay _held_display;
	private PieceDisplay[] _queue_displays = new PieceDisplay[5];

	private Label _score_index_label;
	private Label _score_label;
	private Label _level_index_label;
	private Label _level_label;
	private Label _lines_index_label;
	private Label _lines_label;
	private Label _timer_index_label;
	private Label _timer_label;
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
		
		_score_index_label = GetNode<Label>("ScoreIndexLabel");	
		_score_label = GetNode<Label>("ScoreLabel");
		_level_index_label = GetNode<Label>("LevelIndexLabel");
		_level_label = GetNode<Label>("LevelLabel");
		_lines_index_label = GetNode<Label>("LinesIndexLabel");
		_lines_label = GetNode<Label>("LinesLabel");
		_timer_index_label = GetNode<Label>("TimerIndexLabel");
		_timer_label = GetNode<Label>("TimerLabel");
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

	public void updateLabels(int score, int level, int lines, double time)
	{
		updateScoreLabel(score);	
		updateLevelLabel(level);
		updateLinesLabel(lines);
		updateTimerLabel(time);
	}

	private void updateScoreLabel(int score)
	{
		_score_label.Text = score.ToString();	
	}
	private void updateLevelLabel(int level)
	{
		_level_label.Text = level.ToString();
	}
	private void updateLinesLabel(int lines)
	{
		_lines_label.Text = lines.ToString();
	}
	private void updateTimerLabel(double timer)
	{
		TimeSpan time_span = TimeSpan.FromSeconds(timer);
		_timer_label.Text = $"{time_span.Minutes:D2}:{time_span.Seconds:D2}";
	}

	public override void _Process(double delta)
	{
	}
}