using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    
    Animator _animator;
    SpriteRenderer _spriteRenderer;

    [SerializeField]
    private bool _enabledAtStart;

    void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        SetActive(_enabledAtStart);
    }

    public void SetActive(bool enable) {
        _animator.SetBool("isActive", enable);
    }

}
