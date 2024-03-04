using System.Collections.Generic;
using Godot;

namespace FireWARks.assets.Scripts;

public partial class PlayerTrail : Line2D
{
	private int maxPoints = 100;
	private Curve2D curve;
	private List<CollisionShape2D> tailSegments;
	private Player playerRef;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerRef = GetParent<Player>();
		curve = new Curve2D();
		tailSegments = new List<CollisionShape2D>();

		for (int i = 0; i < maxPoints - 1; i++)
		{
			// Temp, doesn't need to be saved in class but will in scene
			Area2D segment = new Area2D();
			CollisionShape2D collisionShape = new CollisionShape2D();
			collisionShape.Shape = new SegmentShape2D();
			segment.AddChild(collisionShape);
			segment.CollisionLayer = 2;
				
			tailSegments.Add(collisionShape);
			((SegmentShape2D)tailSegments[i].Shape).A = Position;
			((SegmentShape2D)tailSegments[i].Shape).B = Position;
			
			// Set up signals for the area2Ds so they don't need to be saved
			AddChild(segment);
			
			// Create a Callable for the OnAreaEntered method; This allows signals
			Callable callable = new Callable(this, nameof(OnAreaEntered));
			segment.Connect("area_entered", callable);
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
		
		
		for (int i = 0; i < maxPoints - 1 && i < Points.Length - 1; i++)
		{
			((SegmentShape2D)tailSegments[i].Shape).A = Points[i];
			((SegmentShape2D)tailSegments[i].Shape).B = Points[i + 1];
		}
		
	}
	
	private void OnAreaEntered(Area2D area)
	{
		// Check if the area that entered is a player
		if (area is Player player && player != playerRef)
		{
			player.DamagePlayer(1);
		}
	}
}
