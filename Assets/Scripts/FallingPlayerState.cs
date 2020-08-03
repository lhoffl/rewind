using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlayerState : IPlayerState {

    private Player _player;
    private Vector3 _position;

    private Stack<Inputs> _inputs;

    private bool _arrivedFromDefaultState = false;

    private float _currentFallForce = PlayerSettings.DoubleFallForce;

    private int _finalCount;

    private bool _undoActive = false;

    public void HandleInput(Inputs inputs) {

        if(!_inputs.Contains(inputs)) {
            if(inputs.Position != Vector3.zero)
                _inputs.Push(inputs);
        }

        if(inputs.RewindButtonDown) {
            _player.EnterState(new RewindingPlayerState());
        }

        _position = inputs.Position;
    }
    
    public void Update() {
        Move(_position.x);
        Fall();
    }

    public void HandleCollision(Collision2D other) {}

    public void Exit() {
        _finalCount = _inputs.Count;
    }

    public void Enter(Player player) {
        _player = player;
        _inputs = new Stack<Inputs>();

        if(!(_player.GetPreviousState() is DefaultPlayerState) && !(_player.GetPreviousState() is RewindingPlayerState)) {
            Jump();
        }
        else {
            _arrivedFromDefaultState = true;
        }
    }

    private void Fall() {

        _player.AddForce(_currentFallForce);
        _currentFallForce += PlayerSettings.DefaultFallAcceleration;

        if(_player.IsGrounded() && !_undoActive)
            _player.EnterState(new DefaultPlayerState());
    }

    private void Jump() {
        _player.AddForce(PlayerSettings.DoubleJumpForce);
    }

    private void Move(float positionX) {
        _player.Move(PlayerSettings.DefaultAccelerationFactor, PlayerSettings.FallingMaxSpeed, positionX);
    }

    public Stack<Inputs> GetInputs() {
        return _inputs;
    }

    public void Undo() {

        _undoActive = true;

        Inputs input = _inputs.Pop();
        Vector3 position = -input.Position;

        if(_inputs.Count == 1 && !_arrivedFromDefaultState) {
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
