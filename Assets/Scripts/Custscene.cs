using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RhythmNoteController : MonoBehaviour
{
    [Header("Note Reference")]
    public GameObject noteObject;

    [Header("Positions")]
    public Transform startPoint;
    public Transform targetPoint;

    [Header("Movement Options")]
    [Tooltip("Waktu dalam detik yang dibutuhkan note dari start ke target")]
    public float timeToReachTarget = 2f; 

    [Header("Timing Window (in seconds)")]
    public float perfectWindow = 0.01f;
    public float goodWindow = 0.02f;

    [Header("Camera Shake Settings")]
    [Tooltip("Masukkan Main Camera di sini. Jika kosong, script akan otomatis mencari Camera.main")]
    public Camera mainCamera;
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 0.2f;

    private bool isActive = false;
    private float timer = 0f;
    [SerializeField] GameObject cutSceneObj;
    [SerializeField] TextMeshProUGUI statusText; 
    [SerializeField] GameObject TextStatus;
    
    private bool isSpawning = false; 

    void Start()
    {
        noteObject.SetActive(false);

        // Jika kamera belum di-assign di Inspector, otomatis cari Main Camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // Cek juga isSpawning agar coroutine hanya jalan sekali
        if(cutSceneObj.activeInHierarchy && !isSpawning && !isActive)
        {
            isSpawning = true;
            StartCoroutine(WaitAndGo());   
        }

        if (Input.GetKeyDown(KeyCode.S) && !isActive)
            SpawnNote();

        if (!isActive) return;

        timer += Time.deltaTime;

        float t = timer / timeToReachTarget;
        
        noteObject.transform.position = Vector3.LerpUnclamped(startPoint.position, targetPoint.position, t);

        if (timer > timeToReachTarget * 2)
        {
            statusText.text = "MISS";
            TextStatus.SetActive(true);
            Debug.Log("MISS (TERLEWAT)");
            DisableNote();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            CheckHit();

        
        if (cutSceneObj.activeInHierarchy)
            AudioManager.instance.PlayHonkAudio();
    }

    IEnumerator WaitAndGo()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnNote();
    }

    public void SpawnNote()
    {
        timer = 0f;
        noteObject.transform.position = startPoint.position;
        noteObject.SetActive(true);
        isActive = true;
    }

    void CheckHit()
    {
        float timeDifference = Mathf.Abs(timeToReachTarget - timer);

        if (timeDifference <= perfectWindow)  // perfect
        {
            statusText.text = "PERFECT";
            TextStatus.SetActive(true);
            Debug.Log("PERFECT");
            GlobalData.money += 10;
            AudioManager.instance.PlayPerfectAudio();
            // Panggil fungsi Camera Shake di sini
            CameraShake();
        }
        else if (timeDifference <= goodWindow)  // good
        {
            statusText.text = "GOOD";
            TextStatus.SetActive(true);
            Debug.Log("GOOD");
            GlobalData.money += 5;
        }
        else // missed
        {
            statusText.text = "MISS";
            TextStatus.SetActive(true);
            Debug.Log("MISS (TERLALU CEPAT)");
            AudioManager.instance.PlayMissedAudio();
        }

        DisableNote();
    }

    void DisableNote()
    {
        cutSceneObj.SetActive(false);
        noteObject.SetActive(false);
        isActive = false;
        
        // Reset isSpawning agar bisa dipanggil lagi nanti jika dibutuhkan
        isSpawning = false; 
    }

    // ==========================================
    // BAGIAN FUNGSI CAMERA SHAKE
    // ==========================================
    
    public void CameraShake()
    {
        if (mainCamera != null)
        {
            StartCoroutine(ShakeRoutine());
        }
        else
        {
            Debug.LogWarning("Camera Shake gagal: Main Camera belum di-assign atau tidak ditemukan!");
        }
    }

    private IEnumerator ShakeRoutine()
    {
        // Simpan posisi asli kamera sebelum bergetar
        Vector3 originalPosition = mainCamera.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Buat offset posisi secara acak
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            // Terapkan getaran ke kamera
            mainCamera.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            // Tunggu frame berikutnya
            yield return null; 
        }

        // Kembalikan posisi kamera ke posisi semula setelah durasi habis
        mainCamera.transform.localPosition = originalPosition;
    }
}