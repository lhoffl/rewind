using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {

    [SerializeField]
    private LayerMask _platformLayerMask, _floorLayerMask;

    [SerializeField]
    private SpawnPoint _initialSpawn;

    [SerializeField]
    private AudioClip _hit, _levelComplete, _jump, _doubleJump, _death, _checkpoint;

    [SerializeField]
    private MusicManager _music;

    private IPlayerState _currentState;
    private IPlayerState _previousState;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _hitbox;

    private SpawnPoint _spawnPoint;
    private Animator _animator;
    private Color _defaultColor;

    private bool _rewindEnabled;
    private const int REWIND_COOLDOWN = 30;
    private int _cooldownCounter;

    private const int STUCK_COOLDOWN = 30;
    private int _stuckCooldownCounter;

    private const int DAMAGE_COOLDOWN = 15;
    private int _damagedCooldownCounter;

    private bool _takenDamageWithinCooldown = false;

    private int _distanceToDeath = 100;

    private bool _onMovingPlatform;

    private MovingPlatform _currentPlatform;

    private AudioSource _audioSource;

    private UI _ui;

    private int _health = PlayerSettings.MaxHealth;
    public Rigidbody2D Rigidbody {get; private set;}

    public Stack<IPlayerState> StateStack;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hitbox = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        _ui = GetComponent<UI>();

        StateStack = new Stack<IPlayerState>();

        _defaultColor = _spriteRenderer.color;

        _rewindEnabled = true;
        _cooldownCounter = REWIND_COOLDOWN;

        _stuckCooldownCounter = STUCK_COOLDOWN;
        _damagedCooldownCounter = DAMAGE_COOLDOWN;

        _spawnPoint = _initialSpawn;
        _spawnPoint.SetActive(true);
        transform.position = _spawnPoint.transform.position;

        Rigidbody = _rigidbody;

        _audioSource = GetComponent<AudioSource>();

        EnterState(new DefaultPlayerState());
    }
    
    private void Update() {
        TickCooldowns();

        if(IsTrapped()) {
            _stuckCooldownCounter--;
            if(_stuckCooldownCounter <= 0)
                Respawn();
        }
        else {
            _stuckCooldownCounter = STUCK_COOLDOWN;
        }

        if(_onMovingPlatform) {
            RideMovingEntity(_currentPlatform);
        }
    }

    private void FixedUpdate() {
        _currentState.Update();

        if(Mathf.Abs(transform.position.y - _spawnPoint.transform.position.y) > _distanceToDeath) {
            Respawn();
        }
    }

    public void EnterState(IPlayerState state) {
        if(_currentState != null) {
            _currentState.Exit();
        }

        if(!(state is RewindingPlayerState)) {
            StateStack.Push(state);
        }

        _previousState = _currentState;
        _currentState = state;
        _currentState.Enter(this);
    } 
    
    public void HandleInput(Inputs inputs) {
        _currentState.HandleInput(inputs);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        SpawnPoint spawnPoint = other.gameObject.GetComponent<SpawnPoint>();

        if(spawnPoint != null) {
            if(spawnPoint.transform.position != _spawnPoint.transform.position) {
                _spawnPoint.SetActive(false);
                spawnPoint.SetActive(true);
                SetSpawn(spawnPoint);
                _audioSource.PlayOneShot(_checkpoint);
            }
        }

        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if(!(_currentState is RewindingPlayerState) && enemy != null) {
            
            if(!_takenDamageWithinCooldown) {
                _damagedCooldownCounter = DAMAGE_COOLDOWN;
                _takenDamageWithinCooldown = true;

                _animator.SetBool("tookDamage", true);
                _health--;
                _audioSource.PlayOneShot(_hit);
                UpdateHealthUI();
            }
            
            if(_health <= 0) {
                Respawn();
            }
        }

        Goal goal = other.gameObject.GetComponent<Goal>();
        if(goal != null) {
            _audioSource.PlayOneShot(_levelComplete);
        }

        MovingPlatform movingPlatform = other.gameObject.GetComponent<MovingPlatform>();
        if(movingPlatform != null) {
            _onMovingPlatform = true;
            _currentPlatform = movingPlatform;
        }
        else {
            _onMovingPlatform = false;
        }

        _currentState.HandleCollision(other);
    }

    public void UpdateHealthUI() {
        _ui.UpdateHealthUI(_health);
    }

    public void UpdateRewindUI(int maximum, int current) {
        _ui.UpdateRewindMax(maximum);
        _ui.UpdateCurrentState(current);
    }

    public bool IsGrounded() {
        return CheckForCollisionOnLayer(_platformLayerMask) || CheckForCollisionOnLayer(_floorLayerMask);
    }

    private bool CheckForCollisionOnLayer(LayerMask mask) {

        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.1f, transform.position.y - 1f),
            new Vector2(transform.position.x + 0.1f, transform.position.y - 1.1f), mask);
    }

    public bool NotOnSteel() {
        return !CheckForCollisionOnLayer(_floorLayerMask);
    }

    public override bool OnPoweredSteel() {
        
        Collider2D hit = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.1f, transform.position.y - 1f),
            new Vector2(transform.position.x + 0.1f, transform.position.y - 1.1f), _floorLayerMask);

        SteelPlatform platform = null;

        if(hit != null) {
            platform = hit.gameObject.GetComponent<SteelPlatform>();
        }

        return (platform != null && platform.IsPowered());
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

        _audioSource.PlayOneShot(_death);

        _stuckCooldownCounter = STUCK_COOLDOWN;
        transform.position = _spawnPoint.transform.position;
        EnterState(new DefaultPlayerState());
        ResetVelocity();
        _health = PlayerSettings.MaxHealth;
        UpdateHealthUI();
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

    public IPlayerState GetPreviousState() {
        return _previousState;
    }

    public bool CanRewind() {
        return _rewindEnabled;
    }

    public void DisableRewind() {
        _rewindEnabled = false;
    }

    public void EnableRewind() {
        _rewindEnabled = true;
    }

    private void TickCooldowns() {
        TickRewindCooldown();
    }

    private void TickRewindCooldown() {

        if(!_rewindEnabled) {

            _cooldownCounter--;
            if(_cooldownCounter <= 0) {
                _cooldownCounter = REWIND_COOLDOWN;
                _rewindEnabled = true;
            }
        }

        if(_takenDamageWithinCooldown)
            _damagedCooldownCounter--;
        
        if(_damagedCooldownCounter <= 0) {
            _damagedCooldownCounter = DAMAGE_COOLDOWN;
            _animator.SetBool("tookDamage", false);

            _takenDamageWithinCooldown = false;
        }
    }

    public bool IsTrapped() {
        bool leftCheck = CheckHitBox(Vector2.left);
        bool rightCheck = CheckHitBox(Vector2.right);
        bool upCheck = CheckHitBox(Vector2.up);
        bool downCheck = CheckHitBox(Vector2.down);

        return !(_currentState is RewindingPlayerState) && leftCheck && rightCheck && upCheck && downCheck;
    }

    private bool CheckHitBox(Vector2 direction) {
        return (Physics2D.BoxCast(_hitbox.bounds.center, new Vector2(0.1f, 0.1f), 0f, direction, 0.1f, _floorLayerMask)
            || Physics2D.BoxCast(_hitbox.bounds.center, new Vector2(0.1f, 0.1f), 0f, direction, 0.1f, _platformLayerMask));
    }

    public void SetSpawn(SpawnPoint spawnPoint) {
        _spawnPoint = spawnPoint;
    }

    public void PlaySound(string sound) {
        
        AudioClip clip = null;

        if(sound.Equals("jump"))
            clip = _jump;
        else if(sound.Equals("doubleJump"))
            clip = _doubleJump;

        if(clip != null) {
            _audioSource.PlayOneShot(clip);
        }
    }

    public void SetGravityScale(float gravityScale) {
        _rigidbody.gravityScale = gravityScale;
    }
}