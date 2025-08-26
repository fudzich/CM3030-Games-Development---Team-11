using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpGain : MonoBehaviour
{
    [Header("Level Cost (Linear):")] // ΔXP(l) = α + β*(l-1)
    [SerializeField] private int alpha = 120; // L1→2
    [SerializeField] private int beta = 60;  // step per level
    [SerializeField] private int levelCap = 99;

    [Header("Growth per Level")]
    [SerializeField]
    float speedInreaseValue;
    [SerializeField]
    float maxHPInreaseValue;

    [Header("UI")]
    [SerializeField]
    private TMPro.TextMeshProUGUI levelText;

    [SerializeField]
    private ExpBar expBar;

    [SerializeField] private GameObject levelUpAnimation;
    [SerializeField] private float animationDuration = 1.5f;

    private int exp;
    int nextLevelRequirement;
    int currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;
        exp = 0;
        nextLevelRequirement = Mathf.Max(1, NextRequirementForLevel(currentLevel));

        if (expBar) { expBar.updateMaxExp(nextLevelRequirement); expBar.updateExp(exp); }

        UpdateLevelUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLevel < levelCap && exp >= nextLevelRequirement)
        {
            levelUp();
        }
    }

    public void AddEXP(int value)
    {
        if (value <= 0) return;
        exp += value;
        if (expBar) expBar.updateExp(exp);
    }
    private void levelUp()
    {
        exp -= nextLevelRequirement;
        currentLevel = Mathf.Min(levelCap, currentLevel + 1);

        //ststs increase:
        gameObject.GetComponent<PlayerMovement>().IncreaseSpeed(speedInreaseValue);
        gameObject.GetComponent<PlayerStatus>().increaseMaxHP(maxHPInreaseValue);
        nextLevelRequirement = (currentLevel < levelCap) ? NextRequirementForLevel(currentLevel) : int.MaxValue; // cal next level

        expBar.updateMaxExp(nextLevelRequirement);
        expBar.updateExp(exp);
        gameObject.GetComponent<SkillManager>().learnSkillbyLv(currentLevel);
        UpdateLevelUI();

        AudioManager.Instance.Play(AudioManager.AudioType.Levelup);
        StartCoroutine(PlayLevelUpAnimation());
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = "Lv " + currentLevel;
        }
    }
    private IEnumerator PlayLevelUpAnimation()
    {
        levelUpAnimation.SetActive(true);
        yield return new WaitForSeconds(animationDuration);
        levelUpAnimation.SetActive(false);
    }

    private int NextRequirementForLevel(int level)
    {
        return alpha + beta * (level - 1);
    }


}
