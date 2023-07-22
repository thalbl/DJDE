using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HighlightBackground : MonoBehaviour
{
    [SerializeField] private List<Image> backgrounds;
    private List<Color> originalColors;

    public int Idx;

    void Awake()
    {
        this.Idx = 0;
        this.originalColors = new List<Color>();

        foreach(Image bg in this.backgrounds)
            this.originalColors.Add(bg.color);
    }

    public void Highlight(int idx)
    {
        for(int i = 0; i < this.backgrounds.Count; ++i)
        {
            this.backgrounds[i].color = originalColors[i];
        }

        this.backgrounds[idx].color = originalColors[idx] * 2.0f;
    }

    void Update()
    {
        for(int i = 0; i < this.backgrounds.Count; ++i)
            this.backgrounds[i].color = originalColors[i];

        this.backgrounds[this.Idx].color = originalColors[this.Idx] * 2.0f;
    }
}
