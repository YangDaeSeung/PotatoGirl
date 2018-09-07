//#define DEBUG_LOG

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUImanager : MonoBehaviour
{

    public Text ScoreText;
    public GameObject CLEARText;
    public Button saveButton;
    public Button loadButton;
    public InputField stageNumber;
    public GameObject StageUI;
    public GameObject JoyStic;
    public GameObject GoButton;
    public ClearUI m_cClearUI;

    public Image PotatoTile1;
    public Image PotatoTile2;
    public Text Name;
    public Button Start;
    public GameObject BG;
    public Text GameOver;
    public Text Timer;
    public Text CurStage;

    public GameObject stageContainer;
    public GameObject stagePrefab;

    public GameObject SettingCanvers;

    public GameObject potatoStage;
    public GameObject newJoystick;

    public Button pausebtn;
    public Button startbtn;
    public GameObject pause;
    //public Button rebtn;
    //public Button continuebtn;
    //public Button exit;
    public bool pauseon = false;

    public void Save()
    {
        SaveMapData.SavingData();
    }
    //버튼에서 매개변수를 받아와서 변수에 맞는 스테이지를 찾아서 로드하도록 만들어야 함.
    public void Load(int stage)
    {
        if (stage < 0 || stage > GameManager.getInstance().m_cPlayerData.stage.Length) //스테이지값이 비정상적인 경우
        {
            return;
        }
        GameManager.getInstance().curStage = stage; //누른 스테이지를 현재 진행중인 스테이지로
        GameManager.getInstance().iStage = stage;
        SaveMapData.LoadingData(stage); //누른 스테이지를 로드한다.
        if (stage == 30)
        {
            potatoStage.SetActive(true);
        }
        //Debug.Log("스테이지 소환 : " + stage);
#if DEBUG_LOG
#endif
    }
    public void GameStart()
    {
        Start.gameObject.SetActive(false);
        StageUI.gameObject.SetActive(true);
        GameManager.getInstance().InstantiateButton();
    }
    public void stop()
    {
        pausebtn.gameObject.SetActive(true);
        startbtn.gameObject.SetActive(false);
        pause.SetActive(true);
        pauseon = true;
    }
    public void GameRe()
    {
        string json = Resources.Load("SaveFile/MapData/" + GameManager.getInstance().iStage).ToString();
        MapContainer loadMap = JsonUtility.FromJson<MapContainer>(json);
        GameManager.getInstance().ClearMap();
        GameManager.getInstance().LoadMap(loadMap);
        GameManager.getInstance().m_cGUI.ScoreText.text = "SCORE :" + 0;
        pausebtn.gameObject.SetActive(false);
        pause.SetActive(false);
        pauseon = false;
    }
    public void Continue()
    {
        pausebtn.gameObject.SetActive(false);
        startbtn.gameObject.SetActive(true);
        pause.SetActive(false);
        pauseon = false;
    }
    public void EndGame()
    {
   
            Application.Quit();

    }
}
