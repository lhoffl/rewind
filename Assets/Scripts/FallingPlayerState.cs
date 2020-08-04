using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlayerState : IPlayerState {

    private Player _player;
    private Vector3 _position;

    private const int _timerMax = 15;
    private int _currentTimer = 0;

    private float _currentFallForce = PlayerSettings.DoubleFallForce;

    private Queue<ICommand> _jumpCommands;
    private Stack<ICommand> _moveCommands;

    private bool _undoActive = false;

    public void HandleInput(Inputs inputs) {

        if(inputs.JumpButtonDown) {
            if(_player.NotOnSteel())
                _player.EnterState(new JumpingPlayerState());
        }        
        else if(inputs.RewindButtonDown && _player.CanRewind()) {
            _player.EnterState(new RewindingPlayerState());
        }
        else {
            if(inputs.Position != Vector3.zero)
                Move(inputs);
        }
    }

    public void HandleCollision(Collider2D other) {}

    public void Update() {
        Fall();
    }

    public void Exit() {}

    public void Enter(Player player) {
        
        _player = player;
        _jumpCommands = new Queue<ICommand>();
        _moveCommands = new Stack<ICommand>();
        
        _currentTimer = _timerMax;
    }

    private void Fall() {
        
        JumpCommand fallCommand = new JumpCommand(_currentFallForce);
        fallCommand.execute(null, _player);
        _jumpCommands.Enqueue(fallCommand);

        _currentFallForce += PlayerSettings.DefaultFallAcceleration;
        _currentTimer--;

        if(_player.IsGrounded() && _currentTimer <= 0 && !_undoActive)
            _player.EnterState(new DefaultPlayerState());
    }

    private void Move(Inputs inputs) {
        MoveCommand moveCommand = new MoveCommand(PlayerSettings.DefaultAccelerationFactor, PlayerSettings.FallingMaxSpeed);
        moveCommand.execute(inputs, _player);
        _moveCommands.Push(moveCommand);
    }


    public void Undo() {
        _undoActive = true;
        
        if(_jumpCommands.Count > 0) {
            ICommand command = _jumpCommands.Dequeue();
            command.undo();
        }
        
        if(_moveCommands.Count > 0) {
            ICommand command = _moveCommands.Pop();
            command.undo();
        }
    }

    public bool UndoComplete() {
        return (_jumpCommands.Count <= 0 && _moveCommands.Count <= 0);
    }
}
