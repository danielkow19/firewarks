using Godot;
using System;

public partial class BulletTrail : Line2D
{
	private int maxPoints = 20;
	private Curve2D curve;
	private Area2D parent;

	private double baseCd;
	private double cd = 0;
	private Vector2 pointToAdd;
	
	
	//../../Camera2D
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		curve = new Curve2D();
		parent = GetParent<Area2D>();
		//GD.Print("cd" + baseCd);
		pointToAdd = new Vector2(parent.GlobalPosition.X, parent.GlobalPosition.Y);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		cd -= delta;
		if(cd <= 0)
		{
			curve.AddPoint(pointToAdd);
			pointToAdd = new Vector2(parent.GlobalPosition.X, parent.GlobalPosition.Y);
			cd = baseCd;
		}
		if (curve.PointCount > maxPoints)
		{
			curve.RemovePoint(0);
		}
		Points = curve.GetBakedPoints();
	}
}
