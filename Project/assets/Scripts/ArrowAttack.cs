using Godot;
using System;

public partial class ArrowAttack : Button
{
	// Name
	[Export]
	private string name;

	// Reference to it's label/controller
	[Export]
	private AttackSwap swap;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	public void _on_pressed()
	{
		swap._Change_Attack(name);
	}
}
