using Godot;
using System;

public partial class Enemy : Area2D
{
	[Export]
	public PackedScene BulletScene;

	[Export]
	public float FireInterval = 0.6f;

	[Export]
	public int BulletsPerWave = 24;

	private float _timer = 0f;
	private float _angleOffset = 0f;

	// Pattern switching
	private Action _currentPattern;
	private float _patternTimer = 0f;
	private RandomNumberGenerator _rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		AreaEntered += OnEnemyAreaEntered;

		_rng.Randomize();
		_currentPattern = FireCirclePattern;
		_patternTimer = _rng.RandfRange(5f, 10f);
	}

	public override void _PhysicsProcess(double delta)
	{
		_timer -= (float)delta;
		_patternTimer -= (float)delta;

		// Switch pattern randomly every few seconds
		if (_patternTimer <= 0f)
		{
			_patternTimer = _rng.RandfRange(5f, 10f);

			int choice = _rng.RandiRange(0, 3);
			switch (choice)
			{
				case 0: _currentPattern = FireCirclePattern; GD.Print("Pattern: Circle"); break;
				case 1: _currentPattern = FireSpiralPattern; GD.Print("Pattern: Spiral"); break;
				case 2: _currentPattern = FireWavePattern; GD.Print("Pattern: Wave"); break;
				case 3: _currentPattern = FireAimedShot; GD.Print("Pattern: Aimed"); break;
			}
		}

		if (_timer <= 0f)
		{
			_timer = FireInterval;
			_currentPattern?.Invoke();
		}
	}

	// Bullet Patterns
	private void FireCirclePattern()
	{
		if (BulletScene == null) return;

		float angleStep = Mathf.Tau / BulletsPerWave;

		for (int i = 0; i < BulletsPerWave; i++)
		{
			float angle = _angleOffset + angleStep * i;
			Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

			EnemyBullet bullet = BulletScene.Instantiate<EnemyBullet>();
			bullet.Position = GlobalPosition;
			bullet.Velocity = dir * bullet.Speed;

			GetTree().CurrentScene.AddChild(bullet);
		}

		_angleOffset += 0.3f; // slowly rotate the circle
	}

	private void FireSpiralPattern()
	{
		if (BulletScene == null) return;

		float angle = _angleOffset;
		Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

		EnemyBullet bullet = BulletScene.Instantiate<EnemyBullet>();
		bullet.Position = GlobalPosition;
		bullet.Velocity = dir * bullet.Speed;

		GetTree().CurrentScene.AddChild(bullet);

		_angleOffset += 0.8f; // tighter spiral
	}

	private void FireWavePattern()
	{
		if (BulletScene == null) return;

		int waveCount = 10;
		for (int i = 0; i < waveCount; i++)
		{
			float offset = Mathf.Sin(_angleOffset + i * 0.5f) * 100f;
			EnemyBullet bullet = BulletScene.Instantiate<EnemyBullet>();
			bullet.Position = GlobalPosition + new Vector2(offset, 0);
			bullet.Velocity = Vector2.Down * bullet.Speed * 2;

			GetTree().CurrentScene.AddChild(bullet);
		}

		_angleOffset += 0.5f;
	}

	private void FireAimedShot()
	{
		if (BulletScene == null) return;

		Node2D player = GetTree().CurrentScene.GetNode<Node2D>("Player");
		Vector2 dir = (player.GlobalPosition - GlobalPosition).Normalized();

		EnemyBullet bullet = BulletScene.Instantiate<EnemyBullet>();
		bullet.Position = GlobalPosition;
		bullet.Velocity = dir * bullet.Speed * 5;

		GetTree().CurrentScene.AddChild(bullet);
	}

	private void OnEnemyAreaEntered(Area2D area)
	{
		GD.Print("Player hit by: " + area.Name);

		// onhit logic here 
	}
}
