using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour {

    public TileData tileData = new TileData();
    public string name = string.Empty;
    public int rink;
    
    private void OnEnable()
    {        
        SaveMapData.Load += LoadData;
        SaveMapData.Save += SaveData;
        SaveMapData.Save += AddData;        
    }
    private void OnDisable()
    {
        SaveMapData.Load -= LoadData;
        SaveMapData.Save -= SaveData;
        SaveMapData.Save -= AddData;
    }
   
    public void SaveData()
    {
        tileData.pos = transform.position;
        tileData.rot = transform.localRotation;
        tileData.rink = rink;
        tileData.name = name;
        AddData();
    }
    public void LoadData()
    {
        name = tileData.name;
        transform.position = tileData.pos;
        transform.localRotation = tileData.rot;
        rink = tileData.rink;
    }
    public void AddData()
    {
        SaveMapData.AddData(tileData);
    }  
}

[Serializable]
public class TileData
{
    public string name;
    public Vector3 pos;
    public Quaternion rot;
    public int rink;
}
