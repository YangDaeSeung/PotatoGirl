using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class GridWindow3D : EditorWindow { //유니티 에디터에서 창을 띄워 설정 가능 하다.
    Grid3D grid; //3D상의 그리드를 나오게 한다.

    public void Init()
    {
        grid = (Grid3D)FindObjectOfType(typeof(Grid3D)); //해당 타입을 찾아 캐스팅하고 받아옴
    }

    private void OnGUI()
    {
        grid.color = EditorGUILayout.ColorField(grid.color, GUILayout.Width(200f)); //그리드 선의 색깔을 변경 할 수 있음
    }

}
#endif

