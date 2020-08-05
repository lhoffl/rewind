using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : ICommand {

    Inputs _inputs;
    Player _entity;

    float _heightAtExecution;
    float _jumpForce;

    public JumpCommand(float force, float heightAtExecution) {
        _jumpForce = force;
        _heightAtExecution = heightAtExecution;
    }

    public void execute(Inputs input, Entity entity) {
        _inputs = input;
        _entity = (Player) entity;

        Jump();
    }

    public void undo() {
        Jump();
        if(_jumpForce <= 0) {
            float clampedHeight = (Mathf.Clamp(_entity.transform.position.y, _heightAtExecution, float.MaxValue));
            Vector3 clampedPostion = new Vector3(_entity.transform.position.x, clampedHeight, _entity.transform.position.z);
            //_entity.transform.position = clampedPostion;
        }
    }

    private void Jump() {
        _entity.AddForce(_jumpForce);
    }
}
