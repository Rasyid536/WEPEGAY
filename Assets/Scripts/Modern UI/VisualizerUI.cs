using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VisualizerUI : MonoBehaviour
{
    string plant = "plant_atxy 3 3";
    string antipest = "antipest_atxy 3 3";
    [SerializeField] private GameObject plantObj, antipestObj;
    string erase = "erase_atxy 3 3";
    [SerializeField] private TextMeshProUGUI text;
    private bool isOccupied;
    [SerializeField] private GameObject helpUI;

    void Start()
    {
        text.text = "null";
        isOccupied = false;
    }
    void Update()
    {
        if(!helpUI.activeSelf)
        {
           gameObject.SetActive(false);
        }

        if(!gameObject.activeSelf)
        {
            Erase();
            enter();
        }
    }

    public void Plant()
        {text.text = plant;}
    public void Antipest()
        {text.text = antipest;}
    public void Erase()
        {text.text = erase;}


    public void enter()
    {
        if(text.text == plant)
        {
            if(!isOccupied)
            {
                plantObj.SetActive(true);
                isOccupied = true;
            }
            else
            {
                text.text = "grid still occupied";
            }
        }
        else if(text.text == antipest)
        {
            if(!isOccupied)
            {
                antipestObj.SetActive(true);
                Debug.Log("plant log");
                isOccupied = true;
            }
            else
            {
                text.text = "grid still occupied";
            }
        }
        else if(text.text == erase)
        {
            plantObj.SetActive(false);
            antipestObj.SetActive(false);
            isOccupied = false;
        }
    }    
}
