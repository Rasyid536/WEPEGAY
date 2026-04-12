using System.Collections.Generic;
using UnityEngine;

public static class WordBankData
{
    static List<string> subject = new()
    {
        "hamster",
        "marmot",
        "fox",
        "squirrel",
        "pig"
    };
    static List<string> verb = new()
    {
        "jumped",
        "walked",
        "jump",
        "walk",
        "ran",
        "run",
        "fly"
    };
    static List<string> preposition = new()
    {
        "into", 
        "from",
        "to",
        "towards",
        "away from"
    };
    static List<string> posessive = new()
    {
        "their",
        "our",
        "my",
        "it",
        "nobody",
        "someone"
    };
    static List<string> onject = new()
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
}