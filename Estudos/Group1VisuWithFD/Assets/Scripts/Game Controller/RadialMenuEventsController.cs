using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using TMPro;
using EventTrigger = UnityEngine.EventSystems.EventTrigger;
using System;
using System.Linq;

public class RadialMenuEventsController : MonoBehaviour
{
    public GameController gameController;
    private EventTrigger button_mean_speed, button_collectivity, button_interpersonal_distance, button_social_iso, button_hofstede, button_ocean, button_emotion;
    private Transform elements_object;
    public AgentSelectedMenu agentSelected;
    public bool isActivated = false;
    public Agent currentAgent;
    public TMP_Text agentName;
    public List<TMP_Text> texts = new List<TMP_Text>();
    public List<GameObject> emotionsList = new List<GameObject>();
    public List<GameObject> features = new List<GameObject>();
   
    public void PressButtons(PointerEventData data)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            
            foreach (GameObject emotion in emotionsList)
            {
                if (emotion.transform.gameObject.activeSelf)
                {
                    emotion.transform.gameObject.SetActive(false);
                }
            }
            foreach (GameObject feature in features)
            {
                for (int j = 1; j < feature.transform.childCount; j++)
                {

                    if (feature.transform.GetChild(j).transform.gameObject.activeSelf)
                    {
                        feature.transform.GetChild(j).transform.gameObject.SetActive(false);
                    }
                }
            }

            foreach (GameObject feature in features)
            {
                if (feature.transform.name == data.pointerEnter.transform.parent.parent.name && feature.transform.name != features[6].transform.name)
                {
                    for (int j = 1; j < feature.transform.childCount; j++)
                    {
                        feature.transform.GetChild(j).transform.gameObject.SetActive(true);
                    }
                }
                else if(feature.transform.name == data.pointerEnter.transform.parent.parent.name && feature.transform.name == features[6].transform.name)
                {
                    
                    string frameString = gameController.count_frame.ToString();
                    Dictionary<string, int> listEmotions = new Dictionary<string, int>();
                    listEmotions.Add("Happiness", Int32.Parse(currentAgent.emotions_happiness.ElementAt(currentAgent.frames.IndexOf(frameString))));
                    listEmotions.Add("Sadness", Int32.Parse(currentAgent.emotions_sadness.ElementAt(currentAgent.frames.IndexOf(frameString))));
                    listEmotions.Add("Anger", Int32.Parse(currentAgent.emotions_anger.ElementAt(currentAgent.frames.IndexOf(frameString))));
                    listEmotions.Add("Fear", Int32.Parse(currentAgent.emotions_fear.ElementAt(currentAgent.frames.IndexOf(frameString))));
                    
                    List<string> listEmotionsNames = new List<string>();
                    int maxValue = listEmotions.Values.Max();
                    var guidForMaxDate = listEmotions.FirstOrDefault(x => x.Value == listEmotions.Values.Max()).Key;
                    //var emotionsNames = "";
                    for (int j = 0; j <= 3; j++)
                    {
                        Debug.Log("foraaaaaaaaaaaaaaaa");
                        if (listEmotions.Values.ElementAt(j) == maxValue)
                        {
                            emotionsList.ElementAt(j).SetActive(true);
                           
                        }
                    }
                }
            }
        }
      
    }
    

    // Use this for initialization
    void Start ()
    {
       
        elements_object = transform.Find("Elements");

        for(int i = 0; i < transform.Find("Elements").childCount; i++)
        {
            //Debug.Log(gameObject.transform.Find("Elements").GetChild(i).name);
            EventTrigger trigger = gameObject.transform.Find("Elements").GetChild(i).GetChild(0).GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { PressButtons((PointerEventData)data); });
            trigger.triggers.Add(entry);
        }
        
    }
}
