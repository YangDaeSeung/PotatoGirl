using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Setting : MonoBehaviour {
    public GameObject SettingMain;
    public Button JoyClick;
    public Button ButtonClick;
    public Button SettingExit;
    public GameObject KeySetting;
    
    public Button Easy;
    public Button Normal;
    public Button Hard;
    public GameObject LevelSetting;
   
    public void JClick() // 조이스틱 클릭
    {
        GameManager.getInstance().joyclick = true;
        GameManager.getInstance().Btnclick = false;

#if UNITY_ANDROID
        PluginManager.m_AndroidJavaObject.Call("ToastMessege", "조이스틱으로 설정되었습니다.");
#elif UNITY_IOS
#endif

    }
    public void BClick() // 버튼 클릭
    {
        GameManager.getInstance().joyclick = false;
        GameManager.getInstance().Btnclick = true;
#if UNITY_ANDROID
        PluginManager.m_AndroidJavaObject.Call("ToastMessege", "버튼으로 설정되었습니다.");
#elif UNITY_IOS
#endif
    }
    public void KeySettingEx()
    {
        KeySetting.gameObject.SetActive(false);
        GameManager.getInstance().m_cGUI.SettingCanvers.gameObject.SetActive(false);
    }
    public void KeyOnSet()
    {
        KeySetting.gameObject.SetActive(true);
        SettingMain.gameObject.SetActive(false);
    }
    
    public void EasyMod()
    {
        GameManager.getInstance().Level = 11f;
#if UNITY_ANDROID
        PluginManager.m_AndroidJavaObject.Call("ToastMessege", "이즈모드로 설정되었습니다.(10초)");
#elif UNITY_IOS
#endif
    }
    public void NormalMod()
    {
        GameManager.getInstance().Level = 6f;
#if UNITY_ANDROID
        PluginManager.m_AndroidJavaObject.Call("ToastMessege", "노말모드로 설정되었습니다.(5초)");
#elif UNITY_IOS
#endif
    }
    public void HardMod()
    {
        GameManager.getInstance().Level = 4f;
#if UNITY_ANDROID
        PluginManager.m_AndroidJavaObject.Call("ToastMessege", "하드모드로 설정되었습니다.(3초)");
#elif UNITY_IOS
#endif
    }
    public void LevelModExit()
    {
        LevelSetting.gameObject.SetActive(false);
        GameManager.getInstance().m_cGUI.SettingCanvers.gameObject.SetActive(false);
    }
    public void LevelModOn()
    {
        LevelSetting.gameObject.SetActive(true);
        SettingMain.gameObject.SetActive(false);
    }
    public void SettingEx()
    {
        GameManager.getInstance().m_cGUI.SettingCanvers.gameObject.SetActive(false);
    }
    public void Settingbtn()
    {
        GameManager.getInstance().m_cGUI.SettingCanvers.gameObject.SetActive(true);
        //LevelSetting.gameObject.SetActive(true);
        // KeySetting.gameObject.SetActive(true);
        SettingMain.SetActive(true);
    }
}
