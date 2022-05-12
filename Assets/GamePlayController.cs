using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] int score;
    [SerializeField] int highscore;
    public Color[] template = { new Color32(255, 81, 81, 255), new Color32(255, 129, 82, 255), new Color32(255, 233, 82, 255), new Color32(163, 255, 82, 255), new Color32(82, 207, 255, 255), new Color32(170, 82, 255, 255) };

    private int playerColor = 0;
    [SerializeField] Image colorImage;
    private int playerNextColor = 0;
    [SerializeField] Image colorNextImage;
    private UIController uiController;

    private float time;
    [SerializeField] float timeToChangeColor;
    [SerializeField] float timeOfGame;

    // Start is called before the first frame update
    void Start()
    {
        uiController = GetComponent<UIController>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        UpdateSlider();

        if(time < 0)
        {
            GameOver();
        }
    }

    public void UpdateSlider()
    {
        uiController.UpdateSlider(time);
    }

    public void SetSlider()
    {
        uiController.SetSlider(timeOfGame);
    }

    public void OnPressHandle(int index)
    {
        if(index == playerColor)
        {
            UpdateScore();
        }
        else
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        uiController.GameOver();
        Reset();
    }

    public void UpdateScore()
    {
        time++;
        score++;
        uiController.UpdateScore(score);
        if (score > highscore)
        {
            highscore = score;
            uiController.UpdateHighScore(highscore);
            PlayerPrefs.SetInt("highscore", highscore);
        }
    }

    public void UpdateColor()
    {
        colorImage.color = template[playerColor];
        colorNextImage.color = template[playerNextColor];
    }

    public void ChangeColor()
    {
        playerColor = playerNextColor;
        playerNextColor = Random.Range(0, template.Length);
        while(playerNextColor == playerColor)
        {
            playerNextColor = Random.Range(0, template.Length);
        }
        UpdateColor();
    }

    public void Reset()
    {
        Time.timeScale = 1;

        playerNextColor = Random.Range(0, template.Length);
        ChangeColor();
        time = timeOfGame;
        SetSlider();
        score = 0;
        highscore = PlayerPrefs.GetInt("highscore");
        uiController.UpdateScore(score);
        uiController.UpdateHighScore(highscore);
    }

}
