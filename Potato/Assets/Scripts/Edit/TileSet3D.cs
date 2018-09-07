#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
                            //게임 기초 정보 저장에 쓰이면 좋은 방식
public class TileSet3D : ScriptableObject //단독으로 게임 데이터를 저장 할 수 있는 클래스 (실제 데이터를 카피해서 가지는 것 이 아닌, 참조만 하여 데이터가 정확하고 가볍다.)
{
    public Transform[] preFabs = new Transform[0]; //프리팹 사이즈 초기화
}
#endif
