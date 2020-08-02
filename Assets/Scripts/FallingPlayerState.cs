using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlayerState : IPlayerState {

    private Player _player;
    private Vector3 _position;

    private Stack<Inputs> _inputs;

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
        Move();
        Fall();
    }

    public void HandleCollision(Collision2D other) {}
    public void Exit() {}

    public void Enter(Player player) {
        _player = player;
        _inputs = new Stack<Inputs>();

        Jump();
    }

    private void Fall() {

        _player.AddForce(PlayerSettings.DoubleFallForce);

        if(_player.IsGrounded())
            _player.EnterState(new DefaultPlayerState());
    }

    private void Jump() {
        _player.AddForce(PlayerSettings.DoubleJumpForce);
    }

    private void Move() {
        _player.Move(PlayerSettings.DefaultAccelerationFactor, PlayerSettings.FallingMaxSpeed, _position.x);
    }

    public Stack<Inputs> GetInputs() {
        return _inputs;
    }
}
