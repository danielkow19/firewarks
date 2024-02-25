using Godot;
using System;

public partial class PlayButton : Node
{
	[Export]
	string scenePath;
	private Button button;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		button = GetNode<Button>("PlayButton");
		button.Pressed += OnStartButtonPressed;
		button.GrabFocus();
	}

	private void OnStartButtonPressed()
	{
		if(scenePath == null)
		{
			GetTree().ChangeSceneToFile("res://Game.tscn");
		}
		else
		{
			GetTree().ChangeSceneToFile(scenePath);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		button.GrabFocus();
	}
}
