using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {

    public static GUIManager instance;
    [SerializeField]
    private GameObject menuPanel, gamePanel, gameOverPanel, car1;

    [SerializeField]
    private Text gameOverScore, gameOverHiScore;
    [SerializeField]
    private GameObject[] Lifes;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private int scoreMultiplier = 1;

    private float timer;
    private int score = 0;
    private int hiScore;
    private bool gameStarted = false, gameOver = false;
    private bool rePlay =false;

    public int Score { get { return score; } set { score = value; } }
    public bool GameStarted { get { return gameStarted; } set { gameStarted = value; } }
    public bool GameOver { get { return gameOver; } set { gameOver = value; } }


    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start()
    {
        score = 0;
        scoreText.text = "" + score;
        if (PlayerPrefs.HasKey("HiScore"))
        {
            hiScore = PlayerPrefs.GetInt("HiScore");
        }

        if (PlayerPrefs.HasKey("Replay"))
        {
            if (PlayerPrefs.GetString("Replay") == "true")
            {
                rePlay = true;
                PlayerPrefs.SetString("Replay", "false");
            }
        }
        if(rePlay)
        {
            PlayButton();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (gameStarted == false || gameOver == true) 
            return;
        timer += Time.deltaTime;
        if (timer > 1)
        {
            score += 1 *scoreMultiplier;
            Debug.Log("score" + score);
            scoreText.text =  "" + score;
            Debug.Log("scoreText" + scoreText.text);

            timer = 0;
        }
	}

    public void PlayButton()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        car1.SetActive(true);
        gameStarted = true;
    }

    public void ReplayButton()
    {
        PlayerPrefs.SetString("Replay", "true");
        HomeButton();
    }

    public void HomeButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void ReduceLife(int remainingLives)
    { 
        for (int i = 0; i < Lifes.Length; i++)
        {
            Lifes[i].SetActive(false);
        }
        for (int i = 0; i < remainingLives; i++)
        {
            Lifes[i].SetActive(true);
        }
    }

    public void GameOverMethod()
    {
        gameOver = true;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        gameOverScore.text = "" + score;

        if(hiScore <score)
        {
            hiScore = score;
            PlayerPrefs.SetInt("HiScore", hiScore);
        }
        gameOverHiScore.text = "BestScore: " + hiScore;
    }
}
