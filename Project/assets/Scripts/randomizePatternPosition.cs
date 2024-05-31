using Godot;
using System;
using FireWARks.assets.Scripts;

public partial class randomizePatternPosition : Area2D
{
	private RandomNumberGenerator rng;
	private GameManager manager;

	public override void _Ready()
	{
		Node2D parent = GetParent<Node2D>();
		rng = new RandomNumberGenerator();
		manager = GetTree().Root.GetNode<GameManager>("Game2");
		
		bool doAgain = false;

		// do-while because the first time through takes it off the player
		do
		{
			parent.Position = ChoosePosition();
			GlobalPosition = parent.GlobalPosition;

			foreach (Area2D area in GetOverlappingAreas())
			{
				if (area is Player)
				{
					doAgain = true;
					break;
				}   
			}
		} while (doAgain);

		this.QueueFree();
	}

	private Vector2 ChoosePosition()
	{
		Vector2 range = manager.WorldBorder.edgePosition;
		return new Vector2(rng.RandfRange(-range.X + 50, range.X - 50), rng.RandfRange(-range.Y + 50, range.Y - 50));
	}
}
