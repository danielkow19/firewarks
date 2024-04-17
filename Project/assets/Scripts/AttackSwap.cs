using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class AttackSwap : Label
{
	private int playerNum;
	private int attackNum;
	private SelectMenu menuController;
	private int attackIndex;
	public int AttackIndex { get { return attackIndex; } }
	public string attackName
	{
		get
		{
			switch (attackIndex)
			{
				case 0: { return "Circle Burst"; }
				case 1: { return "Fast Spreadshot"; }
				case 2: { return "Knot"; }
				case 3: { return "Spreadshot"; }
				case 4: { return "Swirl"; }
				case 5: { return "Weave"; }
				case 6: { return "Willow"; }
				default: { return "Error"; }
			}
		}
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        playerNum = GetNode<CursorAlt>("%Cursor").playerNum;
        player_settings settings = GetNode<player_settings>("/root/PlayerSettings");
		if(settings.PlayerInfos.Count > playerNum)
		{
			GD.Print("Entered IF");
            PlayerInfo info = new PlayerInfo();
            for (int i = 0; i < settings.PlayerInfos.Count; i++)
            {
                if (playerNum == settings.PlayerInfos[i].PlayerID)
                {
                    info = settings.PlayerInfos[i];
                }
            }
            //Set and show initial selected options
            menuController = GetNode<SelectMenu>("../..");
            if (this.Name.ToString().Contains("1"))
            {
                attackNum = 1;
                menuController.attackIndices[attackNum - 1] = 0;
				attackIndex = getIndexFromPattern(info.LeftPattern);
				this.Text = attackName;
            }
            else if (this.Name.ToString().Contains("2"))
            {
                attackNum = 2;
                menuController.attackIndices[attackNum - 1] = 1;
				attackIndex = getIndexFromPattern(info.RightPattern);
            }
            else
            {
                Debug.Print("Ah this broke!");
            }
        }
		else
		{
			GD.Print("Default");
            //Set and show initial selected options
            menuController = GetNode<SelectMenu>("../..");
            if (this.Name.ToString().Contains("1"))
            {
                attackNum = 1;
                menuController.attackIndices[attackNum - 1] = 0;
                attackIndex = 0;
            }
            else if (this.Name.ToString().Contains("2"))
            {
                attackNum = 2;
                menuController.attackIndices[attackNum - 1] = 1;
                attackIndex = 1;
            }
            else
            {
                Debug.Print("Ah this broke!");
            }
        }
		

	}

	//called when adjusted left or right selects the adjacent attack and displays
	public void _Change_Attack(string arrowName)
	{
		if (arrowName == "left")
		{
			// Decrease
			attackIndex--;
			if (attackIndex < 0)
			{
				// Loop to top
				attackIndex = 6;
			}
			if (menuController.attackIndices[attackNum % 2] == attackIndex) {
				_Change_Attack("left");
			}
			menuController.attackIndices[attackNum - 1] = attackIndex;
		}
		else if (arrowName == "right")
		{
			// Increase
			attackIndex++;
			if (attackIndex > 6) //Change number later
			{
				// Loop to bottom
				attackIndex = 0;
			}
			if (menuController.attackIndices[attackNum % 2] == attackIndex) {
				_Change_Attack("right");
			}
			menuController.attackIndices[attackNum - 1] = attackIndex;
		}
		else { return; } //Break out if something is wrong

		// Update the name
		String labelName = "ERROR";
		switch(attackIndex)
		{
			case 0:
				{
					labelName = "Circle Burst";
					break;
				}
			case 1:
				{
					labelName = "Fast Spreadshot";
                    break;
				}
			case 2:
				{
					labelName = "Knot";
                    break;
				}
			case 3:
				{
					labelName = "Spreadshot";
					break;
				}
			case 4:
				{
					labelName = "Swirl";
                    break;
				}
			case 5:
				{
					labelName = "Weave";
					break;
                }
			case 6:
				{
					labelName = "Willow";
					break;
                }
		}
		// Apply name to label
		this.Text = labelName;
	}

	private int getIndexFromPattern(PackedScene pattern)
	{
		switch (pattern.ResourcePath)
		{
			case "res://assets/prefabs/PatternCircleBurst.tscn":
            {
				return 0;
			}
			case "res://assets/prefabs/PatternFastSS.tscn":
				{
					return 1;
				}
			case "res://assets/prefabs/PatternKnot.tscn":
				{
					return 2;
				}
			case "res://assets/prefabs/PatternSpreadShot.tscn":
				{
					return 3;
				}
			case "res://assets/prefabs/PatternSwirl.tscn":
				{
					return 4;
				}
			case "res://assets/prefabs/PatternWeave.tscn":
				{
					return 5;
				}
			case "res://assets/prefabs/PatternWillow.tscn":
				{
					return 6;
				}
			default: { return 0; }
        }
	}
}
