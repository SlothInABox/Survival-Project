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
    [SerializeField] private Text scoreUI;
    [SerializeField] private Text gameOverScoreUI;
    [SerializeField] private RectTransform healthBar;

    private SpawnManager spawnManager;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        player.OnDeath += OnGameOver;
    }

    private void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        spawnManager.OnNewWave += OnNewWave;
    }

    private void Update()
    {
        scoreUI.text = ScoreKeeper.Score.ToString("D6");
        float healthPercent = 0;
        if (player != null)
        {
            healthPercent = player.health / player.startingHealth;
        }
        healthBar.localScale = new Vector3(healthPercent, 1, 1);
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

    private void OnGameOver()
    {
        Cursor.visible = true;
        StartCoroutine(Fade(Color.clear, new Color(0,0,0, 0.9f), 1));
        gameOverScoreUI.text = scoreUI.text;
        scoreUI.gameObject.SetActive(false);
        healthBar.transform.parent.gameObject.SetActive(false);
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

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
