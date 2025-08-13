using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpGain : MonoBehaviour
{
    private float exp;

    [SerializeField]
    float nextLevelRequirements;

    int currentLevel;

    [SerializeField]
    float speedInreaseValue;
    [SerializeField]
    float maxHPInreaseValue;
    [SerializeField]
    private TMPro.TextMeshProUGUI levelText;

    [SerializeField]
    private ExpBar expBar;

    [SerializeField] private GameObject levelUpAnimation;
    [SerializeField] private float animationDuration = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 1;
        exp = 0;
        UpdateLevelUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (exp >= nextLevelRequirements)
        {
            levelUp();
        }
    }


    public void getEXP(float value)
    {
        exp += value;
        expBar.updateExp(exp);
        // Debug.Log("I earned " + value + " EXP!");
    }

    private void levelUp()
    {
        exp -= nextLevelRequirements;
        currentLevel++;
        // Debug.Log("I levelled UP to" + currentLevel + " level!");

        //ststs increase:
        gameObject.GetComponent<PlayerMovement>().IncreaseSpeed(speedInreaseValue);
        gameObject.GetComponent<PlayerStatus>().increaseMaxHP(maxHPInreaseValue);
        expBar.updateMaxExp(nextLevelRequirements);
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

}
