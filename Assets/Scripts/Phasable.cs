using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phasable : MonoBehaviour {
    
    [SerializeField]
    Player _player;

    void Start() {}

    void Update(){
        for(int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(_player.IsRewinding());
        }
    }
}
