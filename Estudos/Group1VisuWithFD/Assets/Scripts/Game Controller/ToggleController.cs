using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour {

    private Toggle m_Toggle;
    public GameController gameController;
    //public string toggleName = "";

    void Start()
    {
        //Fetch the Toggle GameObject
        m_Toggle = GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
    }

    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        //m_Text.text = "New Value : " + m_Toggle.isOn;
        //Debug.Log(m_Text);
        if(m_Toggle.isOn == true && transform.name == "HumanoidToggle")
        {
            gameController.bothToggle.isOn = false;
            gameController.cylinderToggle.isOn = false;
            gameController.toggleName = "HumanoidToggle";
            gameController.sceneTransition.toggleDecision = "";
            gameController.sceneTransition.toggleDecision = "HumanoidToggle";


        }
        else if (m_Toggle.isOn == true && transform.name == "CylinderToggle")
        {
            gameController.bothToggle.isOn = false;
            gameController.humanoidToggle.isOn = false;
            gameController.toggleName = "CylinderToggle";
            gameController.sceneTransition.toggleDecision = "";
            gameController.sceneTransition.toggleDecision = "CylinderToggle";
        }
        else if (m_Toggle.isOn == true && transform.name == "BothToggle")
        {
            gameController.cylinderToggle.isOn = false;
            gameController.humanoidToggle.isOn = false;
            gameController.toggleName = "BothToggle";
            gameController.sceneTransition.toggleDecision = "";
            gameController.sceneTransition.toggleDecision = "BothToggle";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_Toggle == true)
        {
            m_Toggle.isOn = false;
        }
        else
        {
            m_Toggle.isOn = true;
        }
    }
}
