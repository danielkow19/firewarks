using Godot;
using System;

public partial class ControlsButton : Button
{
	public Control _controlPop;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_controlPop = GetNode<Control>("/root/StartMenu/Controls");
	}

	private void _on_pressed()
	{
		// Change the scene
		_controlPop.Visible = !_controlPop.Visible;
	}
}
