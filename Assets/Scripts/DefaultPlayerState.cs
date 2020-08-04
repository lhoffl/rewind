﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPlayerState : IPlayerState {

    private Player _player;
    private Vector3 _position;

    private Stack<ICommand> _commands;

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
        if(!_player.IsGrounded()) {
            _player.EnterState(new FallingPlayerState());
        }
    }
    
    public void Exit() {}

    public void Enter(Player player) {
        _player = player;
        _commands = new Stack<ICommand>();

        _player.ResetVelocity();
    }

    private void Move(Inputs inputs) {
        MoveCommand moveCommand = new MoveCommand(PlayerSettings.DefaultAccelerationFactor, PlayerSettings.DefaultMaxSpeed);
        moveCommand.execute(inputs, _player);
        _commands.Push(moveCommand);
    }

    public void Undo() {
        ICommand command = _commands.Pop();
        command.undo();
    }

    public bool UndoComplete() {
        return _commands.Count <= 0;
    }
}
