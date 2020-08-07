using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    private Player _player;

    public void StartGame() {
        _player.PlaySound("LevelComplete");
        SceneChanger.Instance.LoadLevel(1);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
