//#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    List<GameObject> stageButtonLists = new List<GameObject>();
    public PlayerData m_cPlayerData = new PlayerData();
    static GameManager Instance = null;  // 싱글톤
    public JoyStick m_cJoyStick;    // 조이스틱 관리
    public GUImanager m_cGUI;
    public Player m_cPlayer;
    public bool GameStart = false;
    public GameObject[] potatos;
    public GameObject Btnstage;
    public GameObject NextCanvers;
    public GameObject StageList;
    public Camera cam;
    Object[] preFabsOb;
    GameObject[] preFabs;
    public Object[] mapFiles;
    public int curStage;
    public int iStage; // 현재 스테이지 
    public bool joyclick = false; // 조이스틱 세팅 유무
    public bool Btnclick = true; //  키패드 세팅 유무
    List<GameObject> stages;
    int stageCount = 1;
    int page = 0;
    bool pageMoving;
    RectTransform rt;
    public Setting m_cSetting;
    public float Level = 6f;  
    Vector3 pinPage;
    //private void OnGUI()
    //{
    //    string jsonPath = Application.streamingAssetsPath;        
    //    GUI.Box(new Rect(300, 400, 500, 600), jsonPath+"\n"+"데이터 패스"+Application.dataPath +"\n"+Application.persistentDataPath);                
    //}
    // Use this for initialization
    void Start()
    {        
        rt = m_cGUI.stageContainer.GetComponent<RectTransform>();
        cam = FindObjectOfType<Camera>();
        preFabsOb = Resources.LoadAll("Prefabs");
        preFabs = new GameObject[preFabsOb.Length];
        for (int i = 0; i < preFabs.Length; i++)
        {
            preFabs[i] = (GameObject)preFabsOb[i];
        }
        m_cPlayer = FindObjectOfType<Player>();
       
        //m_cJoyStick.Player = m_cPlayer.transform;
        //m_cPlayer.transform.parent = null;
        Instance = this;
        //potatos = GameObject.FindGameObjectsWithTag("Potato");
        //if(System.IO.Directory.Exists(Application.dataPath + "/Resources/SaveFile/")) //폴더의 파일 개수 따오기 (C#)
        //{
        //    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.dataPath + "/Resources/SaveFile/");
        //    for (int i = 0; i < di.GetFiles().Length; i++)
        //    {                
        //        //Debug.Log(di.GetFiles()[i].Name);
        //    }
        //}
        mapFiles =  Resources.LoadAll("SaveFile/MapData"); //유니티 상의 리소스 파일만 받아오기(Unity)
        //m_cPlayerData.stage = new bool[mapFiles.Length];
        LoadData(mapFiles.Length); //플레이어의 데이터와 현재 맵의 데이터를 비교       
        //InstantiateButton();
#if DEBUG_LOG
        Debug.Log("세이브 파일 개수 : "+mapFiles.Length);
#endif        
        
    }    

    List<GameObject> GetStage()
    {
        return stageButtonLists;
    }
    
    public static GameManager getInstance()
    {
        return Instance;
    }

    public void PotatoCheck()
    {
        if (m_cPlayer.Potato == potatos.Length)
        {

            m_cGUI.CLEARText.SetActive(true);
            m_cPlayer.PS = PlayerState.CLEAR;
        }
    }

    public void LoadMap(MapContainer _loadMap)
    {
        MapContainer loding = _loadMap;
        for (int i = 0; i < loding.tileDatas.Count; i++)
        {
            string path = "Prefabs/" + loding.tileDatas[i].name;
            GameObject loadMap = Resources.Load<GameObject>(path);
#if DEBUG_LOG
            Debug.Log(loadMap);
            Debug.Log(loding.tileDatas[i].name);
#endif
            if(loadMap != null)
            {
                GameObject insMap = Instantiate(loadMap, loding.tileDatas[i].pos, loding.tileDatas[i].rot);
                insMap.transform.parent = transform;
                insMap.GetComponent<MapData>().rink = loding.tileDatas[i].rink;
                insMap.name = insMap.name + i;
            }
        }
        //m_cPlayer = FindObjectOfType<Player>();
        //m_cJoyStick.Player = m_cPlayer.transform;
       // m_cPlayer.transform.parent = null;
        m_cGUI.StageUI.gameObject.SetActive(false);
        if (joyclick == true)
        {
            m_cGUI.JoyStic.gameObject.SetActive(true); // 조이스틱
            m_cGUI.newJoystick.gameObject.SetActive(false);
        }
        if (Btnclick == true)
        {
            m_cGUI.JoyStic.gameObject.SetActive(false);
            m_cGUI.newJoystick.gameObject.SetActive(true);            
        }
        m_cGUI.GoButton.gameObject.SetActive(true);
        m_cGUI.ScoreText.gameObject.SetActive(true);
        m_cGUI.BG.gameObject.SetActive(false);
        m_cGUI.startbtn.gameObject.SetActive(true);
        cam.depth = -1;        
        GameStart = true;
        // potatos = GameObject.FindGameObjectsWithTag("Potato");
    }
    //public void StageClick(int stagenum, MapContainer mloadMap)
    //{
    //    string json = File.ReadAllText(Application.dataPath + "/Resources/SaveFile/" + stagenum + ".json");
    //    MapContainer loadMap = mloadMap;
    //    GameObject LoadingMap = Resources.Load<GameObject>(json);
    //    for (int i = 0; i < loadMap.tileDatas.Count; i++)
    //    {
    //        for (int mapFiles = 0; mapFiles < preFabs.Length; mapFiles++)
    //        {
    //            if (loadMap.tileDatas[i].name == preFabs[mapFiles].name)
    //            {
    //                GameObject g = (GameObject)Instantiate(preFabs[mapFiles]);
    //                g.transform.position = loadMap.tileDatas[i].pos;
    //                g.transform.parent = transform;
    //                Debug.Log(g.name);
    //            }
    //        }
    //    }
    //    m_cGUI.StageUI.gameObject.SetActive(false);
    //    m_cGUI.JoyStic.gameObject.SetActive(true);
    //    m_cGUI.GoButton.gameObject.SetActive(true);
    //    m_cGUI.ScoreText.gameObject.SetActive(true);        
    //    GameStart = true;
    //}
  public  void ClearMap()
    {
        cam.depth = 1;
        int count = transform.childCount;
       // PluginManager.m_AndroidJavaObject.Call("test", "내 자식들아" + count);
        if (count <= 0)
        {
#if DEBUG_LOG
            Debug.Log("There is nothing to erase");
            Debug.LogError("");
#endif
            return;
        }
        else
        {
#if DEBUG_LOG
            Debug.Log("You Clean " + count + " Objects");
#endif
            for (int i = 0; i < count; i++)
            {
                //Debug.Log("Count"+grid.transform.childCount);
                //Debug.Log("Index:"+i);
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }    
    public void LoadData(int _MapFilesCount)
    {
#if DEBUG_LOG
        Debug.Log("불러와라");        
#endif
        string directory = Application.persistentDataPath + "/PlayerDataFolder";
        string savePath = Application.persistentDataPath + "/PlayerDataFolder/PlayerData.json";
        if(!File.Exists(Application.persistentDataPath + "/PlayerData")) //해당경로에 폴더가 없다면
        {
            Directory.CreateDirectory(directory); //새로 폴더를 만들어준다.(경로에)
        }
        DirectoryInfo folderinFile = new DirectoryInfo(directory);
        if (folderinFile.GetFiles().Length > 0) //플레이어의 세이브 파일이 이미 있다면
        {
            string jsonPath = File.ReadAllText(savePath); //해당 경로에서 플레이어의 데이터를 받아와 string으로 나열
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonPath); //나열한 string을 데이터를 실제 데이터로 만듬
            if (playerData.stage.Length == _MapFilesCount) //로드한 파일 길이와 현재 스테이지 개수를 확인한다. 같다면
            {
#if DEBUG_LOG
                Debug.Log(playerData.stage.Length + "로드한 개수");
                Debug.Log("세이브파일 불러오기");
#endif
                m_cPlayerData = playerData;  //현재 씬의 정보에 데이터를 대입
            }
            else //현재 스테이지 개수와 이전에 저장한 파일에 스테이지 개수가 다르다면
            {
                Debug.Log("개수가 다르다");
#if DEBUG_LOG
                Debug.Log(m_cPlayerData.stage.Length);
#endif
                int lastStage = playerData.lastStage;
                m_cPlayerData.stage = new bool[_MapFilesCount]; //새로 스테이지 길이를 늘려준다.
                for (int i = 0; i < playerData.stage.Length; i++) //현재 스테이지 길이만큼 반복
                {
                    if (playerData.stage[i]) //로드된 데이터에 스테이지를 클리어 했다면
                    {
                        m_cPlayerData.stage[i] = playerData.stage[i];//현재 스테이지 클리어정보를 로드시킨다.
                        if(i == m_cPlayerData.stage.Length - 1) //마지막에 클리어한 스테이지가 어느것인지 확인
                        {
                            m_cPlayerData.lastStage = i; //마지막에 클리어 한 스테이지를 설정해줌
                        }
                    }
                }
                m_cPlayerData.lastStage = lastStage;
                string saveJson = JsonUtility.ToJson(m_cPlayerData);   //Json으로 나열함       
                File.WriteAllText(savePath, saveJson); //다시 저장함
            }
        }
        else //세이브(저장한 파일이 없다면 ->게임을 처음 시작함)
        {
#if DEBUG_LOG
            Debug.Log("세이브파일이 없다.");          
#endif
            m_cPlayerData.stage = new bool[_MapFilesCount]; //세이브 파일을 만들어주기 위해 맵개수만큼 클리어정보를 만들어줌
            string saveJson = JsonUtility.ToJson(m_cPlayerData); //해당 데이터를 string으로 나열
            File.WriteAllText(savePath, saveJson); //해당 경로에 Json파일을 생성            
        }
    }

    private void Update()
    {
        if (!pageMoving)
        {
            rt.anchoredPosition = pinPage;
        }
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        //{
        //    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //    transform.Translate(-touchDeltaPosition.x * spd, -touchDeltaPosition.y * spd, 0);
        //}
    }
    public void SkillUse()
    {
        m_cPlayer.Skill();
    }
    public void InstantiateButton()
    {
        if (stageButtonLists.Count <= 0)
        {
            //Debug.Log(mapFiles.Length % 2);
            for (int i = 0, count = 0, page = 0; i < mapFiles.Length; i++) //스테이지 수 정하기
            {
                count++;
                if (count > 30)
                {
                    stageCount++;
                    page++;
                    count = page; //30스테이지마다 1씩 증가 시켜 30개를 초과 할때 마다 페이지가 정상적으로 만들어지도록 한다.
                }               
            }            
            //if (mapFiles.Length % 2 == 1)
            //{
            //    stageCount++;
            //}
            //스테이지의 수에 비례하게 컨테이너의 크기를 늘이는 코드
            //Debug.Log(stageCount);
            RectTransform rt = m_cGUI.stageContainer.GetComponent<RectTransform>();
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, stageCount * 300);
            rt.GetComponent<GridLayoutGroup>().constraintCount = stageCount;
            m_cGUI.stageContainer.transform.localPosition = new Vector3(stageCount * 300 / 2, m_cGUI.stageContainer.transform.localPosition.y, 0);
            pinPage = rt.anchoredPosition;
            /////////////////////////////////
            stages = new List<GameObject>();
//            for (int i = 0; i < stageCount; i++) //스테이지 수 만큼 스테이지 창 만들기
//            {
//                GameObject stageIns = Instantiate(m_cGUI.stagePrefab, m_cGUI.stageContainer.transform);
//                stages.Add(stageIns);
//            }
//            for (int i = 0, stageIndex = 0, count = 0; i < mapFiles.Length; i++) //스테이지 창에 버튼 만들어주기
//            {
//#if DEBUG_LOG
//                                Debug.Log("세이브 파일 이름 : " + mapFiles[i].name);
//                                Debug.Log("나와라" + i);
//#endif
//                if (count >= 30)
//                {
//                    count = 0;
//                    stageIndex++;
//                }
//                GameObject button = Instantiate(Btnstage) as GameObject;
//                StageBtn sbtn = button.GetComponent<StageBtn>();
//                button.transform.SetParent(stages[stageIndex].transform, false); //바꿔주기
//                sbtn.SetText(count);
//                stageButtonLists.Add(button);
//                count++;
//            }

            for (int i = 0, S = 0, C = 0; i < mapFiles.Length; i++)  // 조건문에 || C<stageCount 이런식으로 넣어서도 사용가능
            {
                if (C < stageCount)
                {
                    GameObject StageIns = Instantiate(m_cGUI.stagePrefab, m_cGUI.stageContainer.transform);
                    stages.Add(StageIns);
                    C++;
                }
                if (stages[S].transform.childCount < 30)
                {
                    GameObject button = Instantiate(Btnstage) as GameObject;
                    StageBtn sbtn = button.GetComponent<StageBtn>();
                    button.transform.SetParent(stages[S].transform, false);
                    sbtn.SetText(i);
                    stageButtonLists.Add(button);
                }
                else if (stages[S].transform.childCount == 30)
                {
                    S++;
                    GameObject button = Instantiate(Btnstage) as GameObject;
                    StageBtn sbtn = button.GetComponent<StageBtn>();
                    button.transform.parent = stages[S].transform;
                    sbtn.SetText(i);
                    stageButtonLists.Add(button);
                }
            }

            // 작업 해야할것
            //if (StageList[S].transform.childCount < 1)
            //{
            //    GameObject button = Instantiate(Btnstage) as GameObject;
            //    StageBtn sbtn = button.GetComponent<StageBtn>();
            //    button.transform.parent = StageList[S].transform;
            //    sbtn.SetText(i);
            //    stageButtonLists.Add(button);
            //}
            //else if (StageList[S].transform.childCount == 1)
            //{
            //    S++;
            //    GameObject button = Instantiate(Btnstage) as GameObject;
            //    StageBtn sbtn = button.GetComponent<StageBtn>();
            //    button.transform.parent = StageList[S].transform;
            //    sbtn.SetText(i);
            //    stageButtonLists.Add(button);
            //}
            StageClearCheck();

            //if (StageList.transform.childCount == 2 )
            //{
            //    GameObject Page2 = Instantiate(NextCanvers) as GameObject;
            //    Page2.gameObject.SetActive(false);
            //    Page2.transform.parent = m_cGUI.BG.transform;
            //    Page2.transform.localPosition = new Vector2(4.62f, -12.1f);
            //    Page2.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //    GameObject button = Instantiate(Btnstage) as GameObject;
            //    StageBtn sbtn = button.GetComponent<StageBtn>();
            //    button.transform.parent = Page2.transform;
            //    sbtn.SetText(i);
            //    stageButtonLists.Add(button);
            //    Debug.Log("다음페이지 생성");
            //}


            //GameObject button = Instantiate(Btnstage) as GameObject;
            //StageBtn sbtn = button.GetComponent<StageBtn>();
            //button.transform.parent = StageList.transform;
            //sbtn.SetText(i);
            //stageButtonLists.Add(button);

            //  button.GetComponent<Button>().onClick.AddListener(() => m_cGUI.Load(i));
        }
    }

    public void StageClearCheck() //스테이지 클리어 여부
    {
#if DEBUG_LOG
        Debug.Log(stageButtonLists.Count+": 버튼리스트");
        Debug.Log(m_cPlayerData.stage.Length+"스테이지리스트");
#endif
        for (int i = 0; i < stageButtonLists.Count; i++)
        {
            int stageNum = i;            
#if DEBUG_LOG
            Debug.Log("스테이지 저장 : " + i);
            Debug.Log(i + "첫 I");
            Debug.Log(stageNum + ":첫 스테이지");
#endif
            if (m_cPlayerData.stage[stageNum] || stageNum == m_cPlayerData.lastStage -1) //한번도 클리어 한 맵이 없다면
            {
#if DEBUG_LOG
                Debug.Log("이게임 처음하는 유저");
                Debug.Log(i + "두번째 I");
                Debug.Log(stageNum + ":두번째 스테이지");
#endif                    
                stageButtonLists[i].GetComponent<Button>().onClick.RemoveAllListeners();
                stageButtonLists[i].GetComponent<Button>().onClick.AddListener(() => m_cGUI.Load(stageNum)); //1스테이지를 열어준다.
                stageButtonLists[i].GetComponent<Image>().enabled = true;
                stageButtonLists[i].GetComponentInChildren<Text>().enabled = true;
                stageNum++;
                if(m_cPlayerData.stage[i] == true)
                {
                    stageButtonLists[i].GetComponent<Image>().color = Color.green;
                }
            }
            else
            {
                stageButtonLists[i].GetComponent<Button>().onClick.RemoveAllListeners();
                stageButtonLists[i].GetComponent<Button>().onClick.AddListener(() => m_cGUI.Load(-1)); //에러용
                stageButtonLists[i].GetComponent<Image>().enabled = false;
                stageButtonLists[i].GetComponentInChildren<Text>().enabled = false;
            }           
        }
    }
    public void GameOverRe()
    {
        ClearMap();
        m_cPlayer.PotatoScore = 0;
        m_cGUI.GameOver.gameObject.SetActive(false);
        m_cGUI.StageUI.gameObject.SetActive(true);
        m_cGUI.CurStage.gameObject.SetActive(false);
        m_cGUI.Timer.gameObject.SetActive(false);
        m_cGUI.ScoreText.gameObject.SetActive(false);
        m_cGUI.newJoystick.gameObject.SetActive(false);
        m_cGUI.Start.gameObject.SetActive(false);
        m_cGUI.BG.gameObject.SetActive(true);       
    }
    public void NextPage(int i)
    {
        if (m_cGUI.StageUI.activeInHierarchy && !pageMoving)
        {
            if(i == 0)
            {
                Debug.Log("다음");                
                if(rt.anchoredPosition.x != (-150 * (stageCount-1)))
                {
                    page++;
                    //m_cGUI.stageContainer.transform.localPosition = Vector3.Lerp(m_cGUI.stageContainer.transform.localPosition,new Vector3(-300f,m_cGUI.stageContainer.transform.localPosition.y,0),3f);
                    StartCoroutine(PageMove(i));
                    pageMoving = true;
                }
            }
            else
            {
                Debug.Log("이전");                
                if(rt.anchoredPosition.x != (150 * (stageCount-1)))
                {
                    page--;
                    StartCoroutine(PageMove(i));
                    pageMoving = true;
                    //m_cGUI.stageContainer.transform.localPosition = Vector3.Lerp(m_cGUI.stageContainer.transform.localPosition, new Vector3(300f, m_cGUI.stageContainer.transform.localPosition.y, 0), 3f);
                }
            }
        }        
    }
    IEnumerator PageMove(int i)
    {
        bool go = true;
        //Vector3 curpos =new Vector3(m_cGUI.stageContainer.transform.localPosition.x,0,0);        
        //RectTransform rt = m_cGUI.stageContainer.GetComponent<RectTransform>();
        Vector3 setpos = Vector3.zero;
        switch (i)
        {
            case 0:
                {                    
                    setpos = new Vector3(rt.anchoredPosition.x - 300f, rt.anchoredPosition.y, 0);                                        
                    break;
                }
            case 1:
                {
                    setpos = new Vector3(rt.anchoredPosition.x + 300f, rt.anchoredPosition.y, 0);                                       
                    break;
                }
        }
        while (go)
        {          
            if(Vector3.Distance(new Vector3(rt.anchoredPosition.x, rt.anchoredPosition.y, 0), setpos) > 0.1f)
            {
                rt.anchoredPosition = Vector3.MoveTowards(new Vector3(rt.anchoredPosition.x,rt.anchoredPosition.y,0), setpos,20f);
                pinPage = rt.anchoredPosition;
            }
            else
            {
                go = false;
                pageMoving = false;               
            }                                                                                                                                         
            yield return null;
        }
        yield return null;
    }    
}
