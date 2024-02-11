using Godot;
using System;

public partial class GameOverButton : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button button = GetNode<Button>("Button");
		button.Pressed += OnButtonPressed;
		button.GrabFocus();
	}

	private void OnButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://StartMenu.tscn");

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
