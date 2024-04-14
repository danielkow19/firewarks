using System.Collections.Generic;
using System.Linq;
using Godot;

namespace FireWARks.assets.Scripts;

public partial class PlayerTrail : Line2D
{
	private Curve2D curve;
	private List<CollisionShape2D> tailSegments;
	private List<Area2D> areaSegments;
	private List<float> timeActive;
	//private Player playerRef;

	private bool isVisible = true;
	//private bool deleteFirstValue = true;

	private Area2D segment = new Area2D();
	private CollisionShape2D collisionShape = new CollisionShape2D();
	private Player playerRef;
	private Callable callable;
	//private List<float> lengths;
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerRef = GetParent<Area2D>() as Player;
		curve = new Curve2D();
		tailSegments = new List<CollisionShape2D>();
		areaSegments = new List<Area2D>();
		timeActive = new List<float>();
		//lengths = new List<float>();
		Modulate = playerRef.Modulate;
		collisionShape.Shape = new SegmentShape2D();
		segment.CollisionLayer = 2;
		callable = new Callable(this, nameof(OnAreaEntered));
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// TODO: Additional possible changes: 1. Add int to delete a value every few frames regardless of time to prevent still trail when player stops moving
		// TODO: 2. Make collisions only be on ~80% of the line to define hit box as it thins out at the end 
		
		Points = curve.GetBakedPoints(); 

		// Add new point
		curve.AddPoint(new Vector2(playerRef.Position.X + 20 * (Mathf.Cos(playerRef.Rotation) * -1), 
			playerRef.Position.Y + 20 * (Mathf.Sin(playerRef.Rotation) * -1)));
		timeActive.Add(0); // new time tracker		
		if(Points.Length > 1){
			//lengths.Add(Points.Last<Vector2>().DistanceTo(Points[Points.Length - 1]));
			Area2D newSegment = (Area2D)segment.Duplicate();
			CollisionShape2D newColShape = (CollisionShape2D)collisionShape.Duplicate();
			Shape2D newShape = (Shape2D)collisionShape.Shape.Duplicate();
			newColShape.Shape = newShape;
			((SegmentShape2D)newColShape.Shape).A = Points[^2];
			((SegmentShape2D)newColShape.Shape).B = Points[^1];	
			newSegment.AddChild(newColShape);	
			tailSegments.Add(newColShape);
			areaSegments.Add(newSegment);
			AddChild(newSegment);
			
			// Create a Callable for the OnAreaEntered method; This allows signals
			newSegment.Connect("area_entered", callable);
		}
		
		// Only start deleting if a line is drawn
		if (curve.PointCount <= 1) return;
		
		for (int i = 0; i < curve.PointCount; i++)
		{
			// last point doesn't need to be incremented
			if (i != curve.PointCount - 1)
			{
				timeActive[i] += (float)delta;
			}
		}
		
		// Time Culling
		float timeAllowed = 2.0f / (playerRef.Health + 1);
		while(timeActive[0] > timeAllowed)
		{
			curve.RemovePoint(0);
			timeActive.RemoveAt(0);
			//lengths.RemoveAt(0);
			var deleteThis1 = tailSegments[0];		
			var deleteThis2 = areaSegments[0];		
			areaSegments.RemoveAt(0);
			tailSegments.RemoveAt(0);
			deleteThis1.QueueFree();
			deleteThis2.QueueFree();
			
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

	public void ToggleTrail()
	{
		if (isVisible)
		{
			this.Hide();
		}
		else
		{
			this.Show();
		}
		isVisible = !isVisible;
	}
}
