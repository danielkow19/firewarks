using Godot;
using System;

public partial class StartButton : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button button = GetNode<Button>("StartButton");
		button.Pressed += OnStartButtonPressed;
		button.GrabFocus();
	}

	private void OnStartButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://Game.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
