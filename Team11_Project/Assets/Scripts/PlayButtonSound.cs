using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonSound : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayMenuSound()
    {
        Debug.Log("a");
        AudioManager.Instance.Play(AudioManager.AudioType.Menu_Buttons);
    }



}
