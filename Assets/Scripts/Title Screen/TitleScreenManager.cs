using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{

    public void StartTheGame() {
        print("starting the game");
        StartCoroutine(LoadGame());
    }

    public IEnumerator LoadGame() {
        print("loading mountain scene");
        SceneManager.LoadScene("Mountain Scene");
        yield return null;
        print("going to mountain scene");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Mountain Scene"));
    }
}
