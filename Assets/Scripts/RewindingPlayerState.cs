﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindingPlayerState : IPlayerState {

    private Player _player;
    private Vector3 _position;

    private Stack<IPlayerState> _states;

    private IPlayerState _currentRewoundState;

    private int _maxStates;

    private bool _wasOnPoweredSteel = false;

    private float _defaultFallAccleration = PlayerSettings.DefaultFallAcceleration;
    private float _currentFallAccleration = 0;

    public void HandleInput(Inputs inputs) {

        if(!inputs.RewindButtonDown) {
            RewindFinished();
        } 
    }

    public void HandleCollision(Collider2D other) {}

    public void Update() {
        Rewind();
    }
    
    public void Exit() {
        _player.UpdateJumpingAnimation(false);
        _player.ToggleCollider(false);
        _states.Clear();
        _player.SetGravityScale(1);
        _player.UpdateRewindUI(_states.Count, 0);
    }

    public void Enter(Player player) {
        _player = player;
        _states = _player.StateStack;
        _player.ToggleCollider(true);
        _player.SetGravityScale(0);

        if(_states.Count > 0) {
            _currentRewoundState = _states.Pop();
            _maxStates = _states.Count;
            _player.UpdateRewindUI(_states.Count, _states.Count);
        }
    }

    public void Undo() {}

    private void Rewind() {

        if(!_currentRewoundState.UndoComplete()) {
            if(!_wasOnPoweredSteel) {
                if(_currentRewoundState.WasOnPoweredSteel()) {
                    _wasOnPoweredSteel = true;
                }
            }
            else {
                _currentRewoundState.ModifySpeed();
            }

            _currentRewoundState.Undo();
        }
        else if (_states.Count > 0) {
            _player.UpdateRewindUI(_maxStates, _states.Count);
            _currentRewoundState = _states.Pop();
        }
        else {
            RewindFinished();
        }
    }

    private void RewindFinished() {
        if(_player.IsGrounded()) { 
            _player.EnterState(new DefaultPlayerState());
        }
        else {
            _player.EnterState(new FallingPlayerState());
        }

        _player.DisableRewind();
    }

    public Stack<Inputs> GetInputs() {
        return null;
    }

    public bool UndoComplete() {
        return _states.Count <= 0;
    }

    public bool WasOnPoweredSteel() {
        return false;
    }

    public void ModifySpeed() {}
}
