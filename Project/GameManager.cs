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
	PackedScene pattern = GD.Load<PackedScene>("res://assets/prefabs/PatternCircleBurst.tscn");
	//private bool _hasSpawned;

	public string currentScene;

	// Player Scene
	PackedScene _playerPrefab = GD.Load<PackedScene>("res://assets/prefabs/Player.tscn");
	

	// Map References
	private PackedScene blank = GD.Load<PackedScene>("res://assets/maps/map_blank.tscn");
	private PackedScene circle = GD.Load<PackedScene>("res://assets/maps/map_circle.tscn");
	private PackedScene compactor = GD.Load<PackedScene>("res://assets/maps/map_compactor.tscn");
	private PackedScene movingBoxes = GD.Load<PackedScene>("res://assets/maps/map_moving_boxes.tscn");
	private PackedScene spinningBar = GD.Load<PackedScene>("res://assets/maps/map_spin_bar.tscn");
	
	private WorldBorder worldBorder;

	public WorldBorder WorldBorder
	{
		get { return worldBorder; }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hook up menu logic
		_pauseMenu = GetNode<Control>("%PauseMenu");
		_resumeButton = GetNode<Button>("PauseMenu/MarginContainer/VBoxContainer/Resume");
		_paused = false;

		// Set current Scene
		worldBorder = GetNode<WorldBorder>("CloudBorder");
		player_settings settings = (player_settings)GetNode("/root/PlayerSettings");
		settings.ResetDeaths();
		// Handle player spawning
		LoadPlayers(settings);
		// Handle Map Loading
		LoadMap(settings.MapName);

		AudioStreamPlayer music = GetNode<AudioStreamPlayer>("/root/SoundManager/Music");
		music.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		if (Input.IsActionPressed("Exit")) 
		{
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("Pause"))
		{
			if(currentScene != scenePaths[0] && currentScene != scenePaths[2]) 
			{ 
				PauseMenu(); 
			}
		}

		if (Input.IsKeyPressed(Key.Key8))
		{
			// Change to Start Screen
			SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
			sceneManager.GoToScene(scenePaths[0]);
		}

		if (Input.IsKeyPressed(Key.Key9))
		{
			// Change to Gameplay Scenevvvv
			SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
			sceneManager.GoToScene(scenePaths[1]);
		}

		if (Input.IsKeyPressed(Key.V))
		{
			Pattern instance = pattern.Instantiate<Pattern>();
			
			instance.Set("position", GetGlobalMousePosition());
			instance.Set("rotation", Vector3.Zero);
			instance.Set("owner", GetNode("Player_1"));
			AddSibling(instance);
		}

		if (Input.IsKeyPressed(Key.Key0))
		{
			// Change to GameOver Scene
			SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
			sceneManager.GoToScene(scenePaths[2]);
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
			player_settings settings = (player_settings)GetNode("/root/PlayerSettings");
			for(int i = 0; i < _players.Count; i++) {
				if(!_players[i]._isDead) settings.playerDeath(_players[i].player_id);
			}
			SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
			sceneManager.GoToScene("res://GameOver.tscn");
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
				//WorldBorder worldBorder = GetNode<WorldBorder>("/root/SceneManager/Game2/CloudBorder");
				if(worldBorder.timeSeconds > 0)
				{
					worldBorder.timeSeconds -= 30;
				}				
			}
			if (deathCount >= _players.Count - 1) 
			{ 
				return true; 
			}
			else 
			{ 
				return false; 
			}
		}
		// Single player death logic
		else if(_players.Count == 1)
		{
			return _players[0]._isDead;
		}
		else 
		{ 
			return false; 
		}
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
		_paused = !_paused;
	}
	public void SpawnPlayer(int playerID, PackedScene patternLeft, PackedScene patternRight, Color color, float x, float y)
	{
		Player instance = (Player)_playerPrefab.Instantiate();
		// Set positions here
		instance.GlobalPosition = new Vector2(x, y);
		instance.Set("player_id", playerID);
		instance.Set("patternLeft", patternLeft);
		instance.Set("patternRight", patternRight);
		instance.Modulate = color;
		instance.SelfModulate = color;
		_players.Add(instance);
		this.AddChild(instance);
		//_hasSpawned = true;
	}
	private void LoadPlayers(player_settings settings)
	{
		for(int i =0; i < settings.PlayerInfos.Count; i++)
		{
			SpawnPlayer(settings.PlayerInfos[i].PlayerID,
				settings.PlayerInfos[i].LeftPattern,
				settings.PlayerInfos[i].RightPattern,
				settings.PlayerInfos[i].Color,
				settings.PlayerInfos[i].X,
				settings.PlayerInfos[i].Y);
		}
	}

	private void LoadMap(string mapName)
	{
		// Choose the map
		PackedScene map;

		switch (mapName)
		{
			case "circle":
				{
					map = circle; break;
				}
			case "compactor":
				{
					map = compactor; break;
				}
			case "boxes":
				{
					map = movingBoxes; break;
				}
			case "bar":
				{
					map = spinningBar; break;
				}
			default:
				{
					map = blank; break;
				}
		}

		// then instantiate and add to scene
		var mapInstance = map.Instantiate();
		mapInstance.Set("position", this.Position);
		AddChild(mapInstance);
	}
}
