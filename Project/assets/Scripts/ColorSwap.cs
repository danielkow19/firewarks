using Godot;
using System;

public partial class ColorSwap : ColorRect
{
	// Reference to child rect
	private ColorRect childRect;
	private int colorIndex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Assign references on load
		childRect = GetNode<ColorRect>("%Color");
		colorIndex = 0;

		// Assign childRects 0 color
		childRect.Color = Colors.Red;
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
		Color newColor;
		switch(colorIndex)
		{
			case 0:
				{
					// Strontium
					newColor = Colors.Red; 
					break;
				}
			case 1:
				{
					// Calcium
					newColor = Colors.Orange;
					break;
				}
			case 2:
				{
					// Sodium
					newColor = Colors.Yellow;
					break;
				}
			case 3:
				{
					// Barium
					newColor = Colors.Green;
					break;
				}
			case 4:
				{
					// Pure Copper
					// Maybe lighten the color
					newColor = Colors.Purple;
					break;
				}
			case 5:
				{
					// Strontium + Copper
					newColor = Colors.Magenta;
					break;
				}
			case 6:
				{
					// Magnesium
					newColor = Colors.Silver;
					break;
				}
			case 7:
				{
					newColor = Colors.Aquamarine;
					break;
				}
			default:
				{
					// Just incase
					newColor = Colors.White; break;
				}
		}

		// Apply the color to the colorRect
		childRect.Color = newColor;
    }
}
