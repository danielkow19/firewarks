using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


public partial class player_select : Control
{
	public int[] colorIndices = new int[4] {-1,-1,-1,-1};
	private Button button;
	private int numPlayers;
	private List<int> deviceNums;
	int keyboardPlayer;
	bool joinable;
	PackedScene selectMenu;
	private int readiedPlayers;

	private int[] takenPostions = new int[4] {0,0,0,0};
	private Node[] menus = new Node[4] {null, null, null, null};

	private Button startButton;

	private int currentPlayerID = -1;

	private player_settings settings;
	private SceneManager sceneManager;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EraseInputs();
		numPlayers = 0;
		deviceNums = new List<int>();
		keyboardPlayer = -1;
		joinable = true;
		readiedPlayers = 0;
		sceneManager = GetNode<SceneManager>("/root/SceneManager");
		selectMenu =  sceneManager.GetReadyScene("res://assets/prefabs/SelectMenu.tscn");
		startButton = GetNode<LobbyButton>("StartButton");
		settings = (player_settings)GetNode("/root/PlayerSettings");
	}


	//!! Move this to its own global class(future)
		public override void _UnhandledInput(InputEvent @event)
	{
		base._GuiInput(@event);
		// Make Sure Inputs are clear before reassigning
		if(@event is InputEventKey ke /*&& ke.Keycode == Key.Space*/ && ke.Pressed && keyboardPlayer == -1 && numPlayers < 4 && joinable && ke.Keycode != Key.Backspace)
		{
			currentPlayerID = -1;
			for(int i = 0; i < 4; i++) {
				if(menus[i] == null) {
					currentPlayerID = i;
					break;
				}
			}

			// Slow
			InputEventKey keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.Shift;
			InputMap.ActionAddEvent($"Slow_{currentPlayerID}", keyEvent);

			// Burst
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.Ctrl;
			InputMap.ActionAddEvent($"Burst_{currentPlayerID}", keyEvent);

			// Left Shoot
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.Space;
			InputMap.ActionAddEvent($"Shoot_L_{currentPlayerID}", keyEvent);

			// Right Shoot
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.Enter;
			InputMap.ActionAddEvent($"Shoot_R_{currentPlayerID}", keyEvent);

			// Movement
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.D;
			InputMap.ActionAddEvent($"Right_{currentPlayerID}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.A;
			InputMap.ActionAddEvent($"Left_{currentPlayerID}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.W;
			InputMap.ActionAddEvent($"Up_{currentPlayerID}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.S;
			InputMap.ActionAddEvent($"Down_{currentPlayerID}", keyEvent);

			// Aiming
			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.L;
			InputMap.ActionAddEvent($"AimRight_{currentPlayerID}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.J;
			InputMap.ActionAddEvent($"AimLeft_{currentPlayerID}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.I;
			InputMap.ActionAddEvent($"AimUp_{currentPlayerID}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.K;
			InputMap.ActionAddEvent($"AimDown_{currentPlayerID}", keyEvent);

            // UI Click Event
			keyEvent = new InputEventKey(); 
			keyEvent.Keycode = Key.Enter; 
			InputMap.ActionAddEvent($"UI_Click_{currentPlayerID}", keyEvent);

			keyEvent = new InputEventKey();
			keyEvent.Keycode = Key.Backspace;
			InputMap.ActionAddEvent($"Back_{currentPlayerID}", keyEvent);

            keyboardPlayer = currentPlayerID;
			numPlayers++;

			InstantiateSelectMenu();
		}
		if(@event is InputEventJoypadButton jbe /*&& jbe.ButtonIndex == JoyButton.A*/ && jbe.Pressed && !deviceNums.Contains(jbe.Device) && numPlayers < 4 && joinable && jbe.ButtonIndex != JoyButton.B)
		{
			currentPlayerID = -1;
			for(int i = 0; i < 4; i++) {
				if(menus[i] == null) {
					currentPlayerID = i;
					break;
				}
			}
			Debug.Print($"{currentPlayerID}");

			InputEventJoypadButton joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.LeftShoulder;
			InputMap.ActionAddEvent($"Slow_{currentPlayerID}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.RightShoulder;
			InputMap.ActionAddEvent($"Burst_{currentPlayerID}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.DpadLeft;
			InputMap.ActionAddEvent($"Left_{currentPlayerID}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.DpadRight;
			InputMap.ActionAddEvent($"Right_{currentPlayerID}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.DpadUp;
			InputMap.ActionAddEvent($"Up_{currentPlayerID}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.DpadDown;
			InputMap.ActionAddEvent($"Down_{currentPlayerID}", joyButton);

            // UI Click Event
			joyButton = new InputEventJoypadButton(); 
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.A;
			InputMap.ActionAddEvent($"UI_Click_{currentPlayerID}", joyButton);

			joyButton = new InputEventJoypadButton();
			joyButton.Device = jbe.Device;
			joyButton.ButtonIndex = JoyButton.B;
			InputMap.ActionAddEvent($"Back_{currentPlayerID}", joyButton);

            InputEventJoypadMotion joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.TriggerLeft;
			InputMap.ActionAddEvent($"Shoot_L_{currentPlayerID}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.TriggerRight;
			InputMap.ActionAddEvent($"Shoot_R_{currentPlayerID}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.LeftX;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"Right_{currentPlayerID}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.LeftX;
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"Left_{currentPlayerID}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.LeftY;
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"Up_{currentPlayerID}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.LeftY;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"Down_{currentPlayerID}", joyAxis);
			
			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.RightX;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"AimRight_{currentPlayerID}", joyAxis);
			
			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.RightX;
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"AimLeft_{currentPlayerID}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.RightY;
			joyAxis.AxisValue = -1;
			InputMap.ActionAddEvent($"AimUp_{currentPlayerID}", joyAxis);

			joyAxis = new InputEventJoypadMotion();
			joyAxis.Device = jbe.Device;
			joyAxis.Axis = JoyAxis.RightY;
			joyAxis.AxisValue = 1;
			InputMap.ActionAddEvent($"AimDown_{currentPlayerID}", joyAxis);


			deviceNums.Add(jbe.Device);
			numPlayers++;

			InstantiateSelectMenu();
		}

		if(@event is InputEventJoypadButton e && e.ButtonIndex == JoyButton.B && e.Pressed && !deviceNums.Contains(e.Device))
		{
			Debug.Print($"Changing Scene");
			sceneManager.ReadyScene("res://StartMenu.tscn");
			sceneManager.GotoReadyScene("res://StartMenu.tscn");
		}
	}

	public void InstantiateSelectMenu() {
		Node instance = selectMenu.Instantiate();
		switch(currentPlayerID) {
			case 0:
				break;
			case 1:
				instance.Set("position", new Vector2(960,0));
				break;
			case 2:
				instance.Set("position", new Vector2(0,540));
				break;
			case 3:
				instance.Set("position", new Vector2(960,540));
				break;
			default:
				// Instantiated right in the center if something goes wrong
				instance.Set("position", new Vector2(480,270));
				break;
		}
		menus[currentPlayerID] = instance;
		instance.GetNode<CursorAlt>("ColorRect/Cursor").Set("playerNum", currentPlayerID);
		instance.GetNode<ReadyButton>("ColorRect/ReadyUp").Set("joinNode", this);
		AddChild(instance);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Game Start Code
		if(numPlayers >=1 && numPlayers == readiedPlayers) {
			//GetTree().ChangeSceneToFile("res://assets/cheatScenes/Game(2 Player).tscn");
			player_settings settings = (player_settings)GetNode("/root/PlayerSettings");
			settings.numPlayers = numPlayers;
			// Pop up a button that will allow us to change scenes
			startButton.Show();
			startButton.GrabFocus();
		}

		for(int i = 0; i < 4; i++) {
			if(Input.IsActionPressed($"Back_{i}")) {
				RemovePlayer(i);
				Debug.Print($"Player {i} Pressed Back");
			}
		}
	}

	public void ReadyPlayer()
	{
		readiedPlayers++;
	}

	private void EraseInputs()
	{
		InputMap.ActionEraseEvents($"UI_Click_{numPlayers}");
        InputMap.ActionEraseEvents($"Slow_{numPlayers}");
        InputMap.ActionEraseEvents($"Burst_{numPlayers}");
        InputMap.ActionEraseEvents($"Up_{numPlayers}");
        InputMap.ActionEraseEvents($"Down_{numPlayers}");
        InputMap.ActionEraseEvents($"Left_{numPlayers}");
        InputMap.ActionEraseEvents($"Right_{numPlayers}");
        InputMap.ActionEraseEvents($"Shoot_R_{numPlayers}");
        InputMap.ActionEraseEvents($"Shoot_L_{numPlayers}");
        InputMap.ActionEraseEvents($"AimUp_{numPlayers}");
        InputMap.ActionEraseEvents($"AimDown_{numPlayers}");
        InputMap.ActionEraseEvents($"AimLeft_{numPlayers}");
        InputMap.ActionEraseEvents($"AimRight_{numPlayers}");
		InputMap.ActionEraseEvents($"Back_{numPlayers}");
    }

	public void EraseInputsByID(int playerID) {
		InputMap.ActionEraseEvents($"UI_Click_{playerID}");
        InputMap.ActionEraseEvents($"Slow_{playerID}");
        InputMap.ActionEraseEvents($"Burst_{playerID}");
        InputMap.ActionEraseEvents($"Up_{playerID}");
        InputMap.ActionEraseEvents($"Down_{playerID}");
        InputMap.ActionEraseEvents($"Left_{playerID}");
        InputMap.ActionEraseEvents($"Right_{playerID}");
        InputMap.ActionEraseEvents($"Shoot_R_{playerID}");
        InputMap.ActionEraseEvents($"Shoot_L_{playerID}");
        InputMap.ActionEraseEvents($"AimUp_{playerID}");
        InputMap.ActionEraseEvents($"AimDown_{playerID}");
        InputMap.ActionEraseEvents($"AimLeft_{playerID}");
        InputMap.ActionEraseEvents($"AimRight_{playerID}");
		InputMap.ActionEraseEvents($"Back_{playerID}");
	}

	public void RemovePlayer(int playerID) {
		Input.ActionRelease($"Back_{playerID}");
		if(keyboardPlayer != playerID) deviceNums.Remove(InputMap.ActionGetEvents($"Back_{playerID}")[0].Device);
		EraseInputsByID(playerID);
		Node menu = menus[playerID];
		menu.Free();
		menus[playerID] = null;
		int index = settings.GetPlayerInfoIndexFromID(playerID);
		if(index != -1) settings.RemovePlayerInfoAt(index);
		numPlayers--;
		if(playerID == keyboardPlayer) keyboardPlayer = -1;
		colorIndices[playerID] = -1;
	}
}
