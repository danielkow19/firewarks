using Godot;
using System;

public partial class LobbyButton : Button
{
	string gameScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameScene = "res://Game.tscn";

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_pressed()
	{
		SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
		sceneManager.GoToScene(gameScene);
	}
}
