using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand {

    Inputs _inputs;
    Entity _entity;

    float _acceleration, _maxSpeed;

    bool _wasOnPoweredSteel;

    public MoveCommand(float acceleration, float maxSpeed) {
        _acceleration = acceleration;
        _maxSpeed = maxSpeed;
    }

    public void execute(Inputs input, Entity entity) {
        _inputs = input;
        _entity = entity;

        _wasOnPoweredSteel = _entity.OnPoweredSteel();
        Move(false);
    }

    public void undo() {
        Move(true);
    }

    private void Move(bool undo) {

        int modifier = undo ? -1 : 1;

        if(_wasOnPoweredSteel) {
            modifier *= PlayerSettings.PoweredSteelMultipler;
        }

        _entity.Move(_acceleration, _maxSpeed, modifier * _inputs.Position.x);
        if(_inputs.Position != Vector3.zero)
            _entity.FlipSprite(modifier * _inputs.Position.x < 0);
    }
}
