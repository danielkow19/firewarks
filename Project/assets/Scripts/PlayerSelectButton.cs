using Godot;
using System;

public partial class PlayerSelectButton : Godot.OptionButton
{
	OptionButton button;

	public int _numSelected;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Will be reassigned later
		_numSelected = -1;
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
	}
}
