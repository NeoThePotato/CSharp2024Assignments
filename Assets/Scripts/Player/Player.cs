using UnityEngine;
using UnityEngine.Events;

namespace Player
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
	public class Player : MonoBehaviour
	{
		[SerializeField, HideInInspector] private Rigidbody2D _rb;
		[SerializeField, HideInInspector] private Collider2D _collider;
		[SerializeField] private PlayerInput _input;
		[SerializeField] private float _movementSpeed = 1;
		[SerializeField] private float _duckingMovementSpeed = 0.5f;
		[SerializeField] private float _jumpSpeed = 5;
		private event UnityAction OnCollisionEnter2DAction;
		private StateMachine _stateMachine;

		private void OnValidate()
		{
			_rb = GetComponent<Rigidbody2D>();
			_collider = GetComponent<Collider2D>();
		}

		private void Awake()
		{
			_input = new PlayerInput();
			InitFSM();
		}

		private void InitFSM()
		{
			_stateMachine = new StateMachine();

			// States
			var standbyState = new StandbyState(this);
			var movingState = new MovingState(this, _movementSpeed);
			var duckingState = new DuckingState(this, _duckingMovementSpeed);
			var jumpingState = new JumpingState(this);

			_stateMachine.AddState(standbyState);
			_stateMachine.AddState(movingState);
			_stateMachine.AddState(duckingState);
			_stateMachine.AddState(jumpingState);

			// Transitions
			_stateMachine.AddTwoWayTransition(standbyState, movingState, () => _input.Player.Move.IsPressed());
			_stateMachine.AddTwoWayTransition(standbyState, duckingState, () => _input.Player.Duck.IsPressed());
			_stateMachine.AddTwoWayTransition(movingState, duckingState, () => _input.Player.Duck.IsPressed());
			_stateMachine.AddTransition(standbyState, jumpingState, () => _input.Player.Jump.IsPressed());
			_stateMachine.AddTransition(movingState, jumpingState, () => _input.Player.Jump.IsPressed());
			OnCollisionEnter2DAction += () => _stateMachine.TriggerTransition(jumpingState, standbyState);
		}

		private void OnEnable() => _input.Enable();

		private void OnDisable() => _input.Disable();

		private void OnCollisionEnter2D(Collision2D collision) => OnCollisionEnter2DAction?.Invoke();

		private void FixedUpdate() => _stateMachine.Update();

		#region STATES
		private abstract class PlayerState : IState
		{
			public Player player;
			public PlayerState(Player player)
			{
				this.player = player;
			}

			public virtual void Update()
			{ }

			public virtual void OnEnter()
			{ }

			public virtual void OnExit()
			{ }
		}

		private class StandbyState : PlayerState
		{
			public StandbyState(Player player) : base(player)
			{ }
		}

		private class MovingState : PlayerState
		{
			protected readonly float _movementSpeed;

			public MovingState(Player player, float movementSpeed) : base(player) => _movementSpeed = movementSpeed;

			public override void Update() => player._rb.velocityX = player._input.Player.Move.ReadValue<float>() * _movementSpeed;
		}

		private class DuckingState : MovingState
		{
			private readonly Transform _playerTransform;

			public DuckingState(Player player, float movementSpeed) : base(player, movementSpeed) => _playerTransform = player.transform;

			public override void OnEnter() => _playerTransform.localScale = new(1, .5f, 1);

			public override void OnExit() => _playerTransform.localScale = Vector3.one;
		}

		private class JumpingState : PlayerState
		{
			public JumpingState(Player player) : base(player)
			{ }

			public override void OnEnter() => player._rb.AddForce(Vector2.up * player._jumpSpeed, ForceMode2D.Impulse);
		}
		#endregion
	}
}