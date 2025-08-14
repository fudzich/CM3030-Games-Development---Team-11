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
    public float GameDuration => gameDuration;
    public float RemainingTime => remainingTime;
    public float ElapsedTime => Mathf.Clamp(gameDuration - remainingTime, 0f, gameDuration);

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
        var _ui = GetComponent<ModalManager>();
        if (_ui != null) _ui.ToggleDeathPanel();
    }

    public void PlayerWin()
    {
        AudioManager.Instance.Play(AudioManager.AudioType.Win);
        isGameOver = true;
        var _ui = GetComponent<ModalManager>();
        if (_ui != null) _ui.ToggleWinPanel();
    }

    public bool IsGameOver() => isGameOver;
}
