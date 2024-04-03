using Godot;
using System;

public partial class PlayButton : Node
{
	[Export]
	string scenePath;
	private Button button;
	private bool isFocused;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		button = GetNode<Button>("PlayButton");
		button.Pressed += OnStartButtonPressed;
		isFocused = false;
	}

	private void OnStartButtonPressed()
	{
		SceneManager sceneManager = GetNode<SceneManager>("/root/SceneManager");
    	
		if(scenePath == null)
		{
			
			sceneManager.GoToScene("res://Game.tscn");
		}
		else
		{
			
			sceneManager.GoToScene(scenePath);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Only grab focus once or else the title controls don't work
		if(!isFocused)
		{
			button.GrabFocus();
			isFocused = true;
		}
	}
}
