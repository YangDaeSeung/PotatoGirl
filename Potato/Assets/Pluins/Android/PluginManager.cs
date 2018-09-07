using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginManager : MonoBehaviour {
#if UNITY_ANDROID
    public static PluginManager ins;
    public static AndroidJavaObject m_AndroidJavaObject = null; //자바 객체
    public static AndroidJavaObject m_AndroidInstance = null;
#elif UNITY_IOS
#endif
    
    private void Start()
    {
#if UNITY_ANDROID
        ins = this;
        using (AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            m_AndroidInstance = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        }
        m_AndroidJavaObject = new AndroidJavaObject("com.sbsgame.android.unityandroidplugin.Plugin"); //오브젝트가 새로 생성된 것을 받아야 플러그인 사용 가능

        Debug.LogWarning("PluginManager.AndroidJavaObject : " + m_AndroidJavaObject);
        m_AndroidJavaObject.Call("SetActivity", m_AndroidInstance);
#endif
    }
    public void ExitPopUp()
    {
        m_AndroidJavaObject.Call("PopUpExit");
    }
}
