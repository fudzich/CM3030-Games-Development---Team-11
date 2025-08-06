using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject playButton;

    [SerializeField]
    GameObject tutorialLevelButton;

    [SerializeField]
    GameObject bigCityLevelButton;

    [SerializeField]
    GameObject retrunButton;

    public void ToggleButtons()
    {
        playButton.SetActive(!playButton.activeSelf);
        tutorialLevelButton.SetActive(!tutorialLevelButton.activeSelf);
        bigCityLevelButton.SetActive(!bigCityLevelButton.activeSelf);
        retrunButton.SetActive(!retrunButton.activeSelf);

    }


}
