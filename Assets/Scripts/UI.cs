using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    
    [SerializeField]
    private Sprite _heartContainer;
    private Texture2D _heartTexture;
    private Rect _heartRect;

    private int _numberOfHearts;
    private float _spacing = 2f;

    void Start() {
        _numberOfHearts = PlayerSettings.MaxHealth / PlayerSettings.HealthPerHeart;
        _heartTexture = _heartContainer.texture;
        _heartRect = _heartContainer.rect;

        ResetUI();

    }

    void Update() {
        
    }

    void ResetUI() {
        for(int i = 0; i < _numberOfHearts; i++) {
            GUI.DrawTextureWithTexCoords(_heartRect, _heartTexture, new Rect(_heartRect.x / _heartTexture.width, 
                _heartRect.y / _heartTexture.height, _heartRect.width / _heartTexture.width,
                _heartRect.height / _heartTexture.height));
        }
    }
}
