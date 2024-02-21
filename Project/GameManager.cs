using FireWARks.assets.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class GameManager : Node2D
{
	[Export]
	public Player[] _players;

	private int _playersCount;

	private string[] scenePaths = { "res://StartMenu.tscn", "res://Game.tscn", "res://GameOver.tscn" };
	private Control _pauseMenu;
	private Button _resumeButton;
	private bool _paused;

	[Export]
	public string currentScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playersCount = _players.Length;
		_pauseMenu = GetNode<Control>("%PauseMenu");
		_resumeButton = GetNode<Button>("PauseMenu/MarginContainer/VBoxContainer/Resume");
		_paused = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("Exit")) {
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("Pause"))
		{
			if(currentScene == scenePaths[1]) { PauseMenu(); }
		}

		if (Input.IsKeyPressed(Key.Key8))
		{
			// Change to Start Screen
			GetTree().ChangeSceneToFile(scenePaths[0]);
		}

		if (Input.IsKeyPressed(Key.Key9))
		{
			// Change to Gameplay Scene
			GetTree().ChangeSceneToFile(scenePaths[1]);
		}

		if (Input.IsKeyPressed(Key.Key0))
		{
			// Change to GameOver Scene
			GetTree().ChangeSceneToFile(scenePaths[2]);
		}

		if (Input.IsKeyPressed(Key.P))
		{
			// Empty Player protection
			if(_players.Length <= 0)
			{
				return;
			}
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
		if (_playersCount <= 0)
		{
			return false;
		}

		int deathCount = 0;
		for(int i  = 0; i < _playersCount; i++)
		{
			if (_players[i]._isDead) { deathCount++; }
		}

		if (deathCount >= _playersCount - 1) { return true; }
		else { return false; }
	}
	public void PauseMenu()
	{
		if (!_paused)
		{
			_pauseMenu.Show();
			_resumeButton.GrabFocus();
			GetTree().Paused = true;
		}
		else
		{
            GetTree().Paused = false;
            _pauseMenu.Hide();	
		}
		if (_playersCount > 0)
		{
			foreach (var player in _players)
			{
				player.ToggleHUD();
			}
		}

		Debug.Print($"{_paused}");
		_paused = !_paused;
	}
}
