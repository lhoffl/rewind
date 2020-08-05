using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    
    [SerializeField]
    private Image[] _heartContainers;

    [SerializeField]
    private Sprite _emptyHeartContainer, _fullHeartContainer;

    [SerializeField]
    private Image _rewindMask;

    [SerializeField]
    private Image _rewindOverlay;

    [SerializeField]
    private Sprite _rewindPressed;

    private Sprite _rewindNormal;

    private int _numberOfHearts;
    private float _spacing = 2f;

    private int _numberOfStatesToRewind;
    private int _currentStateNumber;

    void Start() {
        _numberOfHearts = PlayerSettings.MaxHealth / PlayerSettings.HealthPerHeart;
        _rewindNormal = _rewindOverlay.sprite;

        ResetUI();

    }

    void Update() {
        UpdateRewindBar();
    }

    void ResetUI() {
        
    }

    public void UpdateRewindMax(int max) {
        _numberOfStatesToRewind = max;
    }   

    public void UpdateCurrentState(int current) {
        _currentStateNumber = current;
    }

    public void UpdateHealthUI(int currentHealth) {
        for(int i = 0; i < _heartContainers.Length; i++) {
            if(currentHealth > i)
                _heartContainers[i].sprite = _fullHeartContainer;
            else
                _heartContainers[i].sprite = _emptyHeartContainer;
        }
    }

    void UpdateRewindBar() {
        float fillAmount = 0;
        
        if(_numberOfStatesToRewind > 0) {
            fillAmount = (float) _currentStateNumber / (float) _numberOfStatesToRewind;
            _rewindOverlay.sprite = _rewindPressed;
        }
        else {
            _rewindOverlay.sprite = _rewindNormal;
        }

        _rewindMask.fillAmount = fillAmount;
    }


}
