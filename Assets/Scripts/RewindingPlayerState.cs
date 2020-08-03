using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindingPlayerState : IPlayerState {

    private Player _player;
    private Vector3 _position;

    private Stack<IPlayerState> _states;

    private IPlayerState _currentRewoundState;

    private float _defaultFallAccleration = PlayerSettings.DefaultFallAcceleration;
    private float _currentFallAccleration = 0;

    public void HandleInput(Inputs inputs) {

        if(!inputs.RewindButtonDown) {
            if(_player.IsGrounded()) { 
                _player.EnterState(new DefaultPlayerState());
            }
            else {
                _player.EnterState(new FallingPlayerState());
            }
        } 
    }

    public void HandleCollision(Collision2D other) {}

    public void Update() {
        Undo();
    }
    
    public void Exit() {
        _player.ToggleCollider(false);
        _states.Clear();
    }

    public void Enter(Player player) {
        _player = player;
        _states = _player.StateStack;
        _player.ToggleCollider(true);
        if(_states.Count > 0) {
            //Debug.Log("Rewinding " + _states.Count + " states");
            _currentRewoundState = _states.Pop();
            Debug.Log("Undo " + _currentRewoundState.GetType());
        }
    }

    public void Undo() {

        if(_currentRewoundState is RewindingPlayerState) {
            if(_states.Count > 1) {
                _currentRewoundState = _states.Pop();
                Debug.Log("Undo " + _currentRewoundState.GetType());
            }
        }

        if(!_currentRewoundState.UndoComplete()) {
            _currentRewoundState.Undo();
        }
        else if (_states.Count > 0 ) {
            _currentRewoundState = _states.Pop();
            Debug.Log("Undo " + _currentRewoundState.GetType());
        }
    }

    public Stack<Inputs> GetInputs() {
        return null;
    }

    public bool UndoComplete() {
        return _states.Count <= 0;
    }
}
