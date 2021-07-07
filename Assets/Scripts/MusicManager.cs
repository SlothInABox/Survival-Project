using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioClip menuTheme;

    private int sceneIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        OnLevelWasLoaded(0);
    }

    private void OnLevelWasLoaded(int level)
    {
        int newSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (newSceneIndex != sceneIndex)
        {
            sceneIndex = newSceneIndex;
            Invoke("PlayMusic", 0.2f);
        }
    }

    private void PlayMusic()
    {
        AudioClip clipToPlay = null;

        if (sceneIndex == 0)
        {
            clipToPlay = menuTheme;
        }
        else if (sceneIndex == 1)
        {
            clipToPlay = mainTheme;
        }

        if (clipToPlay != null)
        {
            AudioManager.Instance.PlayMusic(clipToPlay, 2);
            Invoke("PlayMusic", clipToPlay.length);
        }
    }
}
