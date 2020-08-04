using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs {

    private Vector3 _position;
    private bool _jumpButtonDown;
    private bool _rewindButtonDown;
    
    public Vector3 Position => _position;
    public bool JumpButtonDown => _jumpButtonDown;
    public bool RewindButtonDown => _rewindButtonDown;

    public Inputs(Vector3 position, bool jumpButtonDown, bool rewindButtonDown) {
        _position = position;
        _jumpButtonDown = jumpButtonDown;
        _rewindButtonDown = rewindButtonDown;
    }
}