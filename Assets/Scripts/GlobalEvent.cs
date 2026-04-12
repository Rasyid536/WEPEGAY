using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalEvent : MonoBehaviour
{
    [SerializeField] GameObject Pest;
    [SerializeField] TextMeshProUGUI money;


    void Start()
    {
        InvokeRepeating(nameof(InstantiatePest), Random.Range(2, 3), Random.Range(2, 3));
    }

    void InstantiatePest()
    {
        GameObject[] pests = GameObject.FindGameObjectsWithTag("Pest");

        int x = Random.Range(1, 8);
        int y = Random.Range(1, 8);

        bool isPest = false;

        foreach (GameObject obj in pests)
        {
            if (Vector2.Distance(obj.transform.position, new Vector2(x, y)) < 0.1f)
            {
                isPest = true;
                break;
            }
        }
        if (!isPest)
        {
            Instantiate(Pest, GlobalVariable.instance.grid[x, y].transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        string moneys = "" + GlobalData.money;
        money.text = moneys;
    }
}
