using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColourPicker
{
    public static List<Color> ColourPool = new List<Color>();

    public static Color RandomColourFromPool()
    {
        if(ColourPool.Count == 0)
        {
            GeneratePool();
        }
        int i = Random.Range(0, ColourPool.Count);
        return ColourPool[i];
    }

    public static void GeneratePool()
    {
        ColourPool.Add(new Color(230f / 255f, 25f / 255f, 75f / 255f));
        ColourPool.Add(new Color(60f / 255f, 180f / 255f, 75f / 255f));
        ColourPool.Add(new Color(255f / 255f, 225f / 255f, 25f / 255f));
        ColourPool.Add(new Color(0f / 255f, 130f / 255f, 200f / 255f));
        ColourPool.Add(new Color(245f / 255f, 130f / 255f, 48f / 255f));
        ColourPool.Add(new Color(145f / 255f, 30f / 255f, 180f / 255f));
        ColourPool.Add(new Color(70f / 255f, 240f / 255f, 240f / 255f));
        ColourPool.Add(new Color(240f / 255f, 50f / 255f, 230f / 255f));
        ColourPool.Add(new Color(210f / 255f, 245f / 255f, 60f / 255f));
        ColourPool.Add(new Color(250f / 255f, 190f / 255f, 212f / 255f));
        ColourPool.Add(new Color(0f / 255f, 128f / 255f, 128f / 255f));
        ColourPool.Add(new Color(220f / 255f, 190f / 255f, 255f / 255f));
        ColourPool.Add(new Color(170f / 255f, 110f / 255f, 40f / 255f));
        ColourPool.Add(new Color(255f / 255f, 250f / 255f, 200f / 255f));
        ColourPool.Add(new Color(128f / 255f, 0f / 255f, 0f / 255f));
        ColourPool.Add(new Color(170f / 255f, 255f / 255f, 195f / 255f));
        ColourPool.Add(new Color(128f / 255f, 128f / 255f, 0f / 255f));
        ColourPool.Add(new Color(255f / 255f, 215f / 255f, 180f / 255f));
        ColourPool.Add(new Color(0f / 255f, 0f / 255f, 128f / 255f));
        ColourPool.Add(new Color(128f / 255f, 128f / 255f, 128f / 255f));
    }
}
