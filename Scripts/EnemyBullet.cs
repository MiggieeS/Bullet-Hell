using Godot;
using System;

public partial class EnemyBullet : Area2D
{
	[Export]
	public float Speed = 200f;

	public Vector2 Velocity = Vector2.Zero;

	public override void _PhysicsProcess(double delta)
	{
		Position += Velocity * (float)delta;
		// GD.Print("Bullet moving: " + Position);

		if (Position.Y > 400 || Position.Y < -400 || Position.X < -250 || Position.X > 250)
		{
			GD.Print("Bullet freed at: " + Position);
			QueueFree();
		}
	}
}
