using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bcw_LoadScene : MonoBehaviour
{
    [SerializeField] string lobbyScene;
    [SerializeField] string Scene_1;
    [SerializeField] string Scene_2;
    [SerializeField] string Scene_3;
    [SerializeField] string Scene_4;



    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LobbyLoad()
    {
        SceneManager.LoadScene(lobbyScene);
    }

    public void Scene_1Load()
    {
        SceneManager.LoadScene(Scene_1);
    }

    public void Scene_2Load()
    {
        SceneManager.LoadScene(Scene_2);
    }

    public void Scene_3Load()
    {
        SceneManager.LoadScene(Scene_3);
    }
    public void Scene_4Load()
    {
        SceneManager.LoadScene(Scene_4);
    }
}
