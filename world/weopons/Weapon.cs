using Godot;
using System;

public partial class Weapon : Node3D
{
	
	[Export] public Weapons WeoponData;

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
