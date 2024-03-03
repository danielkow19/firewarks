using System;
using System.Collections.Generic;
using Godot;

namespace FireWARks.assets.Scripts;

public partial class PlayerTrail : Line2D
{
	private int maxPoints = 100;
	private Curve2D curve;
	private List<CollisionShape2D> tailSegments;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		curve = new Curve2D();
		tailSegments = new List<CollisionShape2D>();

		for (int i = 0; i < maxPoints - 1; i++)
		{
			tailSegments.Add(new CollisionShape2D());
			tailSegments[i].Shape = new SegmentShape2D();
			AddChild(tailSegments[i]);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Area2D parent = GetParent<Area2D>();

		curve.AddPoint(parent.Position);

		while (curve.PointCount > maxPoints)
		{
			curve.RemovePoint(0);
		}

		Points = curve.GetBakedPoints();
		
		
		for (int i = 0; i < maxPoints - 1 && i < Points.Length + 1; i++)
		{
				((SegmentShape2D)tailSegments[i].Shape).A = Points[i];
				((SegmentShape2D)tailSegments[i].Shape).B = Points[i + 1];
		}
		
	}
}
