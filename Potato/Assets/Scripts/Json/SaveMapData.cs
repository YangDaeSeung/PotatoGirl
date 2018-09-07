//#define DEBUG_LOG
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

//Json Data에 관련한 클래스는 MonoBehaviour를 포함한 어느것도 상속하면 안된다. 오로지 데이터만 저장하는 클래스이기 때문

public class SaveMapData : MonoBehaviour {
    public static SaveMapData ins = new SaveMapData();
    public static MapContainer saveMap = new MapContainer();
    //public static MapContainer loadMap = new MapContainer();
    public delegate void Serialize();
    public static event Serialize Save;
    public static event Serialize Load;
    
    public static void SavingData()
    {   
#if DEBUG_LOG
        Debug.Log("Save");
#endif
        string stageNum = GameManager.getInstance().m_cGUI.stageNumber.text; // 스테이지를 스트링으로 받는다.
        //string stageNum = i.ToString();
        if (stageNum == string.Empty)
        {
#if DEBUG_LOG
            Debug.Log("Error Input StageNumber");
#endif
            return;
        }
        Save();
        string saveJson = JsonUtility.ToJson(saveMap);
        Debug.Log(stageNum);
        File.WriteAllText(Application.dataPath + "/Resources/SaveFile/MapData/" + stageNum + ".json", saveJson);
    }        
   
    public static void LoadingData(int stage)
    {
#if DEBUG_LOG
        Debug.Log("Load");
#endif
        //string path = string.Format("{0}/Resources/SaveFile/{1}.json", Application.dataPath, 1);
        //string json = File.ReadAllText(path);    
        string stageNum = GameManager.getInstance().m_cGUI.stageNumber.text;
        //string json = File.ReadAllText(Application.dataPath + "/Resources/SaveFile/" + stage + ".json");
        string json = Resources.Load("SaveFile/MapData/" + stage).ToString();
            //Application.dataPath + "/Resources/SaveFile/" + stage + ".json");
        MapContainer loadMap = JsonUtility.FromJson<MapContainer>(json);
        GameManager.getInstance().LoadMap(loadMap);
   
        //삽질
        //string jsonPath = System.IO.Path.Combine(Application.dataPath, "/Resources/SaveFile/" + 1 + ".json"); //문자열 합치기 (Application.dataPath는 Combine에서 ""이다 쓰지않는게)
        //MapContainer loadMap = JsonUtility.FromJson<MapContainer>(jsonPath);
        //foreach(TileData data in loadMap.tileDatas)
        //{
        //    GameObject prefab = Resources.Load<GameObject>(jsonPath);
        //    GameObject ins = Instantiate(prefab,data.pos,Quaternion.identity);
        //}
        //Load();
        //string path = System.IO.Path.Combine(Application.dataPath, "C:/Users/aaaaaaaaaaaaaaaaaaaa/Downloads/Potato/Assets/Resources/SaveFile/" + 1 + ".json");
        //Debug.Log(Application.dataPath);
        //Debug.Log(Application.persistentDataPath);
        //string path = File.ReadAllText(Application.dataPath + "/Resources/SaveFile/" + 1 + ".json");
        //Debug.Log("Path :  "+ path);
        //Debug.Log(json);

        //loadMap = JsonUtility.FromJson<MapContainer>(json);
        //for (int i = 0; i < ; i++)
        //{
        //    GameObject load = Resources.Load<GameObject>("/Resources/Prefabs" + m.name);
        //    GameObject ins = Instantiate(load, m[i].pos, Quaternion.identity);
        //}
    }    
    public static void AddData(TileData _Data)    
    {        
#if DEBUG_LOG
        Debug.Log("Add Map");
#endif
        saveMap.tileDatas.Add(_Data);
    }
}

