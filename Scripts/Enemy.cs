using Godot;
using System;

public partial class Enemy : Node2D
{
	[Export]
	public PackedScene BulletScene;

	[Export]
	public float FireInterval = 0.6f;

	[Export]
	public int BulletsPerWave = 24;

	private float _timer = 0f;
	private float _angleOffset = 0f;

	public override void _PhysicsProcess(double delta)
	{
		_timer -= (float)delta;
		// GD.Print("Enemy firing check: timer = " + _timer);

		if (_timer <= 0f)
		{
			_timer = FireInterval;
			FireCirclePattern();
		}
	}

	private void FireCirclePattern()
	{

		if (BulletScene == null)
			return;

		float angleStep = Mathf.Tau / BulletsPerWave;

		for (int i = 0; i < BulletsPerWave; i++)
		{
			float angle = _angleOffset + angleStep * i;
			Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

			EnemyBullet bullet = BulletScene.Instantiate<EnemyBullet>();
			bullet.Position = GlobalPosition;
			bullet.Velocity = dir * bullet.Speed;

			GetTree().CurrentScene.AddChild(bullet);
			// GD.Print("Bullet spawned with velocity: " + bullet.Velocity); 	
		}

		_angleOffset += 0.15f; // slowly rotate the pattern
	}
}
