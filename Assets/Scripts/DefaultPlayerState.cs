using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayerState : IPlayerState {

    private Player _player;
    private Vector3 _position;

    private Stack<Inputs> _inputs;

    public void HandleInput(Inputs inputs) {

        if(!_inputs.Contains(inputs)) {
            if(inputs.Position != Vector3.zero)
                _inputs.Push(inputs);
        }

        if(inputs.JumpButtonDown) {
            if(_player.NotOnSteel())
                _player.EnterState(new JumpingPlayerState());
        }

        if(inputs.RewindButtonDown) {
            _player.EnterState(new RewindingPlayerState());
        }

        _position = inputs.Position;
    }

    public void HandleCollision(Collision2D other) {}

    public void Update() {
        Move();
    }
    
    public void Exit() {}

    public void Enter(Player player) {
        _player = player;
        _inputs = new Stack<Inputs>();
    }

    private void Move() {
        _player.Rigidbody.velocity = new Vector3(PlayerSettings.DefaultAccelerationFactor * _position.x, _player.Rigidbody.velocity.y, 0);
        Mathf.Clamp(_player.Rigidbody.velocity.x, 0, PlayerSettings.DefaultMaxSpeed);
    }

    public Stack<Inputs> GetInputs() {
        return _inputs;
    }
}
