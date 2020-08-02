using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Player _player;
    private Vector3 _position;
    private Inputs _inputs;
    private Inputs _previousInputs;

    private bool rewindButtonDown;

    void Start() {
       _player = GetComponent<Player>();
    }

    void FixedUpdate() {
        CaptureInput();
        UpdatePlayer();    
    }

    private void CaptureInput() {
        _position = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        bool jumpButtonDown = Input.GetKeyDown("space");
        rewindButtonDown = Input.GetKey(KeyCode.LeftShift);

        _inputs = new Inputs(_position, jumpButtonDown, rewindButtonDown);
    }

    private void UpdatePlayer() {
        if(_position != Vector3.zero && !rewindButtonDown)
            _player.FlipSprite(_position.x < 0);
        _player.HandleInput(_inputs);
    }
}
