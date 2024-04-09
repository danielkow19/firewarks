using Godot;
using System;
using System.Diagnostics;

public partial class Pause : Control
{
	[Export]
	private Node _manager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button button = GetNode<Button>("MarginContainer/VBoxContainer/Resume");
		button.GrabFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	public void _on_resume_pressed()
	{
		_manager.Call("PauseMenu");
	}
}
