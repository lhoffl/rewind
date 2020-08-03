using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlayerState : IPlayerState {

    private Stack<Inputs> _inputs;

    private Player _player;
    private Vector3 _position;

    private int _timerMax = 15;
    private int _currentTimer = 0;

    private float _currentFallForce = PlayerSettings.DefaultFallForce;

    private int _finalCount;

    private bool _undoActive = false;

    public void HandleInput(Inputs inputs) {

        if(!_inputs.Contains(inputs)) {
            if(inputs.Position != Vector3.zero)
                _inputs.Push(inputs);
        }

        if(inputs.JumpButtonDown) {
            if(_player.NotOnSteel())
                _player.EnterState(new FallingPlayerState());
        }

        if(inputs.RewindButtonDown) {
            _player.EnterState(new RewindingPlayerState());
        }

        _position = inputs.Position;
    }

    public void HandleCollision(Collision2D other) {}

    public void Update() {
        Move(_position.x);
        Fall();
    }

    public void Exit() {
        _player.UpdateJumpingAnimation(false);
        _finalCount = _inputs.Count;
    }

    public void Enter(Player player) {
        
        _player = player;
        _inputs = new Stack<Inputs>();
        
        Jump();
        
        _currentTimer = _timerMax;
        _player.UpdateJumpingAnimation(true);
    }

    private void Jump() {
        _player.AddForce(PlayerSettings.JumpForce);
    }

    private void Fall() {

        _player.AddForce(_currentFallForce);
        _currentFallForce += PlayerSettings.DefaultFallAcceleration;
        
        _currentTimer--;

        if(_player.IsGrounded() && _currentTimer <= 0 && !_undoActive)
            _player.EnterState(new DefaultPlayerState());
    }

    private void Move(float positionX) {
        _player.Move(PlayerSettings.JumpingAccelerationFactor, PlayerSettings.DefaultMaxSpeed, positionX);
    }
    
    public Stack<Inputs> GetInputs() {
        return _inputs;
    }

    public void Undo() {

        _undoActive = true;

        Inputs input = _inputs.Pop();
        Vector3 position = -input.Position;

        if(_inputs.Count == 1) {
            Jump();
        }

        Move(position.x);
        Fall();

        _player.FlipSprite(position.x < 0);
    }

    public bool UndoComplete() {
        return _inputs.Count <= 0;
    }
}
