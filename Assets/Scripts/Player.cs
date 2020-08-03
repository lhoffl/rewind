using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private LayerMask _platformLayerMask, _floorLayerMask;

    private IPlayerState _currentState;
    private IPlayerState _previousState;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;

    private Vector3 _spawnPoint;

    private Animator _animator;

    private Color _defaultColor;

    public Rigidbody2D Rigidbody {get; private set;}

    public Stack<IPlayerState> StateStack;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        StateStack = new Stack<IPlayerState>();

        _defaultColor = _spriteRenderer.color;

        Rigidbody = _rigidbody;

        EnterState(new DefaultPlayerState());
    }
    
    private void FixedUpdate() {
        _currentState.Update();

        if(Mathf.Abs(transform.position.y) > 100) {
            Respawn();
        }
    }

    public void EnterState(IPlayerState state) {
        if(_currentState != null) {
            _currentState.Exit();
        }

        if(!(state is RewindingPlayerState))
            StateStack.Push(state);

        _previousState = _currentState;
        _currentState = state;
        _currentState.Enter(this);
        
        //Debug.Log(_currentState.GetType());
    } 
    
    public void HandleInput(Inputs inputs) {
        _currentState.HandleInput(inputs);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.CompareTag("SpawnPoint")) {
            Debug.Log("Hit " + other.gameObject.name + " at " + other.gameObject.transform.position);
            _spawnPoint = other.gameObject.transform.position;
        }

        _currentState.HandleCollision(other);
    }

    public void FlipSprite(bool active) {
        _spriteRenderer.flipX = !active;
    }

    public bool IsGrounded() {
        return CheckForCollisionOnLayer(_platformLayerMask) || CheckForCollisionOnLayer(_floorLayerMask);
    }

    private bool CheckForCollisionOnLayer(LayerMask mask) {

        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.1f, transform.position.y - 1f),
            new Vector2(transform.position.x + 0.1f, transform.position.y - 1.1f), mask);
    }

    public bool NotOnSteel() {
        return true; //!CheckForCollisionOnLayer(_floorLayerMask);
    }

    public void ResetVelocity() {
        _rigidbody.velocity = Vector2.zero;
    }

    public void ToggleCollider(bool enabled) {
        Physics2D.IgnoreLayerCollision(0, 8, enabled);
        Color color = _defaultColor;
        color.a = enabled ?  0.5f : 1f;

        ChangeColor(color);
    }

    public void Respawn() {
        transform.position = _spawnPoint;
        EnterState(new DefaultPlayerState());
        ResetVelocity();
    }

    public void ChangeColor(Color color) {
        _spriteRenderer.color = color;
    }

    public void UpdateJumpingAnimation(bool active) {
        _animator.SetBool("isJumping", active);
    }

    public void ResetColor() {
        _spriteRenderer.color = _defaultColor;
    }

    public Color GetColor() {
        return _spriteRenderer.color;
    }

    public Color GetDefaultColor() {
        return _defaultColor;
    }

    public void AddForce(float force) {
        _rigidbody.AddForce(new Vector2(0f, force));
    }

    public void Move(float acceleration, float maxSpeed, float positionX) {
        _rigidbody.velocity = new Vector3(acceleration * positionX, _rigidbody.velocity.y, 0);
        Mathf.Clamp(_rigidbody.velocity.x, 0, maxSpeed);
    }

    public IPlayerState GetPreviousState() {
        return _previousState;
    }
}