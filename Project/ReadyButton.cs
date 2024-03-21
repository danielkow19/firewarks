using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class ReadyButton : Button
{
	private Button button;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		button = GetNode<Button>("../ReadyUp");
	}

	public override void _GuiInput(InputEvent @event)
	{
		base._GuiInput(@event);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_pressed()
	{

        //TEMP change scene
        //GetTree().ChangeSceneToFile("res://Game.tscn");
    }
}
