using Godot;
using System;

public partial class ControlsButton : Button
{
	[Export]
	public Control _controlPop;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	private void _on_pressed()
	{
		GD.Print("Pressed");
		// Change the scene
		_controlPop.Visible = !_controlPop.Visible;
	}
}
