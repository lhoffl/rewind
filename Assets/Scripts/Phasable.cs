using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phasable : MonoBehaviour {
    
    [SerializeField]
    Player _player;

    [SerializeField]
    private bool _enabledWithPhase;

    void Start() {}

    void Update(){
        for(int i = 0; i < transform.childCount; i++) {
            if(_enabledWithPhase)
                transform.GetChild(i).gameObject.SetActive(_player.IsRewinding());
            else
                transform.GetChild(i).gameObject.SetActive(!_player.IsRewinding());
        }
    }
}
