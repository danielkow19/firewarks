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
		switch(options._numSelected){
			case 0:
				{
					_gameManager.SpawnPlayer(0, pattern1, pattern2, -600, -300);
					GD.Print("1 Player Spawned");
					break;
				}
			case 1:
				{
					_gameManager.SpawnPlayer(0, pattern1, pattern2, -600, -300);
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
		//container.Hide();
	}
}
