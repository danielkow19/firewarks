using Godot;
using System;

public partial class MessagePopup : Marker2D
{
	public void Popup(string message, Vector2 position)
	{
		GlobalPosition = position;
		GetChild<Label>(0).Text = message;
		GetChild<AnimationPlayer>(1).Play();
	}
}

