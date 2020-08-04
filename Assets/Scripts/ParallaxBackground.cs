using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

    [SerializeField]
    private Vector2 _parallaxMultiplier;

    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;

    private float _textureSizeX;

    void Start() {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;

        _textureSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void LateUpdate() {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * _parallaxMultiplier.x, deltaMovement.y * _parallaxMultiplier.y);
        _lastCameraPosition = _cameraTransform.position;

        if(Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureSizeX) {
            float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureSizeX;
            transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
        }
    }
}
