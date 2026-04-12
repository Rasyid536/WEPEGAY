using UnityEngine;

public class AntiPest : MonoBehaviour
{
    private Renderer renderer;
    private int arrayX, arrayY;
    
    float time = 0; float duration = 120f;


    void Awake()
    {
        renderer = this.GetComponent<Renderer>();
        arrayY = Mathf.RoundToInt(this.transform.position.y);
        arrayX = Mathf.RoundToInt(this.transform.position.x);
    }

    void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
        
        time += Time.deltaTime;
        float t = time/duration;
        renderer.material.color = Color.Lerp(Color.white, Color.black, t);
        if(t > 1) Destroy(this.gameObject);
    }

    void OnDestroy() { GlobalVariable.instance.isOccuppied[arrayX, arrayY] = false; }
}
