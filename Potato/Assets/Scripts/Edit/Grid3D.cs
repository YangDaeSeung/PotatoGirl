using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR

public class Grid3D: MonoBehaviour {
    public GameObject[] prefab;
    public bool editMod; //에디트 모드

    public float width = 32f; //너비 (가로)
    public float height = 32f; //높이(세로)    
    public Color color = Color.green; //색깔 디폴드값으로 초기화

    public Transform Prefab3D; //선택한 프리팹

    //public TileSet3D tileSet3D; //프리팹 리스트    

    private void OnDrawGizmos() //하이라키에 기즈모 그리기
    {
        if (editMod) //에디트 모드 활성화 
        {
            Vector3 pos = Camera.current.transform.position; //현재 카메라의 포지션
            Gizmos.color = color; //그리드 색 지정
            if(height != 0 && width != 0)
            {
                for (float y = pos.y - 800f; y < pos.y + 800f; y += height) // ex)높이 =3이면 0에 라인, 3에 라인, 6에 라인
                { //가로 라인 그리기
                    Gizmos.DrawLine(new Vector3(-1000000f, 0f, Mathf.Floor(y / height) * height), new Vector3(1000000f, 0f, Mathf.Floor(y / height) * height));
                    //시작포지션 ->Z를 높이에 맞게 X1~X2까지 선을 긋는다.     
                }
                for (float x = pos.x - 1200f; x < pos.x + 1200; x += width)
                {//세로 라인 그리기
                    Gizmos.DrawLine(new Vector3(Mathf.Floor(x / width) * width, 0f, -1000000f), new Vector3(Mathf.Floor(x / width) * height, 0f, 1000000f));
                }
            }
            else
            {
                Debug.Log("Error!!!! Your MeshRendere is Zero");
            }
        }
    }
}
#endif
