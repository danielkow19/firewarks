using FireWARks.assets.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node2D
{
	[Export]
	public Player[] _players;

	private int _playersCount;

	private string[] scenePaths = { "res://StartMenu.tscn", "res://Game.tscn", "res://GameOver.tscn" };

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

		if (Input.IsKeyPressed(Key.Key1))
		{
			// Change to Start Screen
			GetTree().ChangeSceneToFile(scenePaths[0]);
		}

		if (Input.IsKeyPressed(Key.Key2))
		{
			// Change to Gameplay Scene
			GetTree().ChangeSceneToFile(scenePaths[1]);
		}

		if (Input.IsKeyPressed(Key.Key3))
		{
			// Change to GameOver Scene
			GetTree().ChangeSceneToFile(scenePaths[2]);
		}

		if (Input.IsKeyPressed(Key.P))
		{
			// Kill a player
			// Hard code for now, Want to make a menu that pops up show you can select a player number
			_players[0]._isDead = true;
		}
		if (Input.IsKeyPressed(Key.Slash))
		{
			//GetTree().ChangeSceneToFile("res://Pause.tscn");

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
			if (_players[i]._isDead) { deathCount++; }
		}

		if (deathCount >= _playersCount - 1) { return true; }
		else { return false; }
	}
}
