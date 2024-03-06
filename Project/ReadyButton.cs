using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class ReadyButton : Button
{
	private Button button;
	private int numPlayers;
	private List<int> deviceNums;
	bool keyboardPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		button = GetNode<Button>("/root/PlayerSelect/Player1/ColorRect/ReadyUp");
		button.GrabFocus();
		numPlayers = 0;
		deviceNums = new List<int>();
		keyboardPlayer = false;
	}

    public override void _GuiInput(InputEvent @event)
    {
        base._GuiInput(@event);
		if(@event is InputEventMouseButton mbe && mbe.ButtonIndex == MouseButton.Left && mbe.Pressed && !keyboardPlayer && numPlayers < 4) 
		{
			InputEventKey keyEvent = new InputEventKey();
			//keyEvent.Keycode = Key.A;

			InputMap.ActionAddEvent($"Slow_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"Burst_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"Shoot_L_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"Shoot_R_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"Right_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"Left_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"Up_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"Down_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"AimRight_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"AimLeft_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"AimUp_{numPlayers}", keyEvent);
			InputMap.ActionAddEvent($"AimDown_{numPlayers}", keyEvent);
			keyboardPlayer = true;
			numPlayers++;
		}
		if(@event is InputEventJoypadButton jbe && jbe.ButtonIndex == JoyButton.A && jbe.Pressed && !deviceNums.Contains(jbe.Device) && numPlayers < 4) 
		{
			InputEventJoypadButton joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.LeftShoulder;
			InputMap.ActionAddEvent($"Slow_{numPlayers}", joyButton);
			joyButton.ButtonIndex = JoyButton.RightShoulder;
			InputMap.ActionAddEvent($"Burst_{numPlayers}", joyButton);

			InputEventJoypadMotion joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.TriggerLeft;
			InputMap.ActionAddEvent($"Shoot_L_{numPlayers}", joyAxis);
			joyAxis.Axis = JoyAxis.TriggerRight;
			InputMap.ActionAddEvent($"Shoot_R_{numPlayers}", joyAxis);
			joyAxis.Axis = JoyAxis.LeftX;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"Right_{numPlayers}", joyAxis);
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"Left_{numPlayers}", joyAxis);
			joyAxis.Axis = JoyAxis.LeftY;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"Up_{numPlayers}", joyAxis);
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"Down_{numPlayers}", joyAxis);
			joyAxis.Axis = JoyAxis.RightX;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"AimRight_{numPlayers}", joyAxis);
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"AimLeft_{numPlayers}", joyAxis);
			joyAxis.Axis = JoyAxis.RightY;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"AimUp_{numPlayers}", joyAxis);
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"AimDown_{numPlayers}", joyAxis);

			deviceNums.Add(jbe.Device);
			numPlayers++;
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		//button.GrabFocus();
		if(Input.IsActionPressed("Burst_0")) 
		{
			Debug.Print("Bursted");
		}
	}
}
