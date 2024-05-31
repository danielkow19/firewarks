using Godot;
using System;

public partial class ArrowColor : Button
{
	// Reference to self
	[Export]
	private string name;

	// Reference to color swap
	[Export]
	private ColorSwap swap;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_pressed() {
		swap._ChangeColor(name);
	}
}
