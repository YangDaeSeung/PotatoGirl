using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class front : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	bool frontOn;
    // Update is called once per frame
    void Update () {
		if(frontOn)
        {
            Front();
        }
	}
    public void Front()
    {
        //Player.transform.Translate(Vector3.forward * PlayerSpd * Time.deltaTime);

        //if (Move.ins.potato != null || Move.ins.box != null)
        //{
        //    Move.ins.move = true;
        //}       
        ;
        if (GameManager.getInstance().m_cPlayer != null && GameManager.getInstance().m_cPlayer.PS == PlayerState.IDLE)
        {

            if (GameManager.getInstance().m_cPlayer.target != null)
            {
#if DEBUG_LOG
            Debug.Log("감자가있다");
#endif
                Vector3 Dis = new Vector3(GameManager.getInstance().m_cPlayer.target.transform.position.x, 0, GameManager.getInstance().m_cPlayer.target.transform.position.z) -
                              new Vector3(GameManager.getInstance().m_cPlayer.transform.position.x, 0, GameManager.getInstance().m_cPlayer.transform.position.z);
                GameManager.getInstance().m_cPlayer.transform.rotation = Quaternion.LookRotation(Dis);
                GameManager.getInstance().m_cPlayer.PS = PlayerState.MOVE;
            }


        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.getInstance().m_cGUI.pauseon == false)
        {
            frontOn = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (GameManager.getInstance().m_cGUI.pauseon == false)
        {
            frontOn = false;
        }
    }
}
