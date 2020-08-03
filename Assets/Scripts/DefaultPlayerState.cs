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
        Move(_position.x);

        if(!_player.IsGrounded()) {
            _player.EnterState(new FallingPlayerState());
        }
    }
    
    public void Exit() {}

    public void Enter(Player player) {
        _player = player;
        _inputs = new Stack<Inputs>();
    }

    private void Move(float positionX) {
        _player.Move(PlayerSettings.DefaultAccelerationFactor, PlayerSettings.DefaultMaxSpeed, positionX);
    }

    public Stack<Inputs> GetInputs() {
        return _inputs;
    }

    public void Undo() {

        Inputs input = _inputs.Pop();
        Vector3 position = -input.Position;

        Move(position.x);
        _player.FlipSprite(position.x < 0);
    }

    public bool UndoComplete() {
        return _inputs.Count <= 0;
    }
}
