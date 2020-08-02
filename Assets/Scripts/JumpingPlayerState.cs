using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlayerState : IPlayerState {

    private Stack<Inputs> _inputs;

    private Player _player;
    private Vector3 _position;

    private int timerMax = 15;
    private int currentTimer = 0;

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
        Move();
        Fall();
    }

    public void Exit() {
        _player.ResetColor();
    }

    public void Enter(Player player) {
        _player = player;
        _inputs = new Stack<Inputs>();
        Jump();

        currentTimer = timerMax;

        _player.ChangeColor(PlayerSettings.JumpColor);
    }

    private void Jump() {
        _player.AddForce(PlayerSettings.JumpForce);
    }

    private void Fall() {

        _player.AddForce(PlayerSettings.DefaultFallForce);
        currentTimer--;

        if(_player.IsGrounded() && currentTimer <= 0)
            _player.EnterState(new DefaultPlayerState());
    }

    private void Move() {
        _player.Move(PlayerSettings.JumpingAccelerationFactor, PlayerSettings.DefaultMaxSpeed, _position.x);
    }
    
    public Stack<Inputs> GetInputs() {
        return _inputs;
    }
}
