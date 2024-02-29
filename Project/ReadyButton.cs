using Godot;
using System;
using System.Diagnostics;

public partial class ReadyButton : Button
{
	private Button button;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		button = GetNode<Button>("/root/PlayerSelect/Player1/ColorRect/ReadyUp");
		button.GrabFocus();
	}

    public override void _GuiInput(InputEvent @event)
    {
        base._GuiInput(@event);
		if(@event is InputEventMouseButton mbe && mbe.ButtonIndex == MouseButton.Left && mbe.Pressed) {
			InputEventKey a = new InputEventKey();
			a.Keycode = Key.L;

			InputMap.ActionAddEvent("Burst_0", a);
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		//button.GrabFocus();
		if(Input.IsActionPressed("Burst_0")) {
			Debug.Print("Bursted");
		}
	}
}
