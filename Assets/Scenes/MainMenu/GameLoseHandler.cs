using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoseHandler : MonoBehaviour
{
    public void HandleLoss()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
