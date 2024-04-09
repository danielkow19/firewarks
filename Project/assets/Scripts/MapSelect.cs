using Godot;
using System;

public partial class MapSelect : Control
{
	//References to other UI elements
	private string mapName;
	private TextureRect mapImage;
	private Button readyButton;
	private Button leftButton;

	// script related variables
	private int mapIndex;
	private bool startFocus;

	// Temp Variable
	private Label mapLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Assign References
		readyButton = GetNode<Button>("%Ready");
		mapImage = GetNode<TextureRect>("%Image");
		leftButton = GetNode<Button>("%Left");

		readyButton.Pressed += _on_ready_pressed;

		// TEMP
		//mapLabel = GetNode<Label>("%MapName");

		// Logic variables
		mapIndex = 0;
		startFocus = false;

		// Default map name & default map image
		mapName = "Blank";

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(!startFocus)
		{
			leftButton.GrabFocus();
			startFocus = true;
		}
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
		// Create an Image & ImageTexture
		// Default them to the blank ma
		Image img = Image.LoadFromFile("res://assets/sprites/map-images/blankMap.png");
		ImageTexture imageTexture = ImageTexture.CreateFromImage(img);
        // Update name & Update image
        switch (mapIndex)
        {
			case 0: { 
					mapName = "blank";
					// change map image
					// Create an image
					img = Image.LoadFromFile("res://assets/sprites/map-images/blankMap.png");
                    imageTexture.Update(img);
                    break;
				}
			case 1: {
					mapName = "circle";
                    img = Image.LoadFromFile("res://assets/sprites/map-images/circleMap.png");
					imageTexture.Update(img);
                    break;
				}
            case 2: { 
					mapName = "compactor";
                    img = Image.LoadFromFile("res://assets/sprites/map-images/compactorMap.png");
                    imageTexture.Update(img);
                    break;
				}
            case 3: { 
					mapName = "boxes";
                    img = Image.LoadFromFile("res://assets/sprites/map-images/boxesMap.png");
					imageTexture.Update(img);
                    break;
				}
            case 4: { 
					mapName = "bar";
                    img = Image.LoadFromFile("res://assets/sprites/map-images/barMap.png");
					imageTexture.Update(img);
                    break;
				}
			default: { 
					mapName = "Error!";
					//img = Image.LoadFromFile("res://assets/sprites/bullets/warning mockup.png");

                    break; }
        }
		//mapLabel.Text = mapName;
		mapImage.Texture = imageTexture;
    }
	private void _on_ready_pressed()
	{
		player_settings settings = (player_settings)GetNode("/root/PlayerSettings");
		//make sure the name is lowercase
		settings.SaveMap(mapName.ToLower());
		GD.Print($"Map Name: {mapName}");

		// Change scenes
		SceneManager scene = GetNode<SceneManager>("/root/SceneManager");
		scene.GoToScene("res://player_select.tscn");
	}
}
