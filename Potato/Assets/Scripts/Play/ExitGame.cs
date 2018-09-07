using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {
  
    public void ExitButtonClick()
    {
        PluginManager.ins.ExitPopUp();
    }
    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKey(KeyCode.Escape))
        {
            PluginManager.ins.ExitPopUp();
        }            
#endif
    }
    //private void OnGUI()
    //{
    //    nNavitiveData = GetInt();

    //    if (m_AndroidJavaObject != null)
    //    {
    //        GUI.Box(new Rect(0, 0, 200, 20), "GetInt : " + nNavitiveData);
    //        if (GUI.Button(new Rect(200, 100, 300, 200), "ToastMessage"))
    //        {
    //            m_AndroidJavaObject.Call("ToastMessege", "토스트메세지");
    //        }
    //        if (GUI.Button(new Rect(200, 300, 300, 200), "PopUp"))
    //        {
    //            m_AndroidJavaObject.Call("PopUpExit");
    //        }
    //    }
    //    else
    //    {
    //        GUI.Box(new Rect(0, 0, 200, 20), "Plugin Load Failed");
    //    }
    //}
    //int GetInt()
    //{
    //    int nResult = -1;

    //    nResult = m_AndroidJavaObject.Call<int>("GetInt");

    //    return nResult;
    //}
}