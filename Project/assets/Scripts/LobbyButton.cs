using Godot;
using System;

public partial class LobbyButton : Button
{
	string gameScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameScene = "res://Game.tscn";
		SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
		sceneManager.ReadyScene(gameScene);		
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_pressed()
	{
		
		AudioStreamPlayer music = GetNode<AudioStreamPlayer>("/root/SoundManager/Music");
		music.Stop();
		music.Set("stream", GD.Load<AudioStream>("res://assets/Music/battlemusicDraft.wav"));
		SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
		sceneManager.GotoReadyScene(gameScene);
	}
}
