using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalEvent : MonoBehaviour
{
    [SerializeField] GameObject Pest;
    [SerializeField] TextMeshProUGUI money;
    [SerializeField] private GameObject HelpUI;
    [SerializeField] private GameObject cutsSceneObj;[SerializeField] private bool isStart;

    void Start()
    {
        isStart = true;
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
        if (!isPest && GridManager.instance.isGridGenerated && GlobalVariable.gameState)
        {
            if(GlobalVariable.instance.grid[x, y] != null)
                Instantiate(Pest, GlobalVariable.instance.grid[x, y].transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        if(GlobalData.plantrow == Random.Range(5, 7) && isStart == true)
        {
            cutsSceneObj.SetActive(true);
            GlobalData.plantrow = 0;
            isStart = false;
        }
        else if(GlobalData.plantrow == Random.Range(10, 15) && isStart == false)
        {
            cutsSceneObj.SetActive(true);
            GlobalData.plantrow = 0;
        }

        if(Input.GetKeyDown(KeyCode.Z)) cutsSceneObj.SetActive(true);


        if (Input.GetKeyDown("i"))
        {
            up();
        }
        if (Input.GetKeyDown("j"))
        {
            left();
        }

        if (Input.GetKeyDown("k"))
        {
            down();
        }
        if (Input.GetKeyDown("l"))
        {
            right();
        }

        if(!GlobalVariable.gameState)
            HelpUI.SetActive(true);
        else
            HelpUI.SetActive(false);
    }

    public void up(){transform.position += new Vector3(0, -1, 0);}
    public void down(){transform.position += new Vector3(0, 1, 0);}
    public void right(){transform.position += new Vector3(-1, 0, 0);}
    public void left(){transform.position += new Vector3(1, 0, 0);}

    public void zoomout(){transform.position += new Vector3(0, 0, -1);} 
    public void zoomin(){transform.position += new Vector3(0, 0, 1);}
}
