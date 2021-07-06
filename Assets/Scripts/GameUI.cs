using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Image fadePlane;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private RectTransform newWaveBanner;
    [SerializeField] private Text newWaveTitle;
    [SerializeField] private Text newWaveEnemyCount;

    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<Player>().OnDeath += OnGameOver;
    }

    private void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        spawnManager.OnNewWave += OnNewWave;
    }

    private void OnNewWave(int waveNumber)
    {
        string[] numbers = { "One", "Two", "Three", "Four", "Wave" };
        newWaveTitle.text = "- Wave " + numbers[waveNumber - 1] + " -";
        string enemyCountString = ((spawnManager.waves[waveNumber - 1].infinite) ? "Infinite" : spawnManager.waves[waveNumber - 1].enemyCount + "");
        newWaveEnemyCount.text = enemyCountString;

        StopCoroutine("AnimateNewWaveBanner");
        StartCoroutine("AnimateNewWaveBanner");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGameOver()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
    }

    private IEnumerator AnimateNewWaveBanner()
    {
        float delayTime = 1.5f;
        float speed = 3f;
        float animatePercent = 0;
        int dir = 1;

        float endDelayTime = Time.time + 1 / speed + delayTime;

        while (animatePercent >= 0)
        {
            animatePercent += Time.deltaTime * speed * dir;

            if (animatePercent >= 1)
            {
                animatePercent = 1;
                if (Time.time > endDelayTime)
                {
                    dir = -1;
                }
            }

            newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-110, 30, animatePercent);
            yield return null;
        }
    }

    private IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    // UI input
    public void StartNewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
