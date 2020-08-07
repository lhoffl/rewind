using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public static int CurrentLevel { get; set; }
    public static SceneChanger Instance { get; private set; }    

    private void Awake() {
        CurrentLevel = 1;
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public static void LoadMenu(string menu) {
        SceneManager.LoadScene(menu);
    }

    public void LoadLevel(int level) {
        StartCoroutine(Wait(level));
    }

    private IEnumerator Wait(int level) {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("Level" + level);
    }
}
