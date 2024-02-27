using FireWARks.assets.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class GameManager : Node2D
{
	[Export]
	public Godot.Collections.Array<Player> _players;

	private string[] scenePaths = { "res://StartMenu.tscn", "res://Game.tscn", "res://GameOver.tscn" };
	private Control _pauseMenu;
	private Button _resumeButton;
	private bool _paused;
	//private bool _hasSpawned;

	[Export]
	public string currentScene;

	// Player Scene
	PackedScene _playerPrefab = GD.Load<PackedScene>("res://assets/prefabs/Player.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pauseMenu = GetNode<Control>("%PauseMenu");
		_resumeButton = GetNode<Button>("PauseMenu/MarginContainer/VBoxContainer/Resume");
		_paused = false;
		//_hasSpawned = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("Exit")) {
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("Pause"))
		{
			if(currentScene != scenePaths[0] && currentScene != scenePaths[2]) { PauseMenu(); }
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
			if(_players.Count <= 0)
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
		//if(!_hasSpawned){
		//	return false;	
		//}
		if (_players.Count <= 0)
		{
			return false;
		}

		int deathCount = 0;
		for(int i  = 0; i < _players.Count; i++)
		{
			if (_players[i]._isDead) { deathCount++; }
		}

		if (deathCount > _players.Count - 1) { return true; }
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
		if (_players.Count > 0)
		{
			foreach (Player player in _players)
			{
				player.ToggleHUD();
			}
		}

		Debug.Print($"{_paused}");
		_paused = !_paused;
	}
	public void SpawnPlayer(int playerID, PackedScene patternLeft, PackedScene patternRight, float x, float y)
	{
		var instance = _playerPrefab.Instantiate();
		// Set positions here
		instance.Set("player_id", playerID);
		instance.Set("patternLeft", patternLeft);
		instance.Set("patternRight", patternRight);
		if(instance is Player)
		{
			_players.Add((Player)instance);
		}
		//_hasSpawned = true;
	}
}
