using Godot;
using System;

public partial class PauseControlButton : Button
{
	private Control controls;
	private Button resumeButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		controls = GetNode<Control>("%Controls");
		resumeButton = GetNode<Button>("%Resume");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(controls.Visible)
		{
            Button button = GetNode<Button>("%Button");
            button.GrabFocus();
        }
	}
	public void _control_pressed()
	{
		controls.Visible = !controls.Visible;
		resumeButton.GrabFocus();
	}
}
