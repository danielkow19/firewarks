using Godot;
using System;

public partial class ArrowMap : Button
{
	[Export]
	private string name;

	[Export]
	private MapSelect mapSelect;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_pressed()
	{
		mapSelect._Change_Map(name);
	}
}
