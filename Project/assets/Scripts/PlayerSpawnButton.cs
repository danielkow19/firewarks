using Godot;
using System;

public partial class PlayerSpawnButton : Button
{
	[Export]
	GameManager _gameManager;
	
		// All the Patterns (get a better solution later)
	PackedScene pattern1 = GD.Load<PackedScene>("res://assets/prefabs/Pattern1.tscn");
	PackedScene pattern2 = GD.Load<PackedScene>("res://assets/prefabs/Pattern2.tscn");
	PackedScene pattern3 = GD.Load<PackedScene>("res://assets/prefabs/Pattern3.tscn");
	PackedScene pattern4 = GD.Load<PackedScene>("res://assets/prefabs/Pattern4.tscn");
	PackedScene pattern5 = GD.Load<PackedScene>("res://assets/prefabs/Pattern5.tscn");
	
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

		// color field to set player colors
		Color set;

        switch (options._numSelected){
			case 0:
				{

					_gameManager.SpawnPlayer(0, pattern1, pattern2, -600, -300);
					//settings.AddPlayerInfo(0, pattern1, pattern2, Colors.Orange, -600, -300);
					GD.Print("1 Player Spawned");
					break;
				}
			case 1:
				{
					_gameManager.SpawnPlayer(0, pattern4, pattern2, -600, -300);
					_gameManager.SpawnPlayer(1, pattern2, pattern3, 600, -300);
					GD.Print("2 Player Spawned");
					break;
				}
			case 2:
				{
					_gameManager.SpawnPlayer(0, pattern1, pattern2, -600, -300);
					_gameManager.SpawnPlayer(1, pattern2, pattern3, 600, -300);
					_gameManager.SpawnPlayer(2, pattern3, pattern4, -600, 300);
					GD.Print("3 Player Spawned");
					break;
				}
			case 3:
				{
					_gameManager.SpawnPlayer(0, pattern1, pattern2, -600, -300);
					_gameManager.SpawnPlayer(1, pattern2, pattern3, 600, -300);
					_gameManager.SpawnPlayer(2, pattern3, pattern4, -600, 300);
					_gameManager.SpawnPlayer(3, pattern4, pattern5, 600, 300);
					GD.Print("4 Player Spawned");
					break;
				}
			default:
				{
					_gameManager.SpawnPlayer(0, pattern4, pattern1, -600, -300);
					GD.Print("Default Player Spawned");
					break;
				}
		}
		container.Hide();
	}
}
