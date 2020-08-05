using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    
    [SerializeField]
    private Image _fullHeartContainer;

    [SerializeField]
    private Image _rewindMask;

    private int _numberOfHearts;
    private float _spacing = 2f;

    private int _numberOfStatesToRewind;
    private int _currentStateNumber;

    void Start() {
        _numberOfHearts = PlayerSettings.MaxHealth / PlayerSettings.HealthPerHeart;
        ResetUI();

    }

    void Update() {
        UpdateRewindBar();
    }

    void ResetUI() {
        for(int i = 0; i < _numberOfHearts; i++) {
            
        
        }
    }

    public void UpdateRewindMax(int max) {
        _numberOfStatesToRewind = max;
    }   

    public void UpdateCurrentState(int current) {
        _currentStateNumber = current;
    }

    void UpdateRewindBar() {
        float fillAmount = 0;
        
        if(_numberOfStatesToRewind > 0)
            fillAmount = (float) _currentStateNumber / (float) _numberOfStatesToRewind;

        _rewindMask.fillAmount = fillAmount;
    }


}
