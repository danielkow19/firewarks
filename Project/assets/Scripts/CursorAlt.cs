using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;

public partial class CursorAlt : Node2D
{
	public int playerNum;
	public int positionIndex;
	private Godot.Vector2[] positions = {new Godot.Vector2(510, 100), new Godot.Vector2(850, 100), new Godot.Vector2(510, 200), new Godot.Vector2(855, 200), new Godot.Vector2(510, 300), new Godot.Vector2(850, 300), new Godot.Vector2(565, 400)};
	private List<Button> ButtonList;
	private List<PackedScene> patternsList;

	// Reference to the info managing scripts (not arrows)
	[Export]
	private ColorSwap colorSwap;
	[Export]
	private AttackSwap attack1Swap;
	[Export]
	private AttackSwap attack2Swap;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		positionIndex = 0;

        ButtonList = new List<Button> { GetNode<Button>("../ColorLeft"), GetNode<Button>("../ColorRight"), 
			GetNode<Button>("../Attack1Left") ,GetNode<Button>("../Attack1Right"),
            GetNode<Button>("../Attack2Left"), GetNode<Button>("../Attack2Right"),
            GetNode<Button>("../ReadyUp")
        };

		patternsList = new List<PackedScene> { GD.Load<PackedScene>("res://assets/prefabs/PatternCircleBurst.tscn"),
			GD.Load<PackedScene>("res://assets/prefabs/PatternFastSS.tscn"), GD.Load<PackedScene>("res://assets/prefabs/PatternKnot.tscn"),
			GD.Load<PackedScene>("res://assets/prefabs/PatternSpreadShot.tscn"), GD.Load<PackedScene>("res://assets/prefabs/PatternSwirl.tscn"),
			GD.Load<PackedScene>("res://assets/prefabs/PatternWeave.tscn"), GD.Load<PackedScene>("res://assets/prefabs/PatternWillow.tscn")
		};
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Cursor movement Logic
		if(Input.IsActionJustPressed($"Up_{playerNum}")) {
			positionIndex -= 2;
		} else if (Input.IsActionJustPressed($"Down_{playerNum}")) {
			positionIndex += 2;
		} else if (Input.IsActionJustPressed($"Left_{playerNum}")) {
			positionIndex -= 1;
		} else if (Input.IsActionJustPressed($"Right_{playerNum}")) {
			positionIndex += 1;
		}

		if(positionIndex > 6) {
			positionIndex = 6;
		} else if (positionIndex < 0) {
			positionIndex = 0;
		}

		Position = positions[positionIndex];

		// Cursor click logic
		if(Input.IsActionJustPressed($"UI_Click_{playerNum}"))
		{
			switch(positionIndex)
            {
                case 0:// Color Left
				case 1:
					{
						// Color Right
						ArrowColor color = (ArrowColor)ButtonList[positionIndex];
						color._on_pressed();
						break;
					}
				case 2://Attack1Left
				case 3://Attack1Right
				case 4://Attack2Left
				case 5:
					{
                        //Attack2Right
						ArrowAttack attack = (ArrowAttack)ButtonList[positionIndex];
						attack._on_pressed();
                        break;
					}
				case 6:
					{
						//ReadyUp
						// Feed in the PlayerInfo
						addPlayerInfo();

						// Call the on press
						ReadyButton ready = (ReadyButton)ButtonList[6];
						ready._on_pressed();
						break;
					}
			}
		}
	}
	public void addPlayerInfo()
	{
        // get a reference to the player_settings singleton
        player_settings settings = (player_settings)GetNode("/root/PlayerSettings");

		// position logic will go below
		float x = 0;
		float y = 0;

		// Have code that takes in the map, then the player ID to determine the spawn position

		settings.AddPlayerInfo(playerNum, patternsList[attack1Swap.AttackIndex], patternsList[attack2Swap.AttackIndex], colorSwap.ColorChoice, x, y);
    }
}
