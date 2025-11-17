using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialCardConfig", menuName = "Scriptable Objects/Special Card Config")]
public class SpecialCardConfig : ScriptableObject
{
    public List<SpecialCardSetting> specialCards;
}

[Serializable]
public class SpecialCardSetting
{
    public string name;
    public string description;
    public int specialValue;
    public Vector2Int unlockRangeCount = new Vector2Int(1, 1);
}


