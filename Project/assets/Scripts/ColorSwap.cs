using Godot;
using System;

public partial class ColorSwap : ColorRect
{
	// Reference to child rect
	private ColorRect childRect;
	private int colorIndex;
	public int ColorIndex {  get { return colorIndex; } }
	public Color ColorChoice { get { switch (colorIndex)
			{
                case 0:
                    {
                        // Strontium
                        return Colors.Red;
                    }
                case 1:
                    {
                        // Calcium
                        return Colors.Orange;
                    }
                case 2:
                    {
                        // Sodium
                        return Colors.Yellow;
                    }
                case 3:
                    {
                        // Barium
                        return Colors.Green;
                    }
                case 4:
                    {
                        // Pure Copper
                        // Maybe lighten the color
                        return Colors.Purple;
                    }
                case 5:
                    {
                        // Strontium + Copper
                        return Colors.Magenta;
                    }
                case 6:
                    {
                        // Magnesium
                        return Colors.Silver;
                    }
                case 7:
                    {
                        return Colors.Aquamarine;
                    }
                default:
                    {
                        // Just incase
                        return Colors.White;
                    }
            } } }
	// Reference to the Player preview Sprite
	[Export]
	private AnimatedSprite2D sprite;
	
	//private TextureRect texture;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Assign references on load
		childRect = GetNode<ColorRect>("%Color");
		colorIndex = 0;

		// Assign childRects 0 color
		childRect.Color = Colors.Red;
		sprite.Modulate = Colors.Red;
		//texture.Modulate = Colors.Red;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	// Checks input from arrows, then applies a color change
	public void _ChangeColor(string arrowName)
	{
		if(arrowName == "left") {
			// decrease number
			colorIndex--;
			// See if number needs to loop
			if(colorIndex < 0)
			{
				colorIndex = 7;
			}
		}
        else if(arrowName == "right")
		{
			// increase number
			colorIndex++;
			// See if number needs to loop
			if(colorIndex > 7)
			{
				colorIndex = 0;
			}
		}
		else
        {
			// break out if input isn't correct
			return;
        }

		// Choose Colors
		Color newColor = ColorChoice;

		// Apply the color to the colorRect
		childRect.Color = newColor;
		sprite.Modulate = newColor;
		//texture.Modulate = newColor;
    }
}
