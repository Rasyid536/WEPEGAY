using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlobalEvent : MonoBehaviour
{
    [SerializeField] GameObject Pest;
    [SerializeField] TextMeshProUGUI money;
    [SerializeField] GameObject AdjsterUI;


    void Start()
    {
        InvokeRepeating(nameof(InstantiatePest), Random.Range(2, 3), Random.Range(2, 3));
        // test nvim // Instantiate(this.GameObject, new Vector3(1, 2, 0), Quaternion.identity);
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
        /*
        string moneys = "" + GlobalData.money;
        money.text = moneys;

        if (Input.GetButtonDown("i"))
        {
            up();
        }
        if (Input.GetButtonDown("j"))
        {
            left();
        }
        if (Input.GetButtonDown("k"))
        {
            down();
        }
        if (Input.GetButtonDown("l"))
        {
            right();
        }
*/
    }

    public void up(){transform.position += new Vector3(0, 1, 0);}
    public void down(){transform.position += new Vector3(0, -1, 0);}
    public void right(){transform.position += new Vector3(-1, 0, 0);}
    public void left(){transform.position += new Vector3(1, 0, 0);}

    public void zoomout(){transform.position += new Vector3(0, 0, -1);} 
    public void zoomin(){transform.position += new Vector3(0, 0, 1);}

    public void quit(){AdjsterUI.SetActive(false);}
}
