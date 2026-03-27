using UnityEngine;

public class GlobalEvent : MonoBehaviour
{
    [SerializeField]GameObject Pest;


    void Start()
    {
        InvokeRepeating(nameof(InstantiatePest), Random.Range(5, 8), Random.Range(5, 8));
    }

    void InstantiatePest()
    {
        Instantiate(Pest, GlobalVariable.instance.grid[Random.Range(1, 8), 
        Random.Range(1, 8)].transform.position, Quaternion.identity);
    }
}
