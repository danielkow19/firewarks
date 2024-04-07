using Godot;
using System;
using System.Collections.Generic;
using FireWARks.assets.Scripts;

public partial class WorldBorder : Area2D
{
	[Export] private double timeSeconds = 300f;
	private Timer countDown;
	private float scale;
	[Export] private float closingRate = .05f;
	[Export] private Sprite2D[] edges;
	public Vector2 edgePosition;
	
	/// <summary>
	/// Class Assumptions
	///		- Everything is centered at (0,0)
	///		- The top left vertex of the collision polygon is vertex 1, and the collision polygon is the first child
	///		- All edges are positioned correctly and of appropriate size
	/// </summary>
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scale = 1;
		countDown = new Timer();
		AddChild(countDown);
		countDown.OneShot = true;
		countDown.Start(timeSeconds); 
		edgePosition = GetChild<CollisionPolygon2D>(0).Polygon[1].Abs();
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (countDown.TimeLeft == 0 && Math.Abs(scale - .25f) > .01f)
		{
			scale -= closingRate * (float)delta;

			if (scale < .25f)
			{
				scale = .25f;
			}
			
			// Changes the scale of this collider, Sprites are top level so are unaffected
			Scale = new Vector2(scale, scale);
			
			
			// Top left corner
			edgePosition = GetChild<CollisionPolygon2D>(0).Polygon[1].Abs() * scale;

			
			// Update position of edges
			foreach (Sprite2D edge in edges)
			{
				// Determine correct edge
				if (edge.GlobalPosition.X == 0)
				{
					if (edge.GlobalPosition.Y < 0)
					{
						edge.Anch
						edge.GlobalPosition = new Vector2(0, -edgePosition.Y * 1.5625f);
					}
					else
					{
						edge.GlobalPosition = new Vector2(0, edgePosition.Y * 1.5625f);
					}
				}
				else
				{
					if (edge.GlobalPosition.X < 0)
					{
						edge.GlobalPosition = new Vector2(-edgePosition.X - 200, 0);
					}
					else
					{
						edge.GlobalPosition = new Vector2(edgePosition.X + 200, 0);
					}
				}
			}
		}
	}
	
	private void _on_area_entered(Area2D area)
	{
		if (area is Player player)
		{
			player.numClouds++;
		}
	}
	
	private void _on_area_exited(Area2D area)
	{
		if (area is Player player)
		{
			player.numClouds--;
		}
	}
}
