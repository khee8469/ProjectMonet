using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string lobbyScene;
    [SerializeField] string gameScene;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LobbyLoad()
    {
        SceneManager.LoadScene(lobbyScene);
    }

    public void GameScene()
    {
        SceneManager.LoadScene(gameScene);
    }
    
}
