using UnityEngine;
using TMPro;
using UnityEngine.UI; // Wajib ditambahkan untuk memanggil UI TextMeshPro

public class UIContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text; // Slot untuk memasukkan UI Text
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

        text.text = $"FPS: <color={hex[1]}> {roundedFps} </color> || money: {GlobalData.money} || time scale: {Time.timeScale}";
    }
}