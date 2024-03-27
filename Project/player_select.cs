using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;


public partial class player_select : Control
{
	private Button button;
	private int numPlayers;
	private List<int> deviceNums;
	bool keyboardPlayer;
	bool joinable;
	PackedScene selectMenu = GD.Load<PackedScene>("res://assets/prefabs/SelectMenu.tscn");
	private int readiedPlayers;

	private Button startButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		numPlayers = 0;
		deviceNums = new List<int>();
		keyboardPlayer = false;
		joinable = true;
		readiedPlayers = 0;

		startButton = GetNode<LobbyButton>("StartButton");
	}


		public override void _UnhandledInput(InputEvent @event)
	{
		base._GuiInput(@event);
		if(@event is InputEventKey ke /*&& ke.Keycode == Key.Space*/ && ke.Pressed && !keyboardPlayer && numPlayers < 4 && joinable)
		{
			// Slow
			InputEventKey keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.Shift;
			InputMap.ActionAddEvent($"Slow_{numPlayers}", keyEvent);

			// Burst
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.Ctrl;
			InputMap.ActionAddEvent($"Burst_{numPlayers}", keyEvent);

			// Left Shoot
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.Space;
			InputMap.ActionAddEvent($"Shoot_L_{numPlayers}", keyEvent);

			// Right Shoot
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.Enter;
			InputMap.ActionAddEvent($"Shoot_R_{numPlayers}", keyEvent);

			// Movement
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.D;
			InputMap.ActionAddEvent($"Right_{numPlayers}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.A;
			InputMap.ActionAddEvent($"Left_{numPlayers}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.W;
			InputMap.ActionAddEvent($"Up_{numPlayers}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.S;
			InputMap.ActionAddEvent($"Down_{numPlayers}", keyEvent);

			// Aiming
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.L;
			InputMap.ActionAddEvent($"AimRight_{numPlayers}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.J;
			InputMap.ActionAddEvent($"AimLeft_{numPlayers}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.I;
			InputMap.ActionAddEvent($"AimUp_{numPlayers}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.K;
			InputMap.ActionAddEvent($"AimDown_{numPlayers}", keyEvent);

            // UI Click Event
			keyEvent = new InputEventKey(); 
			keyEvent.Keycode = Key.Enter; 
			InputMap.ActionAddEvent($"UI_Click_{numPlayers}", keyEvent);

            keyboardPlayer = true;
			numPlayers++;

			InstantiateSelectMenu();
		}
		if(@event is InputEventJoypadButton jbe /*&& jbe.ButtonIndex == JoyButton.A*/ && jbe.Pressed && !deviceNums.Contains(jbe.Device) && numPlayers < 4 && joinable)
		{
			InputEventJoypadButton joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.LeftShoulder;
			InputMap.ActionAddEvent($"Slow_{numPlayers}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.RightShoulder;
			InputMap.ActionAddEvent($"Burst_{numPlayers}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.DpadLeft;
			InputMap.ActionAddEvent($"Left_{numPlayers}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.DpadRight;
			InputMap.ActionAddEvent($"Right_{numPlayers}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.DpadUp;
			InputMap.ActionAddEvent($"Up_{numPlayers}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.DpadDown;
			InputMap.ActionAddEvent($"Down_{numPlayers}", joyButton);

            // UI Click Event
			joyButton = new InputEventJoypadButton(); 
			joyButton.Device = jbe.Device; 
			InputMap.ActionAddEvent($"UI_Click_{numPlayers}", joyButton);

            InputEventJoypadMotion joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.TriggerLeft;
			InputMap.ActionAddEvent($"Shoot_L_{numPlayers}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.TriggerRight;
			InputMap.ActionAddEvent($"Shoot_R_{numPlayers}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.LeftX;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"Right_{numPlayers}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.LeftX;
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"Left_{numPlayers}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.LeftY;
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"Up_{numPlayers}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.LeftY;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"Down_{numPlayers}", joyAxis);
			
			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.RightX;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"AimRight_{numPlayers}", joyAxis);
			
			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.RightX;
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"AimLeft_{numPlayers}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.RightY;
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"AimUp_{numPlayers}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.RightY;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"AimDown_{numPlayers}", joyAxis);


			deviceNums.Add(jbe.Device);
			numPlayers++;

			InstantiateSelectMenu();
		}
	}

	public void InstantiateSelectMenu() {
		var instance = selectMenu.Instantiate();
			switch(numPlayers) {
				case 1:
					break;
				case 2:
					instance.Set("position", new Vector2(960,0));
					break;
				case 3:
					instance.Set("position", new Vector2(0,540));
					break;
				case 4:
					instance.Set("position", new Vector2(960,540));
					break;
			}
			instance.GetNode<CursorAlt>("ColorRect/Cursor").Set("playerNum", numPlayers - 1);
		instance.GetNode<ReadyButton>("ColorRect/ReadyUp").Set("joinNode", this);
		AddChild(instance);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Game Start Code
		if(numPlayers >=1 && numPlayers == readiedPlayers) {
			//GetTree().ChangeSceneToFile("res://assets/cheatScenes/Game(2 Player).tscn");

			// Pop up a button that will allow us to change scenes
			startButton.Show();
			startButton.GrabFocus();
		}
	}

	public void ReadyPlayer()
	{
		readiedPlayers++;
	}
}
