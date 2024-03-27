using Godot;
using System;

public partial class ControlsButton : Button
{
	private string currentScene;
	private string scenePath;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentScene = GetTree().CurrentScene.SceneFilePath;
		if(currentScene == "res://StartMenu.tscn")
		{
			scenePath = "res://assets/prefabs/Controls.tscn";
        }
		else
		{
			scenePath = "res://StartMenu.tscn";
            this.GrabFocus();
        }
		
	}

	private void _on_pressed()
	{
		GD.Print("Pressed");
		// Change the scene
		GetTree().ChangeSceneToFile(scenePath);
	}
}
