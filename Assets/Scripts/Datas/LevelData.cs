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

    public List<StackData> stackDatas = new List<StackData>();
}

[Serializable]
public class StackData
{
    public List<CardData> cardDatas = new List<CardData>();
}

[Serializable]
public class CardData
{
    public int value;
    public int type;
}
