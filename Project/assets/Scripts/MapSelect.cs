using Godot;
using System;

public partial class MapSelect : Control
{
	//References to other UI elements
	private Label mapName;
	private Button leftArrow;
	private Button rightArrow;
	private TextureRect mapImage;
	private Button readyButton;

	// script related variables
	private int mapIndex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Assign References
		mapName = GetNode<Label>("%MapName");
		//readyButton = GetNode<Button>("%Ready");

		// Logic variables
		mapIndex = 0;
		// Default map name & default map image

		// Assign arrow's on click functions

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void _Change_Map(string name)
    {
        if (name == "left")
        {
            mapIndex--;
            if (mapIndex < 0)
            {
                mapIndex = 4;
            }
        }
        else if (name == "right")
        {
            mapIndex++;
            if(mapIndex < 4)
			{
				mapIndex = 0;
			}
        }
        else { return; } // break out if something is wrong

        // Update name & Update image
        String newName;
        switch (mapIndex)
        {
			case 0: { 
					newName = "Blank"; 
					// change map image

					break;
				}
			case 1: {
					newName = "Circle"; 
					break;
				}
            case 2: { 
					newName = "Compactor"; 
					break;
				}
            case 3: { 
					newName = "Moving Boxes";
					break;
				}
            case 4: { newName = "Spinning Bar"; break; }
			default: { newName = "Error!"; break; }
        }
		mapName.Text = newName;

		
    }
}
