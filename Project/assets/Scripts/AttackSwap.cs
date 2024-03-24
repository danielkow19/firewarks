using Godot;
using System;

public partial class AttackSwap : Label
{
	private int attackIndex;
	public int AttackIndex { get { return attackIndex; } }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		attackIndex = 0;
	}

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
}
