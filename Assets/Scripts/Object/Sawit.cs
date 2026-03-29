using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class Sawit : MonoBehaviour
{
    private SpriteRenderer renderer;
    [SerializeField]private Sprite sprite;
    float time = 0; float duration = 10f;
    private float health;
    private Coroutine colorRoutine;



    void Awake()
    {
        renderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        time += Time.deltaTime; float t = time/duration;
        if(t > 1) renderer.sprite = sprite;
    }

    void OnDestroy()
    {
        if(renderer.sprite == sprite && renderer.color == Color.white)
        {
            GlobalData.money += 1;
            Debug.Log($"duitnya ada : {GlobalData.money}");
        }
        else
        {
            Debug.Log("Belum panen tapi di harvest");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Pest"))
        {
            Debug.Log("Sawit ini collide dengan Pest!");
            
            // Hentikan coroutine sebelumnya jika ada agar tidak bentrok
            if (colorRoutine != null) StopCoroutine(colorRoutine);
            
            // Mulai transisi ke Hitam (durasi 4 detik sesuai rumus t = times/4)
            colorRoutine = StartCoroutine(ChangeColor(Color.black, 4f));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Pest"))
        {
            if (colorRoutine != null) StopCoroutine(colorRoutine);
            
            // Mulai transisi balik ke Putih (durasi 2 detik sesuai rumus t = times/2)
            colorRoutine = StartCoroutine(ChangeColor(Color.white, 2f));
        }
    }

    // Fungsi pembantu untuk transisi warna halus
    IEnumerator ChangeColor(Color targetColor, float duration)
    {
        Color startColor = renderer.material.color;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            renderer.material.color = Color.Lerp(startColor, targetColor, t);
            yield return null; // Tunggu frame berikutnya
        }
        
        renderer.material.color = targetColor;
    }
}
