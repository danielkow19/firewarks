using FireWARks.assets.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

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
		player_settings settings = (player_settings)GetNode("/root/PlayerSettings");
		if(currentScene == scenePaths[1])
		{
			// Handle player spawning
			LoadPlayers(settings);
		}
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
		if (_players.Count == 0)
		{
			return false;
		}
		// Count the number of deaths
		int deathCount = 0;
		if (_players.Count > 1)
		{
			for (int i = 0; i < _players.Count; i++)
			{
				if (_players[i]._isDead) { deathCount++; }
			}
			if (deathCount >= _players.Count - 1) { return true; }
			else { return false; }
		}
		// Single player death logic
		else if(_players.Count == 1)
		{
			return _players[0]._isDead;
		}
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
		Player instance = (Player)_playerPrefab.Instantiate();
		// Set positions here
		instance.GlobalPosition = new Vector2(x, y);
		instance.Set("player_id", playerID);
		instance.Set("patternLeft", patternLeft);
		instance.Set("patternRight", patternRight);
		_players.Add(instance);
		this.AddChild(instance);
		//_hasSpawned = true;
	}
	private void LoadPlayers(player_settings settings)
	{
		GD.Print("Load Players Called");
		for(int i =0; i < settings.PlayerInfos.Count; i++)
		{
			GD.Print($"Loading player {i}");
			SpawnPlayer(settings.PlayerInfos[i].PlayerID,
				settings.PlayerInfos[i].LeftPattern,
				settings.PlayerInfos[i].RightPattern,
				settings.PlayerInfos[i].X,
				settings.PlayerInfos[i].Y);
		}
	}
}
