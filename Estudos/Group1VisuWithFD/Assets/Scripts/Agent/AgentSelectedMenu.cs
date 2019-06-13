using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AgentSelectedMenu : MonoBehaviour
{
    public Agent agentSelected;
    public bool isUsing = false, isRadialMenuActivated = false;
    public GameController gameController;
    //RadialMenuEventsController radialMenu;

	// Use this for initialization
	void Start ()
    {
        EventTrigger triggerRadialMenu = transform.GetChild(0).GetComponent<EventTrigger>();
        EventTrigger.Entry entryRadialMenu = new EventTrigger.Entry();
        entryRadialMenu.eventID = EventTriggerType.PointerClick;
        entryRadialMenu.callback.AddListener((data) => { GoToRadialMenu((PointerEventData)data); });
        triggerRadialMenu.triggers.Add(entryRadialMenu);
        

        for (int i = 1; i < transform.childCount - 1; i++)
        {
            if(i != 4)
            {
                //Debug.Log(gameObject.transform.Find("Elements").GetChild(i).name);
                EventTrigger trigger = transform.GetChild(i).GetChild(0).GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((data) => { ElementText((PointerEventData)data); });
                trigger.triggers.Add(entry);

                EventTrigger trigger3 = transform.GetChild(i).GetChild(0).GetComponent<EventTrigger>();
                EventTrigger.Entry entry3 = new EventTrigger.Entry();
                entry3.eventID = EventTriggerType.PointerExit;
                entry3.callback.AddListener((data) => { ElementTextExit((PointerEventData)data); });
                trigger3.triggers.Add(entry3);


                EventTrigger trigger2 = transform.GetChild(i).GetChild(1).GetComponent<EventTrigger>();
                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry2.eventID = EventTriggerType.PointerEnter;
                entry2.callback.AddListener((data) => { ElementText((PointerEventData)data); });
                trigger2.triggers.Add(entry2);

                EventTrigger trigger4 = transform.GetChild(i).GetChild(1).GetComponent<EventTrigger>();
                EventTrigger.Entry entry4 = new EventTrigger.Entry();
                entry4.eventID = EventTriggerType.PointerExit;
                entry4.callback.AddListener((data) => { ElementTextExit((PointerEventData)data); });
                trigger4.triggers.Add(entry4);
            }
            else if(i == 4)
            {
                for(int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    EventTrigger trigger = transform.GetChild(i).GetChild(j).GetComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerEnter;
                    entry.callback.AddListener((data) => { ElementText((PointerEventData)data); });
                    trigger.triggers.Add(entry);

                    EventTrigger trigger4 = transform.GetChild(i).GetChild(j).GetComponent<EventTrigger>();
                    EventTrigger.Entry entry4 = new EventTrigger.Entry();
                    entry4.eventID = EventTriggerType.PointerExit;
                    entry4.callback.AddListener((data) => { ElementTextExit((PointerEventData)data); });
                    trigger4.triggers.Add(entry4);
                }
            }
            
        }
    }
	
    public void ElementText(PointerEventData data)
    {
        for (int i = 0; i < data.pointerEnter.transform.parent.childCount; i++)
        {
            if (data.pointerEnter.transform.parent.GetChild(i).transform.gameObject.activeSelf)
            {
                transform.GetChild(5).GetComponent<Text>().text = data.pointerEnter.transform.name;
            }
        }
        
    }

    public void ElementTextExit(PointerEventData data)
    {
        transform.GetChild(5).GetComponent<Text>().text = "";
    }

    public void GoToRadialMenu(PointerEventData data)
    {
        if (gameController.isUsingRadialMenu == false && isRadialMenuActivated == false && gameController.radialEventsController.isActivated == false)
        {
            gameController.isUsingRadialMenu = true;
            isRadialMenuActivated = true;
            gameController.radial_menu.gameObject.SetActive(true);
            gameController.radialEventsController.currentAgent = agentSelected;
            gameController.radialEventsController.isActivated = true;
            gameController.radialEventsController.agentSelected = agentSelected.menuSelected;
        }
        else if (gameController.isUsingRadialMenu == true && isRadialMenuActivated == false && gameController.radialEventsController.isActivated == true)
        {
            for (int i = 0; i < gameController.radialEventsController.transform.GetChild(2).childCount; i++)
            {
                for(int j = 1; j < gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).childCount; j++)
                {
                    if (gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(j).transform.gameObject.activeSelf)
                    {
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(j).transform.gameObject.SetActive(false);
                    }
                }
            }
            gameController.radialEventsController.currentAgent = agentSelected;
            gameController.radialEventsController.agentSelected = agentSelected.menuSelected;
            gameController.radialEventsController.isActivated = true;
            gameController.isUsingRadialMenu = true;
            isRadialMenuActivated = true;
        }
        else if(gameController.isUsingRadialMenu == true && isRadialMenuActivated == true && gameController.radialEventsController.isActivated == true)
        {
            for (int i = 0; i < gameController.radialEventsController.transform.GetChild(2).childCount; i++)
            {
                for (int j = 1; j < gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).childCount; j++)
                {
                    if (gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(j).transform.gameObject.activeSelf)
                    {
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(j).transform.gameObject.SetActive(false);
                    }
                }
            }
            gameController.radialEventsController.currentAgent = null;
            gameController.radialEventsController.agentSelected = null;
            gameController.radialEventsController.isActivated = false;
            gameController.isUsingRadialMenu = false;
            isRadialMenuActivated = false;

            gameController.radial_menu.gameObject.SetActive(false);
        }
        
    }

    public void UnselectedMenu()
    {
        if (agentSelected == null && isUsing == true)
        {
            gameController.radialEventsController.currentAgent = null;
            gameController.radialEventsController.agentSelected = null;
            gameController.radialEventsController.isActivated = false;
            gameController.isUsingRadialMenu = false;
            isRadialMenuActivated = false;
            agentSelected = null;
            isUsing = false;
            gameController.radial_menu.gameObject.SetActive(false);
            transform.GetChild(0).transform.GetComponent<Text>().text = "";
            transform.GetChild(5).transform.GetComponent<Text>().text = "";
        }


    }

    // Update is called once per frame
    void Update ()
    {
        UnselectedMenu();
	}
}
