using Godot;
using System;

public partial class EnemyBullet : Area2D
{
	[Export]
	public float Speed = 300f;

	public Vector2 Velocity;

	public override void _PhysicsProcess(double delta)
	{
		Position += Velocity * (float)delta;

		if (Position.Y > 900 || Position.Y < -100 || Position.X < -100 || Position.X > 580)
			QueueFree();
	}
}
