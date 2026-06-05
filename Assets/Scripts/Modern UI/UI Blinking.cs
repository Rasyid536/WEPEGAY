using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIBlinking : MonoBehaviour
{
    private Image image;
    void Start()
    {
        image = GetComponent<Image>();    
    }

   void Update()
    {
        float alpha = Mathf.PingPong(Time.time, 0.7f); 
        
        Color col = image.color;
        col.a = alpha;
        image.color = col;
    }

}
