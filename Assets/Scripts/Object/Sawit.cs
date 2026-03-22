using UnityEngine;

public class Sawit : MonoBehaviour
{
    public Renderer renderer;
    float time = 0; float duration = 2f; 

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        time += Time.deltaTime;
        float t = time/duration;
        renderer.material.color = Color.Lerp(Color.white, Color.black, t);
    }
}
