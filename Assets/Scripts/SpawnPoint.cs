using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    
    Animator _animator;
    SpriteRenderer _spriteRenderer;

    void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetActive(bool enable) {
        _animator.SetBool("isActive", enable);
    }

}
