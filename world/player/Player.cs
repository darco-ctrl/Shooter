using Godot;
using System;
using System.Dynamic;

public partial class Player : CharacterBody3D
{

	// PLAYER MOVEMENT VARIABLES //
	[Export] public float Speed = 5.0f;
	[Export] public float JumpVelocity = 4.5f;

	
	// PLAYER CAMERA VARIABLES //
	[Export] public float Sensitivity = 0.005f;

	const float BOB_FREQUENCY = 2.0f;
	const float BOB_AMPLITUDE = 0.08f;
	private float BobTime = 0f;

	public bool CameraIsLocked = false;
	
	// SCENE NODES //
	[Export] public Node3D Head;
	[Export] public Camera3D Camera;

    public override void _Process(double delta)
    {
        Misc();
    }

	public override void _PhysicsProcess(double delta)
	{
		PlayerMovements((float)delta);
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion)
		{
			PlayerCamera((InputEventMouseMotion)@event);
		}
    }


	// -- HANDLE CAMERA MOTION -- //
	private void PlayerCamera(InputEventMouseMotion mouseMotionEvent)
	{
		if (!CameraIsLocked) return;

		Head.RotateY(-mouseMotionEvent.Relative.X * Sensitivity);
		Camera.RotateX(-mouseMotionEvent.Relative.Y * Sensitivity);

		Vector3 cameraRotation = Camera.Rotation;
		cameraRotation.Y = Math.Clamp(Camera.Rotation.Y, Mathf.DegToRad(-40), Mathf.DegToRad(60));

		Camera.Rotation = cameraRotation;
	}

	private Vector3 HeadBob(float time)
	{
		var position = Vector3.Zero;

		position.Y = Mathf.Sin(time * BOB_FREQUENCY) * BOB_AMPLITUDE;
		position.X = Mathf.Cos(time * BOB_FREQUENCY / 2) * BOB_AMPLITUDE;

		return position;
	}

	// -- HANDLE PLAYER MOVEMENT -- //
	private void PlayerMovements(float delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");

		Vector3 direction = (Head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = 0f;
			velocity.Z = 0f;
		}

		BobTime += delta * velocity.Length() * (IsOnFloor() ? 1 : 0);

		Transform3D t = Camera.Transform;
		t.Origin = HeadBob(BobTime);
		Camera.Transform = t;

		Velocity = velocity;
		MoveAndSlide();
	}

	// MISC //
	private void Misc()
	{
		if (Input.IsActionJustPressed("CameraLock"))
		{
			if (CameraIsLocked)
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
				CameraIsLocked = false;
			} else
			{
				Input.MouseMode = Input.MouseModeEnum.Captured;
				CameraIsLocked = true;
			}
		}
	}
}
