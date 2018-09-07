using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageBtn : MonoBehaviour {
    public Text m_cText;
    public void SetText(int i)
    {
        i += 1;
        m_cText.text = "" + i;
    }
}
