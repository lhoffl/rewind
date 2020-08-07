using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour {
    
    [SerializeField]
    private Image _glowingSprite;

    [SerializeField]
    private int _modifier;

    private float _currentAlpha;
    private int _count;

    private bool _forwards;

    private Color _startingColor, _currentColor;

    void Start() {
        _startingColor = _glowingSprite.color;
        _currentColor = _startingColor;
    }

    void Update() {

        if(_forwards) {
            _currentColor.a += 0.01f;
            
            if(_count >= 60 || _currentAlpha >= 0.75f) {
              _currentAlpha = 0.75f;
              _currentColor.a = _currentAlpha;
              _count = 0;
              _forwards = false;
            }
        }
        else {
            
            _currentColor.a -= 0.01f;

            if(_count >= 60) {
                _currentAlpha = 0f;
                _currentColor.a = _currentAlpha;
                _count = 0;
                _forwards = true;
            }
        }
        _glowingSprite.color = _currentColor;
        _count++;
    }
}
