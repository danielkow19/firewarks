using Godot;
using System;

public partial class MapSelect : Control
{
	//References to other UI elements
	private string mapName;
	private TextureRect mapImage;
	private Button readyButton;

	// script related variables
	private int mapIndex;

	// Temp Variable
	private Label mapLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Assign References
		readyButton = GetNode<Button>("%Ready");
		readyButton.Pressed += _on_ready_pressed;

		// TEMP
		mapLabel = GetNode<Label>("%MapName");

		// Logic variables
		mapIndex = 0;

		// Default map name & default map image
		mapName = "Blank";

		// Assign arrow's on click functions

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void _Change_Map(string direction)
    {
        if (direction == "left")
        {
            mapIndex--;
            if (mapIndex < 0)
            {
                mapIndex = 4;
            }
        }
        else if (direction == "right")
        {
            mapIndex++;
            if(mapIndex > 4)
			{
				mapIndex = 0;
			}
        }
        else { return; } // break out if something is wrong

        // Update name & Update image
        switch (mapIndex)
        {
			case 0: { 
					mapName = "blank";
					// change map image

                    break;
				}
			case 1: {
					mapName = "circle"; 
					break;
				}
            case 2: { 
					mapName = "compactor"; 
					break;
				}
            case 3: { 
					mapName = "boxes";
					break;
				}
            case 4: { 
					mapName = "bar";
					break;
				}
			default: { mapName = "Error!"; break; }
        }
		mapLabel.Text = mapName;
		
    }
	private void _on_ready_pressed()
	{
		player_settings settings = (player_settings)GetNode("/root/PlayerSettings");
		//make sure the name is lowercase
		settings.SaveMap(mapName.ToLower());
		GD.Print($"Map Name: {mapName}");

		// Change scenes
		SceneManager scene = GetNode<SceneManager>("/root/SceneManager");
		scene.GoToScene("res://Game.tscn");
	}
}
