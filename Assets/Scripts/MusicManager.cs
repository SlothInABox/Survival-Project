using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioClip menuTheme;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(menuTheme, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlayMusic(mainTheme, 2);
        }
    }
}
