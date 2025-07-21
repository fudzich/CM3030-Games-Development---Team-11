using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  

public class GameController : MonoBehaviour
{
    public static GameController Instance;  

    [SerializeField] private float gameDuration = 300f;  // 5 minutes in seconds
    private float remainingTime;
    [SerializeField] private TextMeshProUGUI timerText;  // for UI text display

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // Persist across scenes if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        remainingTime = gameDuration;
        UpdateTimerUI();
    }

    void Update()
    {
        if (!isGameOver && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                PlayerWin();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! Player Died.");

        // [TODO: game over logic e.g., show UI, stop enemies, boss stage]
        // Example: Time.timeScale = 0; to pause

        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null)
        {
            _ui.ToggleDeathPanel();
        }
    }


    public void PlayerWin()
    {
        isGameOver = true;
        Debug.Log("Game Over! Player Survived.");

        UIManager _ui = GetComponent<UIManager>();
        if (_ui != null)
        {
            _ui.ToggleWinPanel();
        }
    }

    public bool IsGameOver() => isGameOver;
}