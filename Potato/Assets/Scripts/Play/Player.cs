#if UNITY_EDITOR
//#define DEBUG_LOG
#endif

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PlayerState
{
    IDLE = 0,
    MOVE = 1,
    ATTACK = 2,
    DIE = 3,
    CLEAR = 4,
}
public class Player : MonoBehaviour
{
    public static Player ins;
    public Vector3 rayvec;
    public float Speed = 0.5f;
    public bool b_Move;
    public bool b_JoyStic;
    public int PotatoScore;
   // public float RayCastValue; //확인용일때만 사용
    public PlayerState PS;
    public GameObject[] potatos;
    public Camera cam;
    public int Potato;
    public float Timer = 5f;
    public bool GetPotato;
    public GameObject target = null;
    public Animator Anim;
    GameObject paticleins;


    private AudioSource myAudio;
    public AudioClip[] myClips;
    GameManager gm = GameManager.getInstance();
    public bool debugMode;
    public bool useSkill;
    // Use this for initialization

    void Start () {
        myAudio = GetComponent<AudioSource>();
        //paticleins = Instantiate(potatoPaticle);
        if(ins == null)
        {
            ins = this;
        }
        else if(ins != this)
        {
            DestroyImmediate(gameObject);
        }
        cam = transform.GetComponentInChildren<Camera>();
        PS = PlayerState.IDLE;
        Anim = GetComponent<Animator>();

        if(gm.GameStart == true)
        {
            potatos = GameObject.FindGameObjectsWithTag("Potato");
            Timer = 5f; // 게임과 시작하면 초기값 5를 받음
            gm.m_cGUI.Timer.gameObject.SetActive(true);
            gm.m_cGUI.Timer.text = "" + (int)Timer;
            gm.m_cGUI.CurStage.gameObject.SetActive(true);
            gm.m_cGUI.CurStage.text = "STAGE : "+ gm.iStage;
            PotatoScore = 0;
            StartCoroutine("StartGame");
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0, 0.1f), transform.forward, out hit, 0.3f, 1 << LayerMask.NameToLayer("Potato")) ||
                Physics.Raycast(transform.position + new Vector3(0, 0, 0.1f), transform.forward, out hit, 0.3f, 1 << LayerMask.NameToLayer("Potal")))
            {
                target = hit.transform.gameObject;
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
        CheckingObj();
        //GameObject[] gg = GoRay();        
    }
    GameObject [] GoRay()
    {
        RaycastHit[] hits;
        RaycastHit hitWall;
        Vector3 wallPos;
        GameObject[] slashs;
        if(Physics.Raycast(transform.position, transform.forward, out hitWall, Mathf.Infinity, LayerMask.GetMask("Wall")))
        {
            hits = Physics.RaycastAll(transform.position + new Vector3(0, 0.2f, 0), transform.forward, Vector3.Distance(transform.position,hitWall.transform.position) ,1 << LayerMask.NameToLayer("Potato"));
            Debug.DrawRay(transform.position + new Vector3(0, 0.2f, 0), transform.forward, Color.red, Vector3.Distance(transform.position, hitWall.transform.position));
            if(hits.Length > 1)
            {
                slashs = new GameObject[hits.Length];
                for (int i = 0; i < hits.Length; i++)
                {
                    slashs[i] = hits[i].transform.gameObject;
                    //Debug.Log(slashs[i]);
                }
                return slashs;
            }            
        }        
        return null;
    }
    public void Skill()
    {
        if (!useSkill)
        {
            
            int farOb = 0;
            GameObject[] slashPotatos = GoRay();            
            if(slashPotatos != null)
            {
                for (int i = 0; i < slashPotatos.Length; i++)
                {
                    if(Vector3.Distance(transform.position,slashPotatos[farOb].transform.position) < Vector3.Distance(transform.position, slashPotatos[i].transform.position))
                    {
                        farOb = i;
                    }
                }
                target = slashPotatos[farOb];
                Vector3 Dis = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
                transform.rotation = Quaternion.LookRotation(Dis);
                //Debug.Log(slashPotatos.Length);
                StartCoroutine(SkillMove(slashPotatos,farOb));
                useSkill = true;
            }        
        }
    }
    IEnumerator SkillMove(GameObject [] slash, int _FarOb)
    {
        bool isMove = true;        
        float [] dists = new float[slash.Length];
        myAudio.clip = myClips[2];
        myAudio.PlayOneShot(myAudio.clip);
        yield return new WaitForSeconds(0.5f);
        Anim.SetBool("Slash", true);
        while (isMove)
        {            
            for (int i = 0; i < dists.Length; i++)
            {
                dists[i] = Vector3.Distance(new Vector3(slash[i].transform.position.x, 0, slash[i].transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
                if(dists[i] < 0.07f)
                {
                    if (slash[i].activeInHierarchy)
                    {
                        slash[i].SetActive(false);
                        if (myAudio.clip == null)
                        {
                            myAudio.clip = myClips[0];
                        }
                        if (myAudio.clip == myClips[0])
                        {
                            myAudio.clip = myClips[1];
                        }
                        else
                        {
                            myAudio.clip = myClips[0];
                        }                        
                        //Potato += slash.Length;
                        myAudio.PlayOneShot(myAudio.clip);
                    }
                }
            }
            if (Vector3.Distance(new Vector3(slash[_FarOb].transform.position.x, 0, slash[_FarOb].transform.position.z), new Vector3(transform.position.x, 0, transform.position.z)) > 0.05f)
            {
                transform.Translate(Vector3.forward * Speed *10f* Time.deltaTime);
            }            
            else
            {
                transform.position = new Vector3(slash[_FarOb].transform.position.x, transform.position.y, slash[_FarOb].transform.position.z);
                for (int i = 0; i < slash.Length; i++)
                {
                    if (slash[i].activeInHierarchy)
                    {
                        slash[i].SetActive(false);
                    }
                }
                //Debug.Log("스킬끝");
                Potato += slash.Length;
                PotatoScore += (slash.Length * 30);
                gm.m_cGUI.ScoreText.text = "SCORE : " + PotatoScore;
                Debug.Log(slash.Length * 30);
                PotatoCheck();
                isMove = false;
                target = null;
                Timer = 6f;
                GetPotato = true;
                if(potatos != null)
                {
                    if (Potato != potatos.Length)
                    {
                        NotPotato();
                    }
                }
                Anim.SetBool("Slash", false);
                useSkill = false;
            }            
            yield return null;
        }
    }
    void CheckingObj()
    {
        float dis; // 거리 
        RaycastHit hit;
        // Debug.DrawRay(transform.position + new Vector3(0, transform.localScale.y, 0), transform.forward, Color.blue, 0.5f); // 사용하면 안된다.
        //Debug.DrawRay(transform.position + new Vector3(0, 0, transform.localScale.z), transform.forward, Color.blue, 0.3f);
        switch (PS)
        {
            case PlayerState.IDLE:
                {
                    Anim.SetBool("Run", false);
                    Anim.SetBool("Die", false);
                    if (b_JoyStic || target != null) // 조이스틱이 움직이는 중이라면
                    {                        
                        //레이캐스트를 쐈을때 감자라는 이름이있다면 
                        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0.1f), transform.forward, out hit, 0.3f,LayerMask.GetMask("Potato","Potal")))                            
                        {
                            target = hit.transform.gameObject;                            
                        }
                        else if(!b_JoyStic)
                        {                        
                            Vector3 Dis = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
                            transform.rotation = Quaternion.LookRotation(Dis);
                        }                   
                        else
                        {
                            target = null;                            
                        }
                    }
                    if (GetPotato == true)
                    {
                        if (GameManager.getInstance().m_cGUI.pauseon == false)
                        {
                            Defeat(); // 여기다가 둔이유는 아이들 상태에서만 시간초가 줄어야하니까 
                        }
                    }
                    break;
                }
            case PlayerState.MOVE:
                {                    
                    GetPotato = true;
                    Anim.SetBool("Run", true);
                    dis = Vector3.Distance(new Vector3(target.transform.position.x, 0, target.transform.position.z),new Vector3(transform.position.x, 0, transform.position.z));
                    if (dis > 0.05f) // 0.6f > 0.3f
                    {
#if DEBUG_LOG
                        Debug.Log("나의거리 : " + dis);
                        Debug.Log("움직여라");
#endif
                        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                    }
                    else
                    {
#if DEBUG_LOG
                        Debug.Log("감자이동");
                        Debug.Log(target.layer);
                        Debug.Log(LayerMask.NameToLayer("Potal"));
#endif                                                         
                        transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                        target.SetActive(false); //포탈를 없앤다.
                        if (!target.GetComponentInParent<MeshRenderer>().enabled)
                        {
                            target.GetComponentInParent<MeshRenderer>().enabled = true;
                            Debug.Log("z켜라");
                        }
                                               
                        if(target.layer == LayerMask.NameToLayer("Potal")) //타겟이 포탈이면
                        {
                            GameObject [] potals = GameObject.FindGameObjectsWithTag(target.tag); //현재 포탈의 태그와 같은 포탈을 다 받아온다.
                            //Debug.Log(potals.Length);
                            if(potals != null)
                            {
                                for (int i = 0; i < potals.Length; i++)
                                {                                                                      
                                    if(potals[i].activeInHierarchy && target.GetComponentInParent<MapData>().rink == potals[i].GetComponentInParent<MapData>().rink)
                                    {
                                        target = potals[i];
                                        Debug.Log("포탈이동"+target.name);
                                    }
                                    //Shader de = Shader.Find("FX/Water");
                                    //Material dd = new Material(de);
                                    //Debug.Log(dd.GetColor("_RefrColor"));
                                    //dd.SetColor("_RefrColor", Color.red);
                                    //Debug.Log(dd.GetColor("_RefrColor"));
                                    //dd.SetColor("_RefrColor", Color.blue);
                                    //Debug.Log(dd.GetColor("_RefrColor"));
                                    //dd.SetColor("_RefrColor", Color.white);
                                    //Debug.Log(dd.GetColor("_RefrColor"));
                                }
                                MeshRenderer me =target.GetComponentInParent<MeshRenderer>();
                                transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z); //이동한다.
                                target.SetActive(false);
                                if (!target.GetComponentInParent<MeshRenderer>().enabled)
                                {
                                    target.GetComponentInParent<MeshRenderer>().enabled = true;
                                    Debug.Log("z켜라");
                                }
                            }
                        }
                        
                        gm.m_cGUI.ScoreText.text = "SCORE : " + PotatoScore;

                        PotatoScore += 30;
                        Potato += 1;
                        Timer = 6;
                        //paticleins.transform.position = target.transform.position;
                        target = null;
                        PS = PlayerState.IDLE;
                        if(myAudio.clip == null)
                        {
                            myAudio.clip = myClips[0];
                        }
                        if(myAudio.clip == myClips[0])
                        {
                            myAudio.clip = myClips[1];
                        }
                        else
                        {
                            myAudio.clip = myClips[0];
                        }
                        myAudio.PlayOneShot(myAudio.clip);
                        PotatoCheck();
                        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0.1f), transform.forward, out hit, 0.3f, 1 << LayerMask.NameToLayer("Potato"))||
                            Physics.Raycast(transform.position + new Vector3(0, 0, 0.1f), transform.forward, out hit, 0.3f, 1 << LayerMask.NameToLayer("Potal")))
                        {
                            target = hit.transform.gameObject;
                        }
                        if(potatos != null)
                        {
                            if (Potato != potatos.Length)
                            {
                                NotPotato();
                            }
                        }
                    }
                    break;
                }
            case PlayerState.CLEAR:
                {
                    Anim.SetBool("Clear", true);
                    gm.m_cGUI.m_cClearUI.ClearScore.GetComponent<Text>().text = "점수 : " + gm.m_cPlayer.PotatoScore;
                    break;
                }
            case PlayerState.DIE:
                {
                    Anim.SetBool("Die", true);
                    gm.m_cGUI.GameOver.transform.localPosition = transform.position + new Vector3(5f, -75f);
                    gm.m_cGUI.GameOver.gameObject.SetActive(true);
                    gm.m_cGUI.Timer.gameObject.SetActive(false);
                    cam.transform.LookAt(transform.position);
                    cam.transform.RotateAround(transform.position, new Vector3(0, 2f, 3f), transform.rotation.z);
                    break;
                }
        }
    }
    
    public void PotatoCheck()
    {
        if (Potato == potatos.Length)
        {
            PS = PlayerState.CLEAR;
            gm.GameStart = false;
            gm.m_cGUI.m_cClearUI.UI.gameObject.SetActive(true);
            gm.m_cPlayerData.stage[gm.curStage-1] = true; //불러온 데이터에서 현재 스테이지를 불러와 클리어 시킨다.
            if(gm.curStage == 30)
            {
                gm.m_cGUI.potatoStage.SetActive(false);
            }
            if (gm.m_cPlayerData.lastStage == gm.curStage)
            {
                gm.m_cPlayerData.lastStage = gm.curStage + 1;
            }

            Debug.Log("Save");                    
            //string stageNum = i.ToString();
                    
            string saveJson = JsonUtility.ToJson(gm.m_cPlayerData); //데이터를 모두 불러온다.
            string savePath = Application.persistentDataPath + "/PlayerDataFolder/PlayerData.json"; //세이브 경로,파일이름.확장자       
            File.WriteAllText(savePath, saveJson); //저장한다.
            gm.StageClearCheck(); //버튼 정보 갱신
            potatos = null;           
        }
        //if(PS != PlayerState.CLEAR)
        //{
        //    Potatohit();
        //}
    }
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.red;
            // Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawWireCube(transform.position,new Vector3(0.25f,0f,0.25f));           
        }


    }
    void NotPotato()   //오버랩스피어(감자체크)
    {
        //Debug.Log("감자없다");
        Collider[] hitColliders = Physics.OverlapBox(transform.position, new Vector3(0.15f, 0, 0.15f), Quaternion.Euler(0f, 45f, 0), LayerMask.GetMask("Potato", "Potal"));//Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask("Potato","Potal"));
        //Debug.Log("감자 갯수" + hitColliders.Length);
        if (target == null)
        {
            //Debug.Log("감자없다2");

            if (hitColliders.Length == 0)
            {
                PS = PlayerState.DIE;
                //Debug.Log("감자없다3");
            }
        }
        //if (hitColliders.Length > 0)
        //{
        //    OverRapPotato[0] = hitColliders[0];
        //}
    }
    public IEnumerator StartGame()
    {
        StartCoroutine(CameraTimer(GameManager.getInstance().Level+1));
        yield return new WaitForSeconds(GameManager.getInstance().Level);
#if DEBUG_LOG
        Debug.Log("게임시작");
#endif
        gm.m_cPlayer = this.GetComponent<Player>();
        gm.m_cJoyStick.Player = this.GetComponent<Transform>();
        cam.depth = 2;

        yield return null;
    }
    IEnumerator CameraTimer(float _timer)
    {
        bool timer = true;
        float time = _timer;
        while (timer)
        {
            time -= Time.deltaTime;
            gm.m_cGUI.Timer.text =((int)time).ToString();
            if(time <= 1)
            {
                timer = false;
                gm.m_cGUI.Timer.text = 5.ToString();
            }
            yield return null;
        }
    }
    public void Defeat() // 타이머 
    {
        if(Timer > 0)
        {
            Timer -=Time.deltaTime;
            gm.m_cGUI.Timer.text = "" + (int)Timer;
        }
        else
        {
            PS = PlayerState.DIE; // 죽음상태로 돌아감 
           // cam.transform.localPosition = transform.position + new Vector3(0, 2f, 3f);  // 죽고나면 카메라는 캐릭터 앞을본다. 
           // cam.transform.rotation = transform.rotation * Quaternion.Euler(35f, 180f, 0);
           //cam.transform.LookAt(transform.position);
           //cam.transform.RotateAround(transform.position, new Vector3(0, 2f, 3f), transform.rotation.z); // 뒷통수 
        }
    }
    public void Potatohit() //레이캐스트용 (감자체크) (십자가로 레이 쏘아 주변에 감자 체크한다.)
    {        
        RaycastHit hit;
        bool hitOb = false;
        Vector3[] rayVectors = new Vector3[4];
        rayVectors[0] = transform.forward;
        rayVectors[1] = transform.right;
        rayVectors[2] = -transform.right;
        rayVectors[3] = -transform.forward;        
        for (int i = 0; i < rayVectors.Length; i++)
        {
            Debug.Log(transform.localScale.y);
            Debug.DrawRay(transform.position + new Vector3(0,0.2f,0), rayVectors[i], Color.blue, 0.3f);
            if (Physics.Raycast(transform.position + new Vector3(0, 0.2f, 0f), rayVectors[i], out hit, 0.3f, 1 << LayerMask.NameToLayer("Potato")))
            {
                hitOb = true;
            }
        }
        if (!hitOb)
        {
            Debug.Log("주위에 포테이토가없다");
            PS = PlayerState.DIE;
        }
    }
}
