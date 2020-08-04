using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : ICommand {

    Inputs _inputs;
    Player _player;

    float _jumpForce;

    public JumpCommand(float force) {
        _jumpForce = force;
    }

    public void execute(Inputs input, Entity entity) {
        _inputs = input;
        _player = (Player) entity;

        Jump();
    }

    public void undo() {
        Jump();
    }

    private void Jump() {
        _player.AddForce(_jumpForce);
    }
}
