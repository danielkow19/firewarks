using Godot;
using System;

public partial class SceneManager : Node
{
	public Node currentScene {get; set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Viewport root = GetTree().Root;
		currentScene = root.GetChild(root.GetChildCount() - 1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ReadyScene(string path){
		ResourceLoader.LoadThreadedRequest(path);
	}

	public void GotoReadyScene(string path){
		CallDeferred(MethodName.DeferredGotoReadyScene, path);
	}

	public PackedScene GetReadyScene(string path)
	{
		if(ResourceLoader.LoadThreadedGetStatus(path) == ResourceLoader.ThreadLoadStatus.Loaded)
		{
			return (PackedScene)ResourceLoader.LoadThreadedGet(path);
		}
		else
		{
			return GetScene(path);
		}		
	}

	public void DeferredGotoReadyScene(string path)
	{
		// It is now safe to remove the current scene.
		if(currentScene != null){
			currentScene.Free();
		}
		// Load a new scene.
    	var nextScene = (PackedScene)ResourceLoader.LoadThreadedGet(path);

    	// Instance the new scene.
    	currentScene = nextScene.Instantiate();

    	// Add it to the active scene, as child of root.
    	GetTree().Root.AddChild(currentScene);

		GetTree().CurrentScene =  currentScene;
	}

	public void GoToScene(string path){
		CallDeferred(MethodName.DeferredGotoScene, path);
	}

	public PackedScene GetScene(string path)
	{
		return GD.Load<PackedScene>(path);
	}

	public void DeferredGotoScene(string path)
	{
		// It is now safe to remove the current scene.
		if(currentScene != null){
			currentScene.Free();
		}

    	// Load a new scene.
    	var nextScene = GD.Load<PackedScene>(path);

    	// Instance the new scene.
    	currentScene = nextScene.Instantiate();

    	// Add it to the active scene, as child of root.
    	GetTree().Root.AddChild(currentScene);

		GetTree().CurrentScene =  currentScene;
	}
}
