using Godot;
using System;
using FireWARks.assets.Scripts;

public partial class randomizePatternPosition : Area2D
{
    private RandomNumberGenerator rng;

    public override void _Ready()
    {
        Node2D parent = GetParent<Node2D>();
        rng = new RandomNumberGenerator();
        
        bool doAgain = false;

        // do-while because the first time through takes it off the player
        do
        {
            parent.Position = ChoosePosition();

            foreach (Area2D area in GetOverlappingAreas())
            {
                if (area is Player)
                {
                    doAgain = true;
                    break;
                }   
            }
            
            
        } while (doAgain);
    }

    private Vector2 ChoosePosition()
    {
        return new Vector2(rng.RandfRange(-860, 860),rng.RandfRange(-480, 480));
    }
}
