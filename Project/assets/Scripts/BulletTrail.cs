using Godot;
using System;

public partial class BulletTrail : Line2D
{
	private int maxPoints = 20;
	private Curve2D curve;
	private Area2D parent;
	
	
	//../../Camera2D
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		curve = new Curve2D();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
				
		Area2D parent = GetParent<Area2D>();
		curve.AddPoint(new Vector2(parent.GlobalPosition.X + -7 * Mathf.Cos(parent.Rotation), 
			parent.GlobalPosition.Y + -7 * Mathf.Sin(parent.Rotation)));
		if (curve.PointCount > maxPoints)
		{
			curve.RemovePoint(0);
		}
		Points = curve.GetBakedPoints();
	}
}
