using Godot;

namespace FireWARks.assets.Scripts;

public partial class PlayerTrail : Line2D
{
	private int maxPoints = 100;
	private Curve2D curve;
	
	
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

		curve.AddPoint(parent.Position);

		if (curve.PointCount > maxPoints)
		{
			curve.RemovePoint(0);
		}

		Points = curve.GetBakedPoints();
	}
}
