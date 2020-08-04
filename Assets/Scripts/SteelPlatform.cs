using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelPlatform : MonoBehaviour {

    [SerializeField]
    private bool _isPowered;

    public bool IsPowered() {
        return _isPowered;
    }
}
