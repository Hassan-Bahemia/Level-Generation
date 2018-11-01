using UnityEngine;

public class Move : MonoBehaviour
{
    public float MaxRunningSpeed = 1.1f;
    public float RunningForce = 9f;
    public float JumpForce = 165f;
    public MovementState CurrentState = MovementState.Falling;

    private Rigidbody2D _rigidBody;
    private SpriteRenderer _renderer;
    private Animator _animator;

    public enum MovementState
    {
        Idle, Running, Jumping, Falling
    }

    // Get references at the start of the game.
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Get input from the player.
        var horizontal = 0;
        var vertical = 0;

        if (Input.GetKey(KeyCode.A)) horizontal = -1;
        else if (Input.GetKey(KeyCode.D)) horizontal = 1;

        if (Input.GetKey(KeyCode.Space)) vertical = 1;

        // Check if the player can jump.
        // Checks if the current y velocity is equal to zero, and that the current state is either idle or running,
        // and that you actually pressed space.
        if (_rigidBody.velocity.y == 0 &&
            (CurrentState == MovementState.Idle || CurrentState == MovementState.Running) && vertical > 0)
        {
            // Updates your current state.
            CurrentState = MovementState.Jumping;

            // Works out the new force to add.
            var force = Vector2.up * vertical * JumpForce;
            _rigidBody.AddForce(force);
        }

        // Check if the player can run.
        // Player can't run faster than the max speed.
        if (Mathf.Abs(_rigidBody.velocity.x) < MaxRunningSpeed)
        {
            // Works out the new force to apply.
            var force = Vector2.right * horizontal * RunningForce;
            _rigidBody.AddForce(force);

            // Flip the player image depending on the X velocity.
            if (_rigidBody.velocity.x < 0) _renderer.flipX = false;
            else if (_rigidBody.velocity.x > 0) _renderer.flipX = true;
        }

        // Check for falling.
        // You can only start falling if you've jumped, or fell of an edge.
        if ((CurrentState == MovementState.Jumping || CurrentState == MovementState.Running) &&
            _rigidBody.velocity.y < -0.2)
        {
            CurrentState = MovementState.Falling;
        }
    }

    private void Update()
    {
        switch (CurrentState)
        {
            // Updates the animator based on the current movement state.
            // You can do this part in any way you want.
            case MovementState.Idle:
                // Animator stuff goes here.
                _animator.SetBool("Idle", true);
                _animator.SetBool("Running", false);
                _animator.SetBool("Jumping", false);
                break;
            case MovementState.Running:
                _animator.SetBool("Idle", false);
                _animator.SetBool("Running", true);
                _animator.SetBool("Jumping", false);
                break;
            case MovementState.Jumping:
                _animator.SetBool("Idle", false);
                _animator.SetBool("Running", false);
                _animator.SetBool("Jumping", true);
                break;
            case MovementState.Falling:
                _animator.SetBool("Idle", false);
                _animator.SetBool("Running", false);
                _animator.SetBool("Jumping", true);
                break;
        }
    }

    // Grounded check.
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Skips the check if the y velocity isn't equal to zero or you're jumping.
        if (_rigidBody.velocity.y != 0 || CurrentState == MovementState.Jumping) return;

        // Changes your state based on your x velocity, zero means idle, anything else means running.
        CurrentState = _rigidBody.velocity.x == 0 ? MovementState.Idle : MovementState.Running;
    }
}
