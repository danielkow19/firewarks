using Godot;
using System;
using System.Diagnostics;

public partial class GameOver : Node2D
{
	private player_settings settings;
	private PackedScene stats;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
		stats = sceneManager.GetReadyScene("res://assets/prefabs/PlayerStats.tscn");
		settings = (player_settings)GetNode("/root/PlayerSettings");
		InstantiateStats(settings.numPlayers);
		AudioStreamPlayer music = GetNode<AudioStreamPlayer>("/root/SoundManager/Music");
		music.Set("stream", GD.Load<AudioStream>("res://assets/Music/menusongfirstdraft.wav"));
		music.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void InstantiateStats(int numPlayers) {
		for(int i = 0; i < numPlayers; i++) {
			var instance = stats.Instantiate();
			instance.Set("playerNum", i);
			int x = 24 + 474 * i + (4-numPlayers) * 237;
			Debug.Print($"{x}");
			instance.Set("global_position", new Vector2(x, 170));
			AddChild(instance);
		}
	}
}
