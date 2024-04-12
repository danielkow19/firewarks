using Godot;
using System;
using System.Diagnostics;

public partial class Pause : Control
{
	[Export]
	private Node _manager;
	private Button lobbyButton;
	private Button mainMenuButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Button button = GetNode<Button>("MarginContainer/VBoxContainer/Resume");
		button.GrabFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public void _on_quit_pressed()
	{
		GetTree().Quit();
	}
	public void _on_resume_pressed()
	{
		_manager.Call("PauseMenu");
	}
	public void _on_lobby_pressed()
	{
		// CHECK WITH CONNOR THAT WE CAN GO TO NON-READIED SCENES
		SceneManager scene = (SceneManager)GetNode("/root/SceneManager");
		scene.ReadyScene("res://player_select.tscn");
		scene.GotoReadyScene("res://player_select.tscn");

    }
	public void _on_main_menu_pressed()
	{
		// CHECK WITH CONNOR THAT WE CAN GO TO NON-READIED SCENES
		SceneManager scene = (SceneManager)GetNode("/root/SceneManager");
		scene.ReadyScene("res://StartMenu.tscn");
        scene.GotoReadyScene("res://StartMenu.tscn");
	}
}
