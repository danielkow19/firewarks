using Godot;
using System;
using System.Collections.Generic;
//using System.Numerics;

public partial class CursorAlt : Node2D
{
	bool infoAdded = false;
	public int playerNum;
	public int positionIndex;
	private Godot.Vector2[] positions = {new Godot.Vector2(550, 100), new Godot.Vector2(550, 200), new Godot.Vector2(550, 300), new Godot.Vector2(565, 400)};
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
		Input.ActionRelease($"UI_Click_{playerNum}");

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
			positionIndex -= 1;
		} 
		else if (Input.IsActionJustPressed($"Down_{playerNum}")) {
			positionIndex += 1;
		} 
		else if (Input.IsActionJustPressed($"Left_{playerNum}")) {
			switch(positionIndex) {
				case 0:// Color Left
					colorSwap._ChangeColor("left");
					break;
				case 1:
					attack1Swap._Change_Attack("left");
					break;
				case 2:
				attack2Swap._Change_Attack("left");
					break;
				default:
					break;
			}
		} 
		else if (Input.IsActionJustPressed($"Right_{playerNum}")) {
			switch(positionIndex) {
				case 0:// Color Left
					colorSwap._ChangeColor("right");
					break;
				case 1:
					attack1Swap._Change_Attack("right");
					break;
				case 2:
				attack2Swap._Change_Attack("right");
					break;
				default:
					break;
			}
		}

		if(positionIndex > 3) {
			positionIndex = 3;
		} else if (positionIndex < 0) {
			positionIndex = 0;
		}

		Position = positions[positionIndex];

		// Cursor click logic
		if(Input.IsActionJustPressed($"UI_Click_{playerNum}"))
		{
			// switch(positionIndex)
			// {
			//     case 0:// Color Left
			// 	case 1:
			// 		{
			// 			// Color Right
			// 			ArrowColor color = (ArrowColor)ButtonList[positionIndex];
			// 			color._on_pressed();
			// 			break;
			// 		}
			// 	case 2://Attack1Left
			// 	case 3://Attack1Right
			// 	case 4://Attack2Left
			// 	case 5:
			// 		{
			//             //Attack2Right
			// 			ArrowAttack attack = (ArrowAttack)ButtonList[positionIndex];
			// 			attack._on_pressed();
			//             break;
			// 		}
			// 	case 6:
			// 		{
			// 			//ReadyUp
			// 			// Feed in the PlayerInfo
			// 			if(!infoAdded) {
			// 				addPlayerInfo();

			// 				// Call the on press
			// 				ReadyButton ready = (ReadyButton)ButtonList[6];
			// 				ready._on_pressed();

			// 				infoAdded = true;
			// 			}
			// 			break;
			// 		}
			// }
			if(positionIndex == 3) {
				if(!infoAdded) {
					addPlayerInfo();

					// Call the on press
					ReadyButton ready = (ReadyButton)ButtonList[6];
					ready._on_pressed();

					infoAdded = true;
				}
			}
		}
	}
	public void addPlayerInfo()
	{
		// get a reference to the player_settings singleton
		player_settings settings = (player_settings)GetNode("/root/PlayerSettings");

		// position logic will go below
		// Have code that takes in the map, then the player ID to determine the spawn position

		settings.AddPlayerInfo(playerNum, patternsList[attack1Swap.AttackIndex], patternsList[attack2Swap.AttackIndex], colorSwap.ColorChoice, getSpawnPosition());
	}
	private float getSpawnX()
	{
		switch(playerNum)
		{
			case 0:
				{
					return -600f;
				}
			case 1:
				{
					return 600f;
				}
			case 2:
				{
					return -500f;

				}
			case 3:
				{
					return 500f;

				}
			default:
				{
					return 0f;
				}
		}
	}
	private float getSpawnY()
	{
		switch(playerNum)
		{
			case 0: return -300f;
			case 1: return -300f;
			case 2: return 300f;
			case 3: return 300f;
			default: return 0f;
		}
	}
	
	private Vector2 getSpawnPosition()
	{
		switch(playerNum)
		{
			case 0:	return new Vector2(-600f, -300f);
			case 1:	return new Vector2(600f, -300f);
			case 2:	return new Vector2(-500f, 300f);
			case 3:	return new Vector2(500f, 300f);
			default: { return Vector2.Zero; }
		}
	}
}
