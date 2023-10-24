using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public int id;
    public string name;
    public string imageUrl;
}

[System.Serializable]
public class Data
{
    public List<Card> cards = new List<Card>();
}

public class DataManager : ManagerBase
{
    private void Start()
    {
        Load();
    }

    private void Load()
    {
        TextAsset asset = Resources.Load<TextAsset>("Data");
        Data cardDatas  = JsonUtility.FromJson<Data>(asset.text);
    }
}
