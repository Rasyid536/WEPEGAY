using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUtama; // Slot untuk tulisan panjang
    [SerializeField] private TextMeshProUGUI textAngkaTombol; // Slot untuk teks angka di dalam tombol
    [SerializeField] private RectTransform timeScaleButton; // Slot untuk tombol merah
    
    private float deltaTime = 0.0f;
    private string[] hex = new string[5]; 

    void Start()
    {
        hex[1] = "white";
    }

    void Update()
    {   
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float fps = 1.0f / deltaTime;
        int roundedFps = Mathf.CeilToInt(fps);

        if(fps <= 30)
            hex[1] = "red";
        else if(fps > 30 && fps <= 60)
            hex[1] = "blue";
        else
            hex[1] = "green";

        textUtama.text = $"FPS: <color={hex[1]}> {roundedFps} </color> || money: {GlobalData.money} || time scale: ";
        textAngkaTombol.text = Time.timeScale.ToString();

        textUtama.ForceMeshUpdate(); 

        float textEndPosition = textUtama.preferredWidth;

        timeScaleButton.anchoredPosition = new Vector2(textEndPosition, timeScaleButton.anchoredPosition.y);
    }

    public void TimeScaling()
    {
        Time.timeScale = 1;
    }
}