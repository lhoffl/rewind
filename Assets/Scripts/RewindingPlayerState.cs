using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindingPlayerState : IPlayerState {

    private Player _player;
    private Vector3 _position;

    private Stack<IPlayerState> _states;

    private Stack<Inputs> _currentInputs;
    private IPlayerState _currentRewoundState;

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
            Debug.Log("Rewinding " + _states.Count + " states");
            _currentRewoundState = _states.Pop();
            _currentInputs = _currentRewoundState.GetInputs();
        }
    }

    public void Undo() {

        if(_currentInputs != null && _currentInputs.Count != 0) {            
            Inputs inputs = _currentInputs.Pop();
            Vector3 pos = -inputs.Position;

            float acclerationFactor = PlayerSettings.DefaultAccelerationFactor;
            float maxSpeed = PlayerSettings.DefaultMaxSpeed;

            if(_currentRewoundState is DefaultPlayerState && inputs.JumpButtonDown) {
                _player.AddForce(PlayerSettings.JumpForce);
            }

            if(_currentRewoundState is JumpingPlayerState) {
                if(inputs.JumpButtonDown) {
                    _player.AddForce(PlayerSettings.DoubleJumpForce);
                }
                else { 
                    _player.AddForce(PlayerSettings.DefaultFallForce);
                }

                Color color = PlayerSettings.JumpColor;
                color.a = 0.5f;

                _player.ChangeColor(color);
                acclerationFactor = PlayerSettings.JumpingAccelerationFactor;
            }
            else {
                Color color = _player.GetDefaultColor();
                color.a = 0.5f;

                _player.ChangeColor(color);
            }

            if(_currentRewoundState is FallingPlayerState) {
                _player.AddForce(PlayerSettings.DoubleFallForce);
                maxSpeed = PlayerSettings.FallingMaxSpeed;
            }

            _player.Move(acclerationFactor, maxSpeed, pos.x);
        } 
        else if (_states.Count > 0 ) {
            _currentRewoundState = _states.Pop();
            _currentInputs = _currentRewoundState.GetInputs();
        }
    }

    public Stack<Inputs> GetInputs() {
        return null;
    }
}
