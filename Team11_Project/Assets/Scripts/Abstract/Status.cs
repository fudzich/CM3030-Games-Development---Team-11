using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    [SerializeField]

    protected float hp;

    public virtual float getCurrentHP() => hp;
}
