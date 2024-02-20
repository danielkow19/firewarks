using Godot;

namespace FireWARks.assets.Scripts;

public partial class PlayerTrail : Line2D
{
    private int maxPoints = 100;
    private Curve2D curve;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        curve = new Curve2D();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        curve.AddPoint(((Area2D)GetParent()).GetRelativeTransformToParent(GetParent().GetParent()).Origin);
        //GD.Print( "Position: " + ((Area2D)GetParent()).GetRelativeTransformToParent(GetParent().GetParent()).Origin);

        if (curve.PointCount > maxPoints)
        {
            curve.RemovePoint(0);
        }

        Points = curve.GetBakedPoints();
    }
}