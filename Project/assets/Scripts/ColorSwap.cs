using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class ColorSwap : ColorRect
{
	// Reference to child rect
	private int playerNum;
	private player_select sceneController;
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
                        return Colors.Magenta;
                    }
				case 5:
					{
                        // Strontium + Copper
                        return Colors.MediumVioletRed;
					}
				case 6:
					{
						// Magnesium
						return Colors.Lavender;

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
	public AnimatedSprite2D sprite;
	
	//private TextureRect texture;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sceneController = GetNode<player_select>("../..");
		playerNum = GetNode<CursorAlt>("%Cursor").playerNum;

		// Assign references on load
		childRect = GetNode<ColorRect>("%Color");
		sprite = GetNode<AnimatedSprite2D>("%FlyingSprite");

        //Load in info from player settings
        player_settings settings = (player_settings)GetNode("/root/PlayerSettings");
		// Get player info
		if(settings.PlayerInfos.Count > playerNum)
		{
			try
			{
				PlayerInfo info = new PlayerInfo();
                for (int i =0; i < settings.PlayerInfos.Count; i++)
				{
					if(playerNum == settings.PlayerInfos[i].PlayerID)
					{
						info = settings.PlayerInfos[i];
					}
				}
                colorIndex = _getIndexFromColor(info.Color);
            }
            catch (Exception e)
			{
				GD.Print($"Had an exception: {e}");
				colorIndex = 0;
			}

			// Assign color info
        }
        else
        {
			// Assign childRects color 0
			colorIndex = 0;
        }

		childRect.Color = ColorChoice;
		sprite.Modulate = ColorChoice;
		//texture.Modulate = Colors.Red;
		//Debug.Print($"Hello? {sceneController.colorIndices[playerNum]}");
		//Debug.Print($"Hello? {sceneController.colorIndices.Contains(colorIndex)}");
		if(sceneController.colorIndices.Contains(colorIndex)) {
			
			_ChangeColor("right");
		} else {
			sceneController.colorIndices[playerNum] = colorIndex;
		}
		//Debug.Print($"Hello? {sceneController.colorIndices[playerNum]}");
		//Debug.Print($"Hello? {sceneController.colorIndices.Contains(colorIndex)}");
		
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
			if(sceneController.colorIndices.Contains(colorIndex)) {
				_ChangeColor("left");
			}
			sceneController.colorIndices[playerNum] = colorIndex;
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
			if(sceneController.colorIndices.Contains(colorIndex)) {
				_ChangeColor("right");
			}
			sceneController.colorIndices[playerNum] = colorIndex;
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

	private int _getIndexFromColor(Color color)
	{
		// A Switch statement was causing problems with Color Constant Names
		if(color == Colors.Red)
		{
			return 0;
		}
		else if(color == Colors.Orange)
		{
			return 1;
		}
		else if(color == Colors.Yellow)
		{
			return 2;
		}
		else if(color == Colors.Green)
		{
			return 3;
		}
		else if (color == Colors.Magenta)
		{
			return 4;
		}
		else if(color == Colors.MediumVioletRed)
		{
			return 5;
		}
		else if (color == Colors.Lavender)
		{
			return 6;
		}
		else if(color == Colors.Aquamarine)
		{
			return 7;
		}
		else
		{
			return -1;
		}
	}
}
