using Godot;
using System;

public partial class PlayerSelectButton : Godot.OptionButton
{
	OptionButton button;
	[Export]
	GameManager _gameManager;

	// All the Patterns (get a better solution later)
	PackedScene pattern1 = GD.Load<PackedScene>("res://assets/prefabs/Pattern1.tscn");
    PackedScene pattern2 = GD.Load<PackedScene>("res://assets/prefabs/Pattern2.tscn");
    PackedScene pattern3 = GD.Load<PackedScene>("res://assets/prefabs/Pattern3.tscn");
    PackedScene pattern4 = GD.Load<PackedScene>("res://assets/prefabs/Pattern4.tscn");
    PackedScene pattern5 = GD.Load<PackedScene>("res://assets/prefabs/Pattern5.tscn");

	private int _numSelected;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		button = GetNode<OptionButton>("%OptionButton");
		addItems();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void addItems()
	{
		button.AddItem("1");
		button.AddItem("2");
        button.AddItem("3");
        button.AddItem("4");
    }

	public void _on_item_selected(int index)
	{
		_numSelected = index;
		switch(index)
		{
			case 0:
				{
					_gameManager.SpawnPlayer(0, pattern1, pattern2);
					break;
				}
			case 1:
				{
					_gameManager.SpawnPlayer(0, pattern1, pattern2);
					_gameManager.SpawnPlayer(1, pattern2, pattern3);
					break;
				}
			case 2:
				{
					_gameManager.SpawnPlayer(0, pattern1, pattern2);
					_gameManager.SpawnPlayer(1, pattern2, pattern3);
					_gameManager.SpawnPlayer(2, pattern3, pattern4);
					break;
				}
			case 3:
				{
					_gameManager.SpawnPlayer(0, pattern1, pattern2);
					_gameManager.SpawnPlayer(1, pattern2, pattern3);
					_gameManager.SpawnPlayer(2, pattern3, pattern4);
					_gameManager.SpawnPlayer(3, pattern4, pattern5);
					break;
				}
			default:
				{
					_gameManager.SpawnPlayer(0, pattern4, pattern1);
					break;
				}
				//Spawn 1 player
		}
	}
}
