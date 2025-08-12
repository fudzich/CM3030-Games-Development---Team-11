using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    bool IsInUse();
    float GetCurrentDuration();
    float GetMaxDuration();
    bool IsOnCooldown();
    float GetCurrentCooldown();
    float GetMaxCooldown();
}