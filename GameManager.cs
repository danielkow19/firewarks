using FireWARks.assets.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node2D
{
	[Export]
	public Player[] _players;

	private int _playersCount;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playersCount = _players.Length;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("Exit")) {
			GetTree().Quit();
		}

		if (PlayersDead())
		{
			GetTree().ChangeSceneToFile("res://GameOver.tscn");
		}
	}
	private bool PlayersDead()
	{
		int deathCount = 0;
		for(int i  = 0; i < _playersCount; i++)
		{
			if (_players[i].isDead) { deathCount++; }
		}

		if (deathCount >= _playersCount - 1) { return true; }
		else { return false; }
	}
}
