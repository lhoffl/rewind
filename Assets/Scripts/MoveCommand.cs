using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand {

    Inputs _inputs;
    Entity _entity;

    float _acceleration, _maxSpeed;
    float _heightAtExecution;

    bool _wasOnPoweredSteel;

    public MoveCommand(float acceleration, float maxSpeed, float heightAtExecution) {
        _acceleration = acceleration;
        _maxSpeed = maxSpeed;
        _heightAtExecution = heightAtExecution;
    }

    public void execute(Inputs input, Entity entity) {
        _inputs = input;
        _entity = entity;

        _wasOnPoweredSteel = _entity.OnPoweredSteel();
        Move(false);
    }

    public void undo() {
        Move(true);
        float clampedHeight = (Mathf.Clamp(_entity.transform.position.y, _heightAtExecution, float.MaxValue));
        Vector3 clampedPostion = new Vector3(_entity.transform.position.x, _heightAtExecution, _entity.transform.position.z);

        //_entity.transform.position = clampedPostion;
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
