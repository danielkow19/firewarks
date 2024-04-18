using FireWARks.assets.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

public partial class StartManager : Node2D
{
	private string[] scenePaths = { "res://StartMenu.tscn", "res://Game.tscn", "res://GameOver.tscn" };
	private Control _pauseMenu;
	private Button _resumeButton;
	private bool _paused;
	public string currentScene;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Hook up menu logic
		_pauseMenu = GetNode<Control>("%PauseMenu");
		_resumeButton = GetNode<Button>("PauseMenu/MarginContainer/VBoxContainer/Resume");
		_paused = false;

		// Set current Scene
		//player_settings settings = (player_settings)GetNode("/root/PlayerSettings");
		//settings.Clear();	
		SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
		sceneManager.ReadyScene("res://assets/prefabs/SelectMenu.tscn");
		currentScene = sceneManager.currentScene.SceneFilePath;
		AudioStreamPlayer music = GetNode<AudioStreamPlayer>("/root/SoundManager/Music");
		if(!music.Playing)
		{
		music.Set("stream", GD.Load<AudioStream>("res://assets/Music/menusongfirstdraft.wav"));
		music.Play();
		}
		
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

		if (Input.IsKeyPressed(Key.Key0))
		{
			// Change to GameOver Scene
			SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
			sceneManager.GoToScene(scenePaths[2]);
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
}
