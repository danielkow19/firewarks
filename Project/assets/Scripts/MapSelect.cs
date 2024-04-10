using Godot;
using System;

public partial class MapSelect : Control
{
	//References to other UI elements
	private string mapName;
	private TextureRect mapImage;
	private Button readyButton;
	private ArrowMap leftButton;
	private ArrowMap rightButton;

	// script related variables
	private int mapIndex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Assign References
		readyButton = GetNode<Button>("%Ready");
		mapImage = GetNode<TextureRect>("%Image");
		leftButton = GetNode<ArrowMap>("%Left");
		rightButton = GetNode<ArrowMap>("%Right");

		readyButton.Pressed += _on_ready_pressed;


		// Logic variables
		mapIndex = 0;

		// Default map name & default map image
		mapName = "Blank";

        SceneManager scene = GetNode<SceneManager>("/root/SceneManager");
		scene.ReadyScene("res://player_select.tscn");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_left"))
		{
			leftButton._on_pressed();
		}
		else if (Input.IsActionJustPressed("ui_right"))
		{
			rightButton._on_pressed();
		}
		else if (Input.IsActionJustPressed("ui_select"))
		{
			_on_ready_pressed();
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
						 // Default them to the blank map
		
		Image img = ResourceLoader.Load<CompressedTexture2D>("res://assets/sprites/map-images/blankMap.PNG").GetImage();
        ImageTexture imageTexture = ImageTexture.CreateFromImage(img);
        // Update name & Update image
        switch (mapIndex)
        {
			case 0: { 
					mapName = "blank";
					// change map image
					// Create an image
					img = ResourceLoader.Load<CompressedTexture2D>("res://assets/sprites/map-images/blankMap.PNG").GetImage();
                    imageTexture.Update(img);
                    break;
				}
			case 1: {
					mapName = "circle";
                    img = ResourceLoader.Load<CompressedTexture2D>("res://assets/sprites/map-images/circleMap.PNG").GetImage();
                    imageTexture.Update(img);
                    break;
				}
            case 2: { 
					mapName = "compactor";
                    img = ResourceLoader.Load<CompressedTexture2D>("res://assets/sprites/map-images/compactorMap.PNG").GetImage();
                    imageTexture.Update(img);
                    break;
				}
            case 3: { 
					mapName = "boxes";
                    img = ResourceLoader.Load<CompressedTexture2D>("res://assets/sprites/map-images/boxesMap.PNG").GetImage();
                    imageTexture.Update(img);
                    break;
				}
            case 4: { 
					mapName = "bar";
                    img = ResourceLoader.Load<CompressedTexture2D>("res://assets/sprites/map-images/barMap.PNG").GetImage();
                    imageTexture.Update(img);
                    break;
				}
			default: { 
					mapName = "Error!";
					//img = Image.LoadFromFile("res://assets/sprites/bullets/warning mockup.png");

                    break; }
        }
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
		scene.GotoReadyScene("res://player_select.tscn");
	}
}
