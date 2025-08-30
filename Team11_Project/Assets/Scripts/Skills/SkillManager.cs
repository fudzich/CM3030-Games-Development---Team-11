using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Unlock Levels")]
    [SerializeField] private int learnStealthLv = 2;
    [SerializeField] private int learnFireBallLv = 3;
    [SerializeField] private int learnTornadoLv = 4;
    [SerializeField] private int learnHealLv = 5;

    [Header("UI")]
    [SerializeField] private GameObject StealthUI;
    [SerializeField] private GameObject FireBallUI;
    [SerializeField] private GameObject TornadoUI;
    [SerializeField] private GameObject HealUI;

    [Header("Skill Levels (0–4)")]
    [SerializeField, Range(0, 4)] private int stealthLevel = 0;
    [SerializeField, Range(0, 4)] private int fireballLevel = 0;
    [SerializeField, Range(0, 4)] private int tornadoLevel = 0;
    [SerializeField, Range(0, 4)] private int healLevel = 0;

    private const int MaxSkillLevel = 4;

    public void learnSkillbyLv(int level)
    {
        if (level == learnStealthLv)
        {
            GetComponent<Stealth>().EnableSkill();
            StealthUI.SetActive(true);
            stealthLevel = Mathf.Max(stealthLevel, 1); // ensure at least level 1 on unlock
            return;
        }
        else if (level == learnFireBallLv)
        {
            GetComponent<FireSkill>().EnableSkill();
            FireBallUI.SetActive(true);
            fireballLevel = Mathf.Max(fireballLevel, 1);
            return;
        }
        else if (level == learnTornadoLv)
        {
            GetComponent<TornadoSkill>().EnableSkill();
            TornadoUI.SetActive(true);
            tornadoLevel = Mathf.Max(tornadoLevel, 1);
            return;
        }
        else if (level == learnHealLv)
        {
            GetComponent<Heal>().EnableSkill();
            HealUI.SetActive(true);
            healLevel = Mathf.Max(healLevel, 1);
            return;
        }

        RandomLevelUpOneSkill();
    }

    private void RandomLevelUpOneSkill()
    {
        // Build a list of skills that aren't maxed yet
        var candidates = new List<System.Action>();

        if (stealthLevel < MaxSkillLevel)
            candidates.Add(() => { stealthLevel++; OnSkillLevelChanged("Stealth", stealthLevel); });

        if (fireballLevel < MaxSkillLevel)
            candidates.Add(() => { fireballLevel++; OnSkillLevelChanged("FireBall", fireballLevel); });

        if (tornadoLevel < MaxSkillLevel)
            candidates.Add(() => { tornadoLevel++; OnSkillLevelChanged("Tornado", tornadoLevel); });

        if (healLevel < MaxSkillLevel)
            candidates.Add(() => { healLevel++; OnSkillLevelChanged("Heal", healLevel); });

        if (candidates.Count == 0)
        {
            // All skills already at cap — nothing to do.
            return;
        }

        // Pick one at random and level it up
        int pick = Random.Range(0, candidates.Count);
        candidates[pick].Invoke();
    }
    private void OnSkillLevelChanged(string skillName, int newLevel)
    {
        switch (skillName)
        {
            case "Stealth":
                {
                    SendMessage("OnStealthLevelChanged", newLevel, SendMessageOptions.DontRequireReceiver);
                    ToggleLevelFills(StealthUI.transform, newLevel);
                    break;
                }
            case "FireBall":
                {
                    SendMessage("OnFireBallLevelChanged", newLevel, SendMessageOptions.DontRequireReceiver);
                    ToggleLevelFills(FireBallUI.transform, newLevel);
                    break;
                }
            case "Tornado":
                {
                    SendMessage("OnTornadoLevelChanged", newLevel, SendMessageOptions.DontRequireReceiver);
                    ToggleLevelFills(TornadoUI.transform, newLevel);
                    break;
                }
            case "Heal":
                {
                    SendMessage("OnHealLevelChanged", newLevel, SendMessageOptions.DontRequireReceiver);
                    ToggleLevelFills(HealUI.transform, newLevel);
                    break;
                }
        }
        // Debug.Log($"[{skillName}] leveled up to {newLevel}");
    }
    private void ToggleLevelFills(Transform uiRoot, int level)
    {
        var fill = uiRoot.Find($"Level/{level}/Fill");
        if (fill != null) fill.gameObject.SetActive(true);
    }

}

