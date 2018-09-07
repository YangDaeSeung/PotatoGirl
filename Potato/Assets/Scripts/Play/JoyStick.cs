//#define DEBUG_LOG

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class JoyStick : MonoBehaviour{
    
    public Transform stick; // 조이스틱
    public Transform Player; // 플레이어 
    public int PlayerSpd = 2; // 플레이어 로테이션 속도
    Vector3 StickFirstPos; // 조이스틱의 처음 위치
    Vector3 JoyVec; // 조이스틱의 벡터(방향)
    float JoyRadius; // 조이스틱 배경의 반지름.
    bool move;
    Vector3 localForward;



    private void Start()
    {
#if DEBUG_LOG
        Debug.Log(localForward + "처음값");
#endif
        JoyRadius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFirstPos = stick.transform.position;        
        float Canvers = transform.parent.GetComponent<RectTransform>().localScale.x;

        JoyRadius *= Canvers;      
    }

    public void Drag(BaseEventData _Data)
    {
#if DEBUG_LOG
        print("드레그 중");
#endif
       // Move.ins.rota = true;
       if(GameManager.getInstance().m_cPlayer != null && GameManager.getInstance().m_cPlayer.PS != PlayerState.MOVE)
        {            
            PointerEventData data = _Data as PointerEventData;        
            GameManager.getInstance().m_cPlayer.b_JoyStic = true;
            Vector3 Pos = data.position;

            //조이스틱을 이동시킬 방향을 구함.
            JoyVec = (Pos - StickFirstPos).normalized;

            //조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
            float Dis = Vector3.Distance(Pos, StickFirstPos);


            if(Dis < JoyRadius) // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는곳으로 이동
            {
                stick.position = StickFirstPos + JoyVec * Dis;
            }
            else // 거리의 반지름보다 커지면 조이스틱을 반지름의 크기 만큼 이동
            {
                stick.position = StickFirstPos + JoyVec * JoyRadius;
            }
            //Player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
            Player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0)+localForward * PlayerSpd;
#if DEBUG_LOG
            Debug.Log("Local : " + localForward);
#endif
            //Debug.Log(JoyVec);
            //Debug.Log((Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg));
            //float ad = (Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg);
            //Player.rotation = Quaternion.LookRotation(Player.forward, new Vector3(0, 0, ad));
            //Quaternion au = Quaternion.AngleAxis(ad, Player.up);
            //Player.rotation = Quaternion.Lerp(curRotation,au,1f);
        }
    }
    public void DragEnd()
    {        
        if (GameManager.getInstance().m_cPlayer != null)
        {
            localForward = Player.eulerAngles;                    
            GameManager.getInstance().m_cPlayer.b_JoyStic = false;
            //  Move.ins.rota = false;
            stick.position = StickFirstPos; // 스틱을 원래 위치로 이동
            JoyVec = Vector3.zero; // 방향을 0값으로      
        }
#if DEBUG_LOG
        Debug.Log(localForward + "나중값");        
        print("드레그 끝");
#endif
    }

  


    public void LeftRotation()
    {
        if (GameManager.getInstance().m_cPlayer != null)
        {
            if (GameManager.getInstance().m_cGUI.pauseon == false)
            {
                GameManager.getInstance().m_cPlayer.b_JoyStic = true;
                if (GameManager.getInstance().m_cPlayer != null && GameManager.getInstance().m_cPlayer.PS != PlayerState.MOVE)
                {
                    Player.transform.Rotate(0, 90, 0);
                }
            }
        }
    }
    public void RightRotation()
    {    
        if(GameManager.getInstance().m_cPlayer != null)
        {
            if (GameManager.getInstance().m_cGUI.pauseon == false)
            {
                GameManager.getInstance().m_cPlayer.b_JoyStic = true;
                if (GameManager.getInstance().m_cPlayer != null && GameManager.getInstance().m_cPlayer.PS != PlayerState.MOVE)
                {
                    Player.transform.Rotate(0, -90, 0);
                }
            }
        }
    }
}
