using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIContainer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI mainTextLineSec;
    [SerializeField] private TextMeshProUGUI mainTextLineThird; 
    [SerializeField] private TextMeshProUGUI stateStatus;
    [SerializeField] private RectTransform timeScaleButton;
    [SerializeField] private GameObject palmSprite, palmSpriteErase, pestSprite, antiPestSprite;
    
    private float deltaTime = 0.0f;
    private string[] hex = new string[5]; 
    float spriteBlink = 0;

    void Start()
    {
        GlobalVariable.gameState = false;
        hex[1] = "white";
    }

    void Update()
    {   
        spriteBlink += Time.deltaTime;
        bool showSprite = (int)(spriteBlink * 2) % 2 == 0;
        
        /////////////////////////////////////////////////
        palmSprite.SetActive(showSprite? true : false);
        palmSpriteErase.SetActive(showSprite? true : false);
        antiPestSprite.SetActive(showSprite? true : false);
        pestSprite.SetActive(showSprite? true : false);
        //////////////////////////////////////////////////


        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float fps = 1.0f / deltaTime;
        int roundedFps = Mathf.CeilToInt(fps);

        if(fps <= 30)
            hex[1] = "red";
        else if(fps > 30 && fps <= 60)
            hex[1] = "blue";
        else
            hex[1] = "green";

        mainText.text = $"FPS: <color={hex[1]}> {roundedFps} </color> || money: {GlobalData.money} || game state: ";
        mainTextLineSec.text = $"pest amount : {GlobalVariable.pestAmount} || palm tree : {GlobalVariable.palmTreeAmount} || antipest : {GlobalVariable.antiPestAmount}";
        mainTextLineThird.text = $"grid at : {getTilePointedCursor.cursorAt} || Force Focus : not made yet"; 


        stateStatus.text = GlobalVariable.gameState ? "1" : "0";

        mainText.ForceMeshUpdate(); 

        float textEndPosition = mainText.preferredWidth;

        timeScaleButton.anchoredPosition = new Vector2(textEndPosition, timeScaleButton.anchoredPosition.y);
    }

    public void TimeScaling()
    {
        if (GlobalVariable.gameState)
            GlobalVariable.gameState = false;
        else
            GlobalVariable.gameState = true;
    }
}