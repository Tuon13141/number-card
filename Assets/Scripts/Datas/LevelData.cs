using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public List<LayerData> layerDatas = new List<LayerData>();

    public float playTime;
    //public float reward;
}

[Serializable]
public class LayerData
{
    public int width;
    public int height;

    public List<CardData> cardDatas = new List<CardData>();
}

[Serializable]
public class CardData
{
    public int value;
    public int type;
}

[Serializable]
public class SpecialCardData : CardData
{
    public int totalPairCountDownToNormal;
}

[Serializable]
public class BonusCardData : SpecialCardData
{
    public int multiplier;
}

[Serializable]
public class FreezeCardData : SpecialCardData
{
    public float timeFreeze;
}

[Serializable]
public class TimeCardData : SpecialCardData
{
    public float timeAdd;
}

[Serializable]
public class FireworkCardData : SpecialCardData
{
    public int totalPairGetDestroy;
}
