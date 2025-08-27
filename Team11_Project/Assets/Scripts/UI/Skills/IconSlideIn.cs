using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSlideIn : MonoBehaviour
{
    public float duration = 0.5f;
    public float offsetY = 100f;

    RectTransform IconSet;
    Vector2 endPos;
    Vector2 startPos;
    float timer;

    void Awake()
    {
        IconSet = GetComponent<RectTransform>();
        endPos = IconSet.anchoredPosition;
    }

    void OnEnable()
    {
        timer = 0f;
        startPos = endPos + new Vector2(0f, -Mathf.Abs(offsetY));
        IconSet.anchoredPosition = startPos;
    }

    void Update()
    {
        if (timer >= duration) return;
        timer += Time.deltaTime;
        float p = Mathf.Clamp01(timer / duration);
        IconSet.anchoredPosition = Vector2.Lerp(startPos, endPos, p);
    }
}
