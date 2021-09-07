using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    
    public void Change(string sceneName){
        SceneManager.LoadScene(sceneName);

    }
    public void LoadGameScene(int level)
    {
        int row = level /  3;
        SceneManager.LoadScene("GameScene");
    }

    public void Exit(){
        Application.Quit();
    }
    
}
