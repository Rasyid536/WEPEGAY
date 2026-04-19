using System.Collections.Generic;
using UnityEngine;

public static class WordBankData
{
    public static List<string> subject = new()
    {
        "hamster",
        "marmot",
        "fox",
        "squirrel",
        "pig"
    };
    public static List<string> verb = new()
    {
        "jumped",
        "walked",
        "jump",
        "walk",
        "ran",
        "run",
        "fly"
    };
    public static List<string> preposition = new()
    {
        "into", 
        "from",
        "to",
        "towards",
        "away from"
    };
    public static List<string> possessive = new()
    {
        "their",
        "our",
        "my",
        "it",
        "nobody",
        "someone"
    };
    public static List<string> objects = new()
    {
        "home",
        "ground",
        "nests",
        "stuff",
        "place",
    };
}

public class WordBank : MonoBehaviour
{
    public static WordBank instance;

    void Awake()
    {
        instance = this;
    }

    public static string GenerateSentence()
    {
        return $"{GetRandom(WordBankData.subject)} {GetRandom(WordBankData.verb)} {GetRandom(WordBankData.preposition)} {GetRandom(WordBankData.possessive)} {GetRandom(WordBankData.objects)}";
    }

    static string GetRandom(List<string> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    void Update()
    {
        //Debug.Log(GenerateSentence());
    }
}