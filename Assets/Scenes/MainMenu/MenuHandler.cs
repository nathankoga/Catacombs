using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public GameObject TheCatInQuestion;
    public AudioSource MeowMeowmeowmreowmMeow;

    public GameObject ChoiceMenu;
    public GameObject StoryMenu;
    public GameObject OptionMenu;
    public void OnPlay()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void OnOptions()
    {
        ChoiceMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void OnStory()
    {
        ChoiceMenu.SetActive(false);
        StoryMenu.SetActive(true);
    }
    public void OnCat()
    {
        // Toggle cat active.
        TheCatInQuestion.SetActive(!TheCatInQuestion.activeSelf);
        // Play meow sound.
        MeowMeowmeowmreowmMeow.Play();
    }

    public void OnBack()
    {
        ChoiceMenu.SetActive(true);
        StoryMenu.SetActive(false);
        OptionMenu.SetActive(false);
    }
}
