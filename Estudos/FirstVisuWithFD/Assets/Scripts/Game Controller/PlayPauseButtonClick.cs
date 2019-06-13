using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayPauseButtonClick : MonoBehaviour {

    public string ButtonTest(Button btn)
    {
        //Debug.Log(btn.name.ToString());
        return btn.name.ToString();
    }
}
