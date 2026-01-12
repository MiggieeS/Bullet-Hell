using Godot;
using System;

public partial class PlayerBullet : Area2D
{
	[Export]
	public float Speed = 600f; // double the player's speed

	public Vector2 Direction = new Vector2(0, -1);

	public override void _PhysicsProcess(double delta)
	{

		Position += Direction * Speed * (float)delta;

	}
}
