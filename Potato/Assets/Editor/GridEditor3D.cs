//#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;

[CustomEditor(typeof(Grid3D))]
public class GridEditor3D : Editor {
    Grid3D grid;
    Vector3 mouseCur = Vector3.zero;
    private static int oldIndex; //static이 아니면 초기화 되서 값이 저장되지 않는다.    
    int index;
    bool selected;
    string[] names;
    int[] values;
    static int stageNumber;
    Object[] prefabsSource;
    GameObject[] preFabs;
    bool build;
    private void OnEnable()
    {
        //oldIndex = 0;
        grid = (Grid3D)target;
        prefabsSource = Resources.LoadAll("preFabs");
        preFabs = new GameObject[prefabsSource.Length];
        for (int i = 0; i < prefabsSource.Length; i++)
        {
#if DEBUG_LOG
            Debug.Log(prefabsSource[i]);
#endif
            preFabs[i] = (GameObject)prefabsSource[i];
        }
    }

    [MenuItem("Assets/Create/TileSet3D")]
    static void CreateTileSet()
    {
        TileSet3D asset = CreateInstance<TileSet3D>();
        //TileSet asset = ScriptableObject.CreateInstance<TileSet>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        //string path = AssetDatabase.GetAssetPath(Selection.activeObject);
#if DEBUG_LOG
        Debug.Log(path);
#endif
        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if(Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(path), "");
        }
        else
        {
            path += "/";
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet3D.asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        asset.hideFlags = HideFlags.DontSave; //오브젝트가 새로운 장면이 로드 될때 파괴 되지 않게한다.
        //메모리 누출을 피하려면 DestroyImmediate를 사용하여 객체를 수동으로 지워야함.
        
    }

    public override void OnInspectorGUI()
    {
        grid.editMod = GUILayout.Toggle(grid.editMod, "EditorMod");
        if (GUILayout.Button("Save"))
        {
            Save();
        }
        if (grid.editMod)
        {
            // prefabsSource = EditorGUILayout.ObjectField("", prefabsSource[0], typeof(List), false);
           
            EditorGUI.BeginChangeCheck();           
            //if (GUILayout.Button("Change Grid Color"))
            //{
            //    GridWindow3D window = (GridWindow3D)EditorWindow.GetWindow(typeof(GridWindow3D));
            //    window.Init();
            //}

            EditorGUILayout.LabelField("Grid Color");            
            grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200f));            
            //base.OnInspectorGUI();
            //grid.width = CreateSlider("Width", grid.width);
            //grid.height = CreateSlider("Height", grid.height);
            grid.width = CreateFloatField("Width", grid.width);
            grid.height = CreateFloatField("Height", grid.height);            
           
            //Tile Prefab
            Transform newTilePrefab = (Transform)EditorGUILayout.ObjectField("Tile Prefab", grid.Prefab3D, typeof(Transform), false);
            //Transform newTilePrefab = (Transform)EditorGUILayout.ObjectField("Tile Prefab", grid.tilePrefab, typeof(Transform), false);
            if (EditorGUI.EndChangeCheck())
            {
                grid.Prefab3D = newTilePrefab;
                Undo.RecordObject(target, "Grid Changed");
            }

            //Tile Map
            EditorGUI.BeginChangeCheck();
            //TileSet3D newTileSet = (TileSet3D)EditorGUILayout.ObjectField("Tile Set", grid.tileSet3D, typeof(TileSet3D), false);
            //TileSet newTileSet = (TileSet)EditorGUILayout.ObjectField("Tile Set", grid.tileSet, typeof(TileSet), false);
            if (EditorGUI.EndChangeCheck())
            {
                //grid.tileSet3D = newTileSet;
                Undo.RecordObject(target, "Grid Chaneged");
            }
                       
            //Select Tile
            if /*(grid.tileSet3D != null)*/(prefabsSource !=null) //그리드의 타일셋이 있다면
            {
                EditorGUI.BeginChangeCheck(); //GUI.changed의 현재 값을 스택 푸쉬하여 이전의 누적값과 현재값으로 설정한다 (변경사항을 시작/종료중에 계속 감지함)

                //names = new string[grid.tileSet3D.preFabs.Length];
                names = new string[prefabsSource.Length];
                values = new int[names.Length];
            
                for (int i = 0; i < names.Length; i++)
                {
                    //names[i] = grid.tileSet3D.preFabs[i] != null ? grid.tileSet3D.preFabs[i].name : "Error";
                    names[i] = prefabsSource[i] != null ? prefabsSource[i].name : "Error";
                    values[i] = i;
                    
#if DEBUG_LOG
                    //Debug.Log(names[i]);
                    //Debug.Log(values[i]);
#endif
                }
                //string[] names = new string[grid.tileSet3D.preFabs.Length];
                //int[] values = new int[names.Length];
                //oldIndex = index;
#if DEBUG_LOG
                //Debug.Log("변화전:" + oldIndex);
                //Debug.Log("in"+index);        
#endif
                //                              라벨 ,         이전값,프리팹이름그룹, 프리팹인덱스 그룹
                index = EditorGUILayout.IntPopup("Select Tile", oldIndex, names, values);
                if (EditorGUI.EndChangeCheck()) //변화가 끝나면
                {
#if DEBUG_LOG
                    //Debug.Log("이전값:" + oldIndex);
                    //Debug.Log("바꿀값" + index);
#endif
                    Undo.RecordObject(target, "Grid Changed");
                    if (oldIndex != index)
                    {
                        oldIndex = index;
#if DEBUG_LOG
                        //Debug.Log(oldIndex);
#endif
                        
                        //grid.Prefab3D = grid.tileSet3D.preFabs[index];
                        grid.Prefab3D = preFabs[index].transform;
                        if(grid.Prefab3D.GetComponent<Renderer>() != null)
                        {
                            float width = grid.Prefab3D.GetComponent<Renderer>().bounds.size.x;
                            float height = grid.Prefab3D.GetComponent<Renderer>().bounds.size.z;
                            if (width == 0 || height == 0) //그리드 값이 0이면 디폴트로 다른 프리팹을 받아 처리해줌
                            {
                                grid.width =  preFabs[1].GetComponent<Renderer>().bounds.size.x;
                                grid.height = preFabs[1].GetComponent<Renderer>().bounds.size.z;
                            }
                            else
                            {
                                grid.width = width;
                                grid.height = height;
                            }
                        }
                       
                        //if(width == 0 || height == 0) //그리드 값이 0이면 디폴트로 다른 프리팹을 받아 처리해줌
                        //{
                        //    grid.width =  grid.tileSet3D.preFabs[1].GetComponent<Renderer>().bounds.size.x;
                        //    grid.height = grid.tileSet3D.preFabs[1].GetComponent<Renderer>().bounds.size.z;
                        //}
                    }
                }
                else
                {
                    GameObject g = (GameObject)prefabsSource[oldIndex];
                    //.Prefab3D = grid.tileSet3D.preFabs[oldIndex];
                    grid.Prefab3D = g.transform;
                    if(grid.Prefab3D.GetComponent<Renderer>() != null)
                    {
                        float width = grid.Prefab3D.GetComponent<Renderer>().bounds.size.x;
                        float height = grid.Prefab3D.GetComponent<Renderer>().bounds.size.z;
                        if (width == 0 || height == 0) //그리드 값이 0이면 디폴트로 다른 프리팹을 받아 처리해줌
                        {
                            grid.width = preFabs[1].GetComponent<Renderer>().bounds.size.x;
                            grid.height = preFabs[1].GetComponent<Renderer>().bounds.size.z;
                        }
                        else
                        {
                            grid.width = width;
                            grid.height = height;
                        }
                    }

                }                
            }
            GUILayout.Label("StageNumber");
            stageNumber = EditorGUILayout.IntField(stageNumber);            
            if (GUILayout.Button("Load"))
            {
                Load();
            }
            //맵 초기화
            if(GUILayout.Button("Clear Map"))
            {
                ClearMap();
            }
        }
        
    }

    private float CreateSlider(string labelName , float sliderPosition)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid  " + labelName);
        grid.width = EditorGUILayout.Slider(sliderPosition, 1f, 100f, null);
        GUILayout.EndHorizontal();

        return sliderPosition;
    }
    private float CreateFloatField(string labelName, float floatField)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid  " + labelName);
        grid.width = EditorGUILayout.FloatField(floatField);
        GUILayout.EndHorizontal();

        return floatField;
    }

    private void OnSceneGUI() //씬에 GUI
    {
        if (grid.editMod)
        {
            int controllID = GUIUtility.GetControlID(FocusType.Passive);
            Event prefabsSource = Event.current;

            //Ray ray = Camera.current.ViewportPointToRay(new Vector3(prefabsSource.mousePosition.x, 0f,-prefabsSource.mousePosition.y + Camera.current.pixelHeight));
            //Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(prefabsSource.mousePosition);
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector3 mousePos = ray.origin;            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,Mathf.Infinity))
            {
                //Debug.Log("hit hit : " +hit.transform.name);
                mousePos = hit.point;                
            }            
            if (prefabsSource.isMouse && prefabsSource.type == EventType.MouseDown && prefabsSource.button == 0)
            {
                build = true;
                GUIUtility.hotControl = controllID;
                prefabsSource.Use();

                GameObject g;
                Transform prefab = grid.Prefab3D;

                if (prefab)
                {
                    Undo.IncrementCurrentGroup();
                    Vector3 aligned = new Vector3((Mathf.Floor(mousePos.x / grid.width) * grid.width) + grid.width / 2f, 0f, (Mathf.Floor(mousePos.z / grid.height) * grid.height) + grid.height / 2f);
                    //Debug.Log("생성 위치: "+aligned);
                    if (GetTransformFromPosition(aligned) != null) //클릭된 부분에 무언가 있다면
                    {
                        //Debug.Log("You Bilud Already");//이미 지었으니 아무일도 일어나지 않게
                        return;
                    }
                    g = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);
                    //원점(0,0)을 기준으로 현재 마우스가 클릭 된 지점과 그리드 사이를 계산하여 중간 값을 찾음 
                    //Debug.Log("마우스 포지션X : "+mousePos.x+", 그리드값 : "+grid.width);
                    //Debug.Log("메스계산전 : " + mousePos.x/grid.width);
                    //Debug.Log("X첫계산값 : " + Mathf.Floor(mousePos.x / grid.width)); //-0.1~-1.9는 -1을 반환 , 0.1~0.9는 0을 반환 (그 다음 소수점은 다 버림)                
                    //Debug.Log("두번째계산값 : " + grid.width + grid.width / 2f);
                    //Debug.Log("최종계산값 : " + Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2f);

                    //Debug.Log("마우스 포지션Y : " + mousePos.y + ", 그리드값 : " + grid.height);
                    //Debug.Log("메스계산전 : " + mousePos.y / grid.height);
                    //Debug.Log("Y첫계산값 : " + Mathf.Floor(mousePos.y / grid.height)); //-0.1~-0.9는 -1을 반환 , 0.1~0.9는 0을 반환 (그 다음 소수점은 다 버림)                
                    //Debug.Log("두번째계산값 : " + grid.height + grid.height / 2f);
                    //Debug.Log("최종계산값 : " + Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2f);
                    if(g.GetComponent<Renderer>() != null)
                    {
                        g.transform.position = aligned + new Vector3(0,g.transform.GetComponent<Renderer>().bounds.size.y/2f,0f);
                    }
                    g.transform.parent = grid.transform;

                    Undo.RegisterCreatedObjectUndo(g, "Create " + g.name);
                }
            }           
            if(prefabsSource.isMouse & prefabsSource.type == EventType.MouseDown && prefabsSource.button == 1) //오른쪽 마우스 회전후 원치 않는 삭제를 막아줌
            {
                //Debug.Log(GUIUtility.hotControl);
                mouseCur = mousePos;
            }
            if (prefabsSource.isMouse & prefabsSource.type == EventType.MouseUp && prefabsSource.button == 1)
            {
                //Debug.Log("Del");
                if (mousePos == mouseCur)
                {
                    GUIUtility.hotControl = controllID;
                    prefabsSource.Use();
                    Vector3 aligned = new Vector3((Mathf.Floor(mousePos.x / grid.width) * grid.width) + grid.width / 2f,
                                                 0f, (Mathf.Floor(mousePos.z / grid.height) * grid.height) + grid.height / 2f);

                    //Debug.Log("지울 위치: " + aligned);
                    Transform tr = GetTransformFromPosition(aligned);
                    if (tr != null)
                    {
                        DestroyImmediate(tr.gameObject); //메모리 누수 방지 
                    }
                    GUIUtility.hotControl = 0; //마우스 커서 상태 초기화
                    mouseCur = Vector3.zero;
                }
            }                    
            //if (prefabsSource.isMouse && prefabsSource.type == EventType.MouseUp)
            //{
            //}
        }        
    }
    Transform GetTransformFromPosition(Vector3 _Aligned)
    {
        int i = 0;
#if DEBUG_LOG
        //Debug.Log("생성수 : " +grid.transform.childCount);
#endif
        while (i < grid.transform.childCount)
        {
            Transform tr = grid.transform.GetChild(i);
#if DEBUG_LOG
            //Debug.Log("d? :"+grid.transform.GetChild(i).transform);
            //Debug.Log("아들위치 :"+tr.position);
            //Debug.Log("매개변수 :"+ _Aligned);     
#endif

            if (tr.GetComponent<Renderer>() != null &&tr.position - new Vector3(0,tr.transform.GetComponent<Renderer>().bounds.size.y/2f,0) == _Aligned)
            {
#if DEBUG_LOG
                //Debug.Log("있음");
#endif
                return tr;
            }
            i++;
        }
    return null;
    }
    void Save() //해당 메소드를 변경시키면 로드,게임매니저의 로드 값도 변경 해주어야함
    {
#if DEBUG_LOG
        //Debug.Log("Save!!!");
#endif       
       
        MapContainer map = new MapContainer();
        map.tileDatas = new List<TileData>();
        for (int i = 0; i < grid.transform.childCount; i++)
        {
            grid.transform.GetChild(i).GetComponent<MapData>().tileData.name = grid.transform.GetChild(i).name;
            grid.transform.GetChild(i).GetComponent<MapData>().tileData.pos = grid.transform.GetChild(i).transform.position;
            grid.transform.GetChild(i).GetComponent<MapData>().tileData.rot = grid.transform.GetChild(i).transform.localRotation;
            grid.transform.GetChild(i).GetComponent<MapData>().tileData.rink = grid.transform.GetChild(i).GetComponent<MapData>().rink;           
            
            TileData ta = new TileData();            
            ta.rink = grid.transform.GetChild(i).GetComponent<MapData>().tileData.rink;            
            ta.name = grid.transform.GetChild(i).GetComponent<MapData>().tileData.name;
            ta.pos = grid.transform.GetChild(i).GetComponent<MapData>().tileData.pos;
            ta.rot = grid.transform.GetChild(i).GetComponent<MapData>().tileData.rot;
            map.tileDatas.Add(ta);                            
        }
        string saveJson = JsonUtility.ToJson(map);
#if DEBUG_LOG
        //Debug.Log(stageNumber);
#endif
        File.WriteAllText(Application.dataPath + "/Resources/SaveFile/MapData/" + stageNumber + ".json", saveJson);
    }
    void Load() //맵파일 로드
    {   
        if(grid.transform.childCount > 0)
        {
#if DEBUG_LOG
            Debug.Log("Your Already Loaded");
#endif
            return;
        }        
        string json = File.ReadAllText(Application.dataPath + "/Resources/SaveFile/MapData/" + stageNumber + ".json");     //맵파일들 String으로 나열   
        MapContainer loadMap = JsonUtility.FromJson<MapContainer>(json); //나열된 값을 실제 데이터로
        for (int i = 0; i < loadMap.tileDatas.Count; i++)
        {
            //for (int j = 0; j < grid.tileSet3D.preFabs.Length; j++)
            //{
            //    if(loadMap.tileDatas[i].name == grid.tileSet3D.preFabs[j].name)
            //    {
            //        GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(grid.tileSet3D.preFabs[j].gameObject);
            //        g.transform.position = loadMap.tileDatas[i].pos;
            //        g.transform.parent = grid.transform;
            //        //Debug.Log(g.name);
            //    }
            //}
            for (int j = 0; j < prefabsSource.Length; j++) //리소스의 프리팹중에서
            {
                if(loadMap.tileDatas[i].name == prefabsSource[j].name) //데이터와 프리팹의 이름이 같은 오브젝트를 찾음
                {
                    GameObject g = (GameObject)prefabsSource[j];
                    g = (GameObject)PrefabUtility.InstantiatePrefab(g); //찾은 오브젝트를 생성
                    g.transform.position = loadMap.tileDatas[i].pos; //데이터의 정보를 받아와 지정해줌
                    g.transform.localRotation = loadMap.tileDatas[i].rot;
                    g.transform.parent = grid.transform;
                    g.GetComponent<MapData>().rink = loadMap.tileDatas[i].rink;
#if DEBUG_LOG
                    //Debug.Log(g.name);
#endif
                }
            }
        }        
    }
    void ClearMap()
    {
        int count = grid.transform.childCount;
        if (count <= 0)
        {
#if DEBUG_LOG
            //Debug.Log("There is nothing to erase");
#endif
            return;
        }
        else
        {
#if DEBUG_LOG
            //Debug.Log("You Clean " + count + " Objects");
#endif
            for (int i = 0; i < count; i++)
            {

#if DEBUG_LOG
                //Debug.Log("Count"+grid.transform.childCount);
                //Debug.Log("Index:"+i);
#endif
                DestroyImmediate(grid.transform.GetChild(0).gameObject);
            }
        }
    }
}
#endif
