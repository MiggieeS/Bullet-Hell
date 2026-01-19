using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public float Speed = 300f;

	[Export]
	public PackedScene BulletScene;

	[Export]
	public float FireRate = 0.15f;

	private float _fireCooldown = 0f;
	private Marker2D _bulletSpawn;

	public override void _Ready()
	{
		_bulletSpawn = GetNode<Marker2D>("BulletSpawn");

		// Connect Hitbox signal manually
		Area2D hitbox = GetNode<Area2D>("Hitbox");
		hitbox.AreaEntered += OnHitboxAreaEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 input = Vector2.Zero;

		if (Input.IsActionPressed("move_left"))
			input.X -= 1;
		if (Input.IsActionPressed("move_right"))
			input.X += 1;
		if (Input.IsActionPressed("move_up"))
			input.Y -= 1;
		if (Input.IsActionPressed("move_down"))
			input.Y += 1;

		input = input.Normalized();
		Velocity = input * Speed;
		MoveAndSlide();

		HandleShooting(delta);
	}

	private void HandleShooting(double delta)
	{
		_fireCooldown -= (float)delta;

		if (Input.IsActionPressed("shoot") && _fireCooldown <= 0f)
		{
			_fireCooldown = FireRate;

			if (BulletScene != null)
			{
				PlayerBullet bullet = BulletScene.Instantiate<PlayerBullet>();
				bullet.Position = _bulletSpawn.GlobalPosition;
				GetTree().CurrentScene.AddChild(bullet);
			}
		}
	}

	// ✅ Signal callback for Hitbox collisions
	private void OnHitboxAreaEntered(Area2D area)
	{
		GD.Print("Player hit by: " + area.Name);

		// Example: check specifically for EnemyBullet
		if (area is EnemyBullet)
		{
			GD.Print("Player was hit by an EnemyBullet!");
			// TODO: reduce health, trigger death, etc.
		}
	}
}
