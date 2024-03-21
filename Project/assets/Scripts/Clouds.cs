using System;
using Godot;
using Godot.Collections;

namespace FireWARks.assets.Scripts;

public partial class Clouds : Area2D
{
	[Export(PropertyHint.Enum, "Up,Down,Left,Right,UpAndLeft,UpAndRight,DownAndLeft,DownAndRight,None")]
	public string[] directions = new string[0];
	[Export]
	public double[] speed = new double[0];
	[Export]
	public double[] rotations = new double[0];
	[Export]
	public double[] scaleHolder = new double[0];
	[Export]
	public double[] timersHolder = new double[0];
	[Export]
	public bool boomerang = false;
	[Export]
	public bool loop = false;
	private int step = 0;
	private Vector2 start;
	private bool reverseTracking = false;
	public int stepCount;
	private double[] timers;
	private double lastScale = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		start = Position;
		step = 0;
		if(directions.Length == 0)
		{
			directions = new string[1];
			directions[0] = "None";
		}
		stepCount = directions.Length;
		if(speed.Length < directions.Length)
		{
			double[] update = new double[stepCount];
			for(int i = 0; i < stepCount; i++){
				if(i < speed.Length)
				{
					update[i] = speed[i];
				}
				else
				{
					if(i == 0)
					{
						update[i] = 0;
					}
					else{
						update[i] = update[i-1];
					}
				}
			}
			speed = update;
		}
		if(rotations.Length < directions.Length)
		{
			double[] update = new double[stepCount];
			for(int i = 0; i < stepCount; i++){
				if(i < rotations.Length)
				{
					update[i] = rotations[i];
				}
				else
				{
					update[i] = 0;
				}
			}
			rotations = update;
		}
		if(timersHolder.Length < directions.Length)
		{
			double[] update = new double[stepCount];
			for(int i = 0; i < stepCount; i++){
				if(i < timersHolder.Length)
				{
					update[i] = timersHolder[i];
				}
				else
				{
					update[i] = 0;
				}
			}
			timersHolder = update;
		}
		if(scaleHolder.Length < directions.Length)
		{
			double[] update = new double[stepCount];
			for(int i = 0; i < stepCount; i++){
				if(i < scaleHolder.Length)
				{
					update[i] = scaleHolder[i];
				}
				else
				{
					if(i == 0){update[i] = 1;}
					else{update[i] = update[i-1];}
				}
			}
			scaleHolder = update;
		}
		timers = new double[timersHolder.Length];
		setTimers();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print(timersHolder[step]);
		Vector2 movingVec = new Vector2(0,0);
		
		if((directions[step] == "Up" && !reverseTracking) || (directions[step] == "Down" && reverseTracking)){
			movingVec = new Vector2(0,-(float)speed[step] * (float)delta);
		}
		else if((directions[step] == "Down" && !reverseTracking) || (directions[step] == "Up" && reverseTracking)){
			movingVec = new Vector2(0,(float)speed[step] * (float)delta);
		}
		else if((directions[step] == "Left" && !reverseTracking) || (directions[step] == "Right" && reverseTracking)){
			movingVec = new Vector2(-(float)speed[step] * (float)delta, 0);
		}
		else if((directions[step] == "Right" && !reverseTracking) || (directions[step] == "Left" && reverseTracking)){
			movingVec = new Vector2((float)speed[step] * (float)delta, 0);
		}
		else if((directions[step] == "UpAndRight" && !reverseTracking) || (directions[step] == "DownAndLeft" && reverseTracking)){
			movingVec = new Vector2(MathF.Sqrt((float)speed[step]) * (float)delta,-MathF.Sqrt((float)speed[step]) * (float)delta);
		}
		else if((directions[step] == "UpAndLeft" && !reverseTracking) || (directions[step] == "DownAndRight" && reverseTracking)){
			movingVec = new Vector2(-MathF.Sqrt((float)speed[step]) * (float)delta,-MathF.Sqrt((float)speed[step]) * (float)delta);
		}
		else if((directions[step] == "DownAndRight" && !reverseTracking) || (directions[step] == "UpAndLeft" && reverseTracking)){
			movingVec = new Vector2(MathF.Sqrt((float)speed[step])* (float)delta,MathF.Sqrt((float)speed[step])* (float)delta);
		}
		else if((directions[step] == "DownAndLeft" && !reverseTracking) || (directions[step] == "UpAndRight" && reverseTracking)){
			movingVec = new Vector2(-MathF.Sqrt((float)speed[step])* (float)delta,MathF.Sqrt((float)speed[step])* (float)delta);
		}		
		if(Rotation > 2*Math.PI)
		{
			Rotate(-2*(float)Math.PI);
		}
		Rotate((float)rotations[step]/(float)timersHolder[step] * (float)delta * (float)Math.PI/180);
		//uncomment this line if translation is being rotated
		//movingVec = movingVec.Rotated(Rotation);
		Translate(movingVec);
		Scale = new Vector2(Scale.X+((float)(scaleHolder[step]-lastScale)/(float)timersHolder[step]*(float)delta),Scale.Y+((float)(scaleHolder[step]-lastScale)/(float)timersHolder[step]*(float)delta));
		timers[step] -= delta;

		if(timers[step] <= 0){
			Scale = new Vector2((float)scaleHolder[step],(float)scaleHolder[step]);
			lastScale = scaleHolder[step];
			if(!reverseTracking)
			{
				step += 1;
				if(step == stepCount && !boomerang)
				{
					step = 0;
					setTimers();
					if(!loop){
						Position = start;
					}
				}
				else if(step == stepCount && boomerang)
				{
					step = stepCount;
					setTimers();
					reverseTracking = true;
				}

			}
			else{
				step -= 1;
				if(step == 0)
				{
					step = 0;
					setTimers();
					reverseTracking = false;
				}
			}
			
		}
	}
	
	private void _on_area_entered(Area2D area)
	{
		if (area is Player player)
		{
			player.numClouds++;
		}
	}
	
	private void _on_area_exited(Area2D area)
	{
		if (area is Player player)
		{
			player.numClouds--;
		}
	}

	private void setTimers()
	{
		for(int i = 0; i < timersHolder.Length;i++){
			timers[i] = timersHolder[i];
		}
	}
}


