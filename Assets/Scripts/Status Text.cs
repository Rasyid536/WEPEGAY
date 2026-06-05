using System.Collections;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Kecepatan UI bergerak ke atas")]
    public float moveSpeed = 50f; 
    
    [Tooltip("Lama waktu UI muncul sebelum menghilang (detik)")]
    public float duration = 1f;

    private Vector3 startLocalPosition;
    private bool hasSavedPosition = false;

    void Awake()
    {
        // Simpan posisi awal UI saat game baru mulai.
        // Pakai localPosition agar aman di dalam Canvas.
        startLocalPosition = transform.localPosition;
        hasSavedPosition = true;
    }

    void OnEnable()
    {
        // 1. Reset posisi ke titik awal setiap kali diaktifkan
        if (hasSavedPosition)
        {
            transform.localPosition = startLocalPosition;
        }

        // 2. Mulai animasi bergerak ke atas
        StartCoroutine(FloatAndHide());
    }

    IEnumerator FloatAndHide()
    {
        float timer = 0f;

        // Looping selama waktu timer masih kurang dari duration
        while (timer < duration)
        {
            // Gerakkan ke atas
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            
            // Tambah waktu
            timer += Time.deltaTime;
            
            // Tunggu frame berikutnya sebelum lanjut looping
            yield return null; 
        }

        // 3. Setelah waktu habis, matikan objek ini
        gameObject.SetActive(false);
    }
}