using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIController : MonoBehaviour
{
    [SerializeField]
    private Image iconFill;

    [SerializeField]
    private MonoBehaviour skillComponent;

    [SerializeField]
    private Image frameImage;

    [SerializeField]
    private Color originalFrameColor = new Color(1f, 0.843f, 0f, 1f);

    private ISkill skill;

    // void Awake()
    // {
    //     if (skillComponent != null && skillComponent is ISkill)
    //     {
    //         skill = (ISkill)skillComponent;
    //     }
    //     else
    //     {
    //         // Debug.LogError("SkillUIController: The assigned skillComponent does not implement ISkill!");
    //     }
    // }

    void Update()
    {
        if (skill == null) return;

        if (skill.IsInUse())
        {
            iconFill.fillAmount = skill.GetCurrentDuration() / skill.GetMaxDuration();
        }
        else
        if (skill.IsOnCooldown())
        {
            float cooldownProgress = 1f - (skill.GetCurrentCooldown() / skill.GetMaxCooldown());
            iconFill.fillAmount = cooldownProgress;
            frameImage.color = Color.gray;
        }
        else
        {
            iconFill.fillAmount = 1f;
            frameImage.color = originalFrameColor;
        }
    }
}