using Godot;
using System;

public partial class PlayerSpawnButton : Button
{
	[Export]
	GameManager _gameManager;
	
		// All the Patterns (get a better solution later)
	PackedScene patternCircle = GD.Load<PackedScene>("res://assets/prefabs/PatternCircleBurst.tscn");
	PackedScene patternSpreadShot = GD.Load<PackedScene>("res://assets/prefabs/PatternSpreadShot.tscn");
	PackedScene patternFastSS = GD.Load<PackedScene>("res://assets/prefabs/PatternFastSS.tscn");
	PackedScene patternWeave = GD.Load<PackedScene>("res://assets/prefabs/PatternWeave.tscn");
	PackedScene patternKnot = GD.Load<PackedScene>("res://assets/prefabs/PatternKnot.tscn");
	PackedScene patternSwirl = GD.Load<PackedScene>("res://assets/prefabs/PatternSwirl.tscn");
	PackedScene patternWillow = GD.Load<PackedScene>("res://assets/prefabs/PatternWillow.tscn");

	PlayerSelectButton options;
	
	MarginContainer container;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		options = GetNode<PlayerSelectButton>("%OptionButton");
		container = GetNode<MarginContainer>("%MarginContainer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_pressed()
	{
		//switch(options._numSelected){
		//	case 0:{
		//		// 1 player scene
		//		GetTree().ChangeSceneToFile("res://assets/cheatScenes/Game(1 Player).tscn");
		//		break;
		//	}
		//	case 1:{
		//		// 2 player scene
		//		GetTree().ChangeSceneToFile("res://assets/cheatScenes/Game(2 Player).tscn");
		//		break;
		//	}
		//	case 2:{
		//		// 3 player scene
		//		GetTree().ChangeSceneToFile("res://assets/cheatScenes/Game(3 Player).tscn");
		//		break;
		//	}
		//	case 3:{
		//		// 4 player scene
		//		GetTree().ChangeSceneToFile("res://assets/cheatScenes/Game(4 Players).tscn");
		//		break;
		//	}
		//	default:{
		//		// 1 player scene
		//		GetTree().ChangeSceneToFile("res://assets/cheatScenes/Game(1 Player).tscn");
		//		break;
		//	}
		//}

		// get a reference to the player_settings singleton
		player_settings settings = (player_settings)GetNode("/root/PlayerSettings");

		//temp code to prevent spawning bug, remove once player select screen is setup
		//	settings.Clear();

		switch (options._numSelected)
		{	
			case 0:
				{
					settings.AddPlayerInfo(0, patternCircle, patternSpreadShot, Colors.Orange, -600, -300);
					GD.Print("1 Player Spawned");
					break;
				}
			case 1:
				{
					settings.AddPlayerInfo(1, patternFastSS, patternWeave, Colors.Green, 600, -300);
					settings.AddPlayerInfo(0, patternCircle, patternSpreadShot, Colors.Orange, -600, -300);
					GD.Print("2 Player Spawned");
					break;
				}
			case 2:
				{
					settings.AddPlayerInfo(0, patternCircle, patternSpreadShot,Colors.Aqua, -600, -300);
					settings.AddPlayerInfo(1, patternFastSS, patternWeave, Colors.Gold, 600, -300);
					settings.AddPlayerInfo(2, patternSwirl, patternKnot, Colors.Red, -500, 300);
					GD.Print("3 Player Spawned");
					break;
				}
			case 3:
				{
					settings.AddPlayerInfo(0, patternCircle, patternSpreadShot,Colors.Aquamarine, -600, -300);
					settings.AddPlayerInfo(1, patternFastSS, patternWeave, Colors.Purple, 600, -300);
					settings.AddPlayerInfo(2, patternSwirl, patternKnot, Colors.Red, -500, 300);
					settings.AddPlayerInfo(3, patternSwirl, patternWeave, Colors.Green, 500, 300);

					GD.Print("4 Player Spawned");
					break;
				}
			default:
				{
					settings.AddPlayerInfo(0, patternCircle, patternSpreadShot,Colors.Aquamarine, -600, -300);
					GD.Print("Default Player Spawned");
					break;
				}
		}
		GetTree().ChangeSceneToFile("res://Game.tscn");
	}
}
