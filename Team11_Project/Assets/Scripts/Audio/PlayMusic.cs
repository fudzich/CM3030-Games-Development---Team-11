using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.AudioType.Background_Music);

        AudioManager.Instance.PlayMusic(AudioManager.AudioType.Background_Sounds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
