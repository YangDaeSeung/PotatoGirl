using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//pngtree.com 배포할때 사용 출처 남기기
public class ClearUI : MonoBehaviour {
    public GameObject UI;
    public GameObject Next;
    public GameObject Re;
    public GameObject Exit;
    public GameObject Menu;
    public GameObject ClearScore;
    public GameObject StarImage1;
    public GameObject StarImage2;
    public GameObject StarImage3;

   public int NextStage;
   public void MenuClick()
    {
        GameManager.getInstance().ClearMap();
        GameManager.getInstance().m_cPlayer.PS = PlayerState.IDLE;
        UI.SetActive(false);
        GameManager.getInstance().m_cGUI.ScoreText.gameObject.SetActive(false);
        GameManager.getInstance().m_cGUI.JoyStic.gameObject.SetActive(false);
        GameManager.getInstance().m_cGUI.GoButton.gameObject.SetActive(false);
        GameManager.getInstance().m_cGUI.StageUI.SetActive(true);
        GameManager.getInstance().m_cGUI.BG.gameObject.SetActive(true);
        GameManager.getInstance().m_cGUI.newJoystick.SetActive(false);
        GameManager.getInstance().m_cGUI.GoButton.SetActive(false);
        GameManager.getInstance().m_cGUI.startbtn.gameObject.SetActive(false);
        GameManager.getInstance().StageClearCheck();
    }
    public void NextClick()
    {
        //for (int i = 0; i < GameManager.getInstance().mapFiles.Length; i++)
        //{
        //    NextStage = 1;
        //    if (i == GameManager.getInstance().iStage)
        //    {
        //        NextStage = GameManager.getInstance().curStage;
        //        NextStage++;
        //        string json = Resources.Load("SaveFile/MapData/" + NextStage).ToString();
        //        MapContainer loadMap = JsonUtility.FromJson<MapContainer>(json);
        //        GameManager.getInstance().ClearMap();
        //        GameManager.getInstance().LoadMap(loadMap);
        //        GameManager.getInstance().m_cGUI.ScoreText.text = "SCORE :" + 0;
        //        GameManager.getInstance().iStage += 1;
        //        UI.SetActive(false);
        //        break;
        //    }
        //}
        GameManager.getInstance().ClearMap();
        GameManager.getInstance().m_cGUI.Load(GameManager.getInstance().curStage + 1);
        GameManager.getInstance().m_cGUI.ScoreText.text = "SCORE :" + 0;
        UI.SetActive(false);
    }
    public void ReGame()
    {

        string json = Resources.Load("SaveFile/MapData/" + GameManager.getInstance().iStage).ToString();
        MapContainer loadMap = JsonUtility.FromJson<MapContainer>(json);
        GameManager.getInstance().ClearMap();
        GameManager.getInstance().LoadMap(loadMap);
        GameManager.getInstance().m_cGUI.ScoreText.text = "SCORE :" + 0;
        UI.SetActive(false);
    }
    public void ExitClick()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}
