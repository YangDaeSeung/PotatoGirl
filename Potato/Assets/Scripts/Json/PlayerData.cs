using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerData {
    public bool[] stage = new bool[0]; //스테이지 클리어 정보
    public int lastStage = 1; //초기화 값
}
