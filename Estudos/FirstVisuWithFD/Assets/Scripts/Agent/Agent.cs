using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Agent : MonoBehaviour
{
    public Renderer rend;
    public GameObject emotions, isolationsAgent, collectivitiesAgent;
    private GameObject hapAgent, sadAgent, fearAgent, angerAgent, isolationAgent, socializationAgent, collectivityAgent, notCollectivityAgent;
    private GameObject hapAgentSel, sadAgentSel, fearAgentSel, angerAgentSel, hapAngerAgentSel, hapSadAgentSel, hapFearAgentSel, sadFearAgentSel, sadAngerAgentSel, fearAngerAgentSel, hapSadFearAgentSel, hapSadAngerAgentSel, hapFearAngerAgentSel, sadFearAngerAgentSel, hapSadFearAngerAgentSel;
    public float timePass = 0.0f;
    public bool isActive = false, isUsingCamera = false, isUsingRadialMenu = false, worldspaceIsActivate = false, isSelected = false, hasMenu = false, agentActivatedOnTheScene = false;
    public float scaleSpeed, rotSpeed;
    public RMF_RadialMenu radial_menu;
    public Vector3 current_position;
    public Camera mini_cam, main_cam;
    public MainCamera maincam;
    public string id;
    public int startingFrame;
    //ending frame
    public int endingFrame;
    //starting position
    public Vector3 startingPosition;
    //ending position
    public Vector3 endingPosition;
    //movements
    [HideInInspector]
    public List<Vector3> movements, movements_perspective;
    public string hofstede_pdi, hofstede_idv, hofstede_mas, hofstede_lto, hofstede_ind;
    [HideInInspector]
    public List<string>  frames, activate_list, agent_velocities, agent_directions, dist_from_others_videos, o_agent_list, c_agent_list, e_agent_list, 
        a_agent_list, n_agent_list, areas_around, collectivities, disturbances, isolations, socializations, px_to_m_factors, emotions_anger, emotions_fear, 
        emotions_happiness, emotions_sadness;
    public GameController gameController;
    private Animator anim;
    public Transform sadness, fear, anger, happiness, agent_o, agent_c, agent_e, agent_a, agent_n, worldspace;
    private bool isHap = false, isFear = false, isSad = false, isAnger = false;
    public bool isRendering = false, mainCamActivated = false;
    private string frameString;
    public Texture m_MainTexture, m_Normal, m_Metal;
    private SkinnedMeshRenderer m_Renderer;
    public AgentSelectedMenu menuSelected;
    private string textClick;

    private Color color;

    void Start()
    {

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (gameController.toggleName.Equals("HumanoidToggle"))
        {
            m_Renderer = transform.GetChild(4).GetChild(2).GetComponent<SkinnedMeshRenderer>();
        }
           
        anim = GetComponent<Animator>();
        rend = GetComponent<Renderer>();
        //optionMenu = gameController.FindGameObjectWithTag("OptionMenu");
        //radial_menu = gameController.FindGameObjectWithTag("RadialMenu").GetComponent<GameObject>();
        mini_cam = GameObject.FindGameObjectWithTag("MiniCamera").GetComponent<Camera>();
        main_cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        maincam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCamera>();
        string agentname;
        foreach (Agent agent_new in gameController.agents)
        {
            agentname = "Agent" + agent_new.id;
            if (agentname.Equals(transform.name))
            {
                movements = agent_new.movements;
                movements_perspective = agent_new.movements_perspective;
                frames = agent_new.frames;
                activate_list = agent_new.activate_list;
                agent_velocities = agent_new.agent_velocities;
                agent_directions = agent_new.agent_directions;
                dist_from_others_videos = agent_new.dist_from_others_videos;
                o_agent_list = agent_new.o_agent_list;
                c_agent_list = agent_new.c_agent_list;
                e_agent_list = agent_new.e_agent_list;
                a_agent_list = agent_new.a_agent_list;
                n_agent_list = agent_new.n_agent_list;
                areas_around = agent_new.areas_around;
                collectivities = agent_new.collectivities;
                disturbances = agent_new.disturbances;
                isolations = agent_new.isolations;
                socializations = agent_new.socializations;
                px_to_m_factors = agent_new.px_to_m_factors;
                emotions_anger = agent_new.emotions_anger;
                emotions_fear = agent_new.emotions_fear;
                emotions_happiness = agent_new.emotions_happiness;
                emotions_sadness = agent_new.emotions_sadness;
                hofstede_pdi = agent_new.hofstede_pdi;
                hofstede_idv = agent_new.hofstede_idv;
                hofstede_mas = agent_new.hofstede_mas;
                hofstede_lto = agent_new.hofstede_lto;
                hofstede_ind = agent_new.hofstede_ind;
                id = agent_new.id;
            }
        }
        //gameObject.agents
        /*emotions = GameObject.Find("/" + transform.name + "/Emotions");
        collectivitiesAgent = GameObject.Find("/" + transform.name + "/Collectivism");
        //Debug.Log(emotions.name);
        isolationsAgent = GameObject.Find("/" + transform.name + "/SocializationVsIsolation");
        isolationAgent = isolationsAgent.transform.GetChild(1).gameObject;
        socializationAgent = isolationsAgent.transform.GetChild(2).gameObject;
        collectivityAgent = collectivitiesAgent.transform.GetChild(1).gameObject;
        notCollectivityAgent = collectivitiesAgent.transform.GetChild(2).gameObject;
        hapAgent = emotions.transform.GetChild(0).transform.GetChild(3).gameObject;
        sadAgent = emotions.transform.GetChild(0).transform.GetChild(0).gameObject;
        fearAgent = emotions.transform.GetChild(0).transform.GetChild(1).gameObject;
        angerAgent = emotions.transform.GetChild(0).transform.GetChild(2).gameObject;*/

    }

    public Agent(string id_agent, string hofstede_pdi_value, string hofstede_idv_value, string hofstede_mas_value, string hofstede_lto_value, string hofstede_ind_value)
    {
        movements = new List<Vector3>();
        movements_perspective = new List<Vector3>();
        frames = new List<string>();
        activate_list = new List<string>();
        agent_velocities = new List<string>();
        agent_directions = new List<string>();
        dist_from_others_videos = new List<string>();
        o_agent_list = new List<string>();
        c_agent_list = new List<string>();
        e_agent_list = new List<string>();
        a_agent_list = new List<string>();
        n_agent_list = new List<string>();
        areas_around = new List<string>();
        collectivities = new List<string>();
        disturbances = new List<string>();
        isolations = new List<string>();
        socializations = new List<string>();
        px_to_m_factors = new List<string>();
        emotions_anger = new List<string>();
        emotions_fear = new List<string>();
        emotions_happiness = new List<string>();
        emotions_sadness = new List<string>();
        hofstede_pdi = hofstede_pdi_value;
        hofstede_idv = hofstede_idv_value;
        hofstede_mas = hofstede_mas_value;
        hofstede_lto = hofstede_lto_value;
        hofstede_ind = hofstede_ind_value;
        id = id_agent;
    }


    private void WalkingOrRunning()
    {
        //Debug.Log(gameController.count_frame + " -- " + Time.frameCount);
        if (float.Parse(agent_velocities.ElementAt(frames.IndexOf(gameController.count_frame.ToString()))) >= 1.8f)
        {
            anim.Play("Running");
        }
        else if (float.Parse(agent_velocities.ElementAt(frames.IndexOf(gameController.count_frame.ToString()))) < 1.8f && float.Parse(agent_velocities.ElementAt(frames.IndexOf(gameController.count_frame.ToString()))) > 0)
        {
            anim.Play("Walking");
        }
        else
        {
            anim.Play("Idling");
        }
    }
    public void AnimationWalkingAndRunning()
    {
        float meanSpeed = (float.Parse(agent_velocities.ElementAt(frames.IndexOf(gameController.count_frame.ToString()))) + float.Parse(agent_velocities.ElementAt(frames.IndexOf(gameController.count_frame.ToString()) + 1)) + float.Parse(agent_velocities.ElementAt(frames.IndexOf(gameController.count_frame.ToString()) - 1))) / 3;
        if (frames.Contains(gameController.count_frame.ToString()))
        {
            anim.SetFloat("Speed", meanSpeed / 3);
        }
    }

    public void AnimationIdling()
    {
        anim.SetFloat("Speed", 0);
    }

    public void PlayAndPause()
    {
        if (gameController.isPlay == true)
        {
            if (frames.Contains((gameController.count_frame + 10).ToString()))
            {
                Vector3 relativePos2 = movements.ElementAt(frames.IndexOf((gameController.count_frame + 10).ToString())) - movements.ElementAt(frames.IndexOf(gameController.count_frame.ToString()));
                Quaternion rotationAgent2 = Quaternion.LookRotation(relativePos2, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotationAgent2, Time.deltaTime * 2.5f);

            }
            else
            {
                //int countPlusone = gameController.count_frame + 1;
                //Debug.Log((frames.Contains(gameController.count_frame.ToString()) && frames.Contains((gameController.count_frame + 1).ToString()) && movements.Contains(movements.ElementAt(frames.IndexOf(frames.Count.ToString()) + 1))));
                if (frames.Contains(gameController.count_frame.ToString()) && frames.Contains((gameController.count_frame + 1).ToString()) && movements.Contains(movements.ElementAt(frames.IndexOf(frames.Count.ToString()) + 1)) && agentActivatedOnTheScene == true)
                {
                    //Debug.Log(gameController.count_frame + "-------------" + (gameController.count_frame + 1).ToString() + "-------------" + transform.name + "-----" +movements.ElementAt(frames.IndexOf(frames.Count.ToString()) + 1));
                    Vector3 relativePos2 = movements.ElementAt(frames.IndexOf(frames.Count.ToString()) + 1) - movements.ElementAt(frames.IndexOf(gameController.count_frame.ToString()));

                    //Debug.Log(relativePos2);
                    Quaternion rotationAgent2 = Quaternion.LookRotation(relativePos2, Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotationAgent2, Time.deltaTime * 2.5f);
                }
                
            }
            if (gameController.toggleName.Equals("HumanoidToggle") || gameController.toggleName.Equals("BothToggle"))
            {
                AnimationWalkingAndRunning();
            }
        }
        else
        {
            if (gameController.toggleName.Equals("HumanoidToggle") || gameController.toggleName.Equals("BothToggle"))
            {
                AnimationIdling();
            }
            
            //Debug.Log(agentActivatedOnTheScene + "-------------" + transform.name + "-----------" + movements.ElementAt(frames.IndexOf(gameController.count_frame.ToString())));
            if(agentActivatedOnTheScene == true)
            {
                if (frames.Contains(gameController.count_frame.ToString()))
                {
                    transform.position = movements.ElementAt(frames.IndexOf(gameController.count_frame.ToString()));
                    if (gameController.toggleName.Equals("HumanoidToggle") || gameController.toggleName.Equals("BothToggle"))
                    {
                        AnimationWalkingAndRunning();
                    }
                        
                }
            }
            else
            {
                Destroy(transform.gameObject);
            }
            //transform.position = movements.ElementAt(frames.IndexOf(gameController.count_frame.ToString()));
        }
    }
    /*
    public void ActivateFeaturesRadialMenu()
    {
        if (isSelected == true && hasMenu == true)
        {
            frameString = gameController.count_frame.ToString();
            if (menuSelected.isRadialMenuActivated == true && gameController.isUsingRadialMenu == true && gameController.radialEventsController.isActivated == true && frames.Contains(gameController.count_frame.ToString()))
            {
                gameController.radialEventsController.agentName.text = gameController.radialEventsController.currentAgent.name;
                for (int i = 0; i < gameController.radialEventsController.transform.GetChild(2).childCount - 1; i++)
                {
                    //Debug.Log(gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).transform.name == "Emotion (OCC)");
                    if (gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).transform.name == "Mean Speed")
                    {
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(1).transform.GetComponent<Slider>().value = (float.Parse(agent_velocities.ElementAt(frames.IndexOf(frameString))) * 1) / 3;
                    }
                    else if (gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).transform.name == "Collectivity")
                    {
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(1).transform.GetComponent<Slider>().value = float.Parse(collectivities.ElementAt(frames.IndexOf(frameString)));
                    } else if (gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).transform.name == "Interpersonal Distance")
                    {
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(1).transform.GetComponent<Slider>().value = (float.Parse(dist_from_others_videos.ElementAt(frames.IndexOf(frameString))) * 1) / float.Parse(dist_from_others_videos.Max());
                    } else if (gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).transform.name == "Socialization vs Isolation")
                    {
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(1).transform.GetComponent<Slider>().value = float.Parse(socializations.ElementAt(frames.IndexOf(frameString)));
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(2).transform.GetComponent<Slider>().value = float.Parse(isolations.ElementAt(frames.IndexOf(frameString)));
                        
                    }else if (gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).transform.name == "OCEAN (Big5)")
                    {
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(1).transform.GetComponent<Slider>().value = float.Parse(o_agent_list.ElementAt(frames.IndexOf(frameString)));
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(2).transform.GetComponent<Slider>().value = float.Parse(c_agent_list.ElementAt(frames.IndexOf(frameString)));
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(3).transform.GetComponent<Slider>().value = float.Parse(e_agent_list.ElementAt(frames.IndexOf(frameString)));
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(4).transform.GetComponent<Slider>().value = float.Parse(a_agent_list.ElementAt(frames.IndexOf(frameString)));
                        gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).GetChild(5).transform.GetComponent<Slider>().value = float.Parse(n_agent_list.ElementAt(frames.IndexOf(frameString)));
                        
                    } else if (gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).transform.name == "Hofstede (HCD)")
                    {
                        //Debug.Log(gameController.radialEventsController.transform.GetChild(2).GetChild(4).GetChild(1).GetChild(1).GetChild(0).transform.name);
                        gameController.radialEventsController.texts[0].text = hofstede_ind.ToString();
                        gameController.radialEventsController.texts[1].text = hofstede_mas.ToString();
                        gameController.radialEventsController.texts[2].text = hofstede_idv.ToString();
                        gameController.radialEventsController.texts[3].text = hofstede_pdi.ToString();
                        gameController.radialEventsController.texts[4].text = hofstede_lto.ToString();

                    }
                    else if (gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).transform.name == "Emotion (OCC)")
                    {
                        //Debug.Log(gameController.radialEventsController.transform.GetChild(2).transform.GetChild(i).transform.name == "Emotion (OCC)");
                        Dictionary<string, int> listEmotions = new Dictionary<string, int>();
                        List<string> listEmotionsObjectsName = new List<string>();
                        listEmotions.Add("Happiness", Int32.Parse(emotions_happiness.ElementAt(frames.IndexOf(frameString))));
                        listEmotions.Add("Sadness", Int32.Parse(emotions_sadness.ElementAt(frames.IndexOf(frameString))));
                        listEmotions.Add("Anger", Int32.Parse(emotions_anger.ElementAt(frames.IndexOf(frameString))));
                        listEmotions.Add("Fear", Int32.Parse(emotions_fear.ElementAt(frames.IndexOf(frameString))));
                        listEmotionsObjectsName.Add(gameController.radialEventsController.emotionsList[1].name);
                        listEmotionsObjectsName.Add(gameController.radialEventsController.emotionsList[0].name);
                        listEmotionsObjectsName.Add(gameController.radialEventsController.emotionsList[3].name);
                        listEmotionsObjectsName.Add(gameController.radialEventsController.emotionsList[2].name);

                        List<string> listEmotionsNames = new List<string>();
                        int maxValue = listEmotions.Values.Max();
                        var guidForMaxDate = listEmotions.FirstOrDefault(x => x.Value == listEmotions.Values.Max()).Key;
                        //var emotionsNames = "";
                        for (int j = 0; j <= 3; j++)
                        {
                            if (listEmotions.Values.ElementAt(j) == maxValue)
                            {
                               if (listEmotions.Keys.ElementAt(j).Equals(gameController.radialEventsController.emotionsList.ElementAt(j).name))
                                {
                                    gameController.radialEventsController.emotionsList.ElementAt(j).SetActive(true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    */
    void Update()
    {
        PlayAndPause();
        /*ActivateSocializationIsolation();
        ActivateEmotions();
        ActivateCollectivity();
        ActivateVelocity();
        
        ActivateFeaturesRadialMenu();
        
        if (isRendering == true)
        {
            AgentEmotions();
            AgentSocialization();
            AgentCollectivity();
        }
        else if(isRendering == false)
        {
            frameString = gameController.count_frame.ToString();

            if(gameController.isEmotion == true)
            {
                if (frames.Contains(frameString))
                {
                    for (int i = 1; i < emotions.transform.GetChild(0).transform.childCount; i++)
                    {
                        if (emotions.transform.GetChild(0).transform.GetChild(i).transform.gameObject.activeSelf)
                        {
                            emotions.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }

            else if (gameController.isSocializationvsIsolation == true)
            {
                if (frames.Contains(frameString))
                {
                    for (int i = 1; i < isolationsAgent.transform.childCount; i++)
                    {
                        if (isolationsAgent.transform.GetChild(i).transform.gameObject.activeSelf)
                        {
                            isolationsAgent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else if (gameController.isCollectivity == true)
            {
                if (frames.Contains(frameString))
                {
                    for (int i = 1; i < collectivitiesAgent.transform.childCount; i++)
                    {
                        if (collectivitiesAgent.transform.GetChild(i).transform.gameObject.activeSelf)
                        {
                            collectivitiesAgent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        */
    }
    /*
    public void ActivateSocializationIsolation()
    {
        if (isSelected == true && hasMenu == true)
        {
            for (int i = 0; i < menuSelected.transform.GetChild(3).childCount; i++)
            {
                if (menuSelected.transform.GetChild(3).GetChild(i).transform.gameObject.activeSelf)
                {
                    menuSelected.transform.GetChild(3).GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            frameString = gameController.count_frame.ToString();
            if (frames.Contains(frameString))
            {

                if (float.Parse(isolations.ElementAt(frames.IndexOf(frameString))) > float.Parse(socializations.ElementAt(frames.IndexOf(frameString))))
                {
                    menuSelected.transform.GetChild(3).GetChild(1).transform.gameObject.SetActive(true);
                }
                else if (float.Parse(isolations.ElementAt(frames.IndexOf(frameString))) <= float.Parse(socializations.ElementAt(frames.IndexOf(frameString))))
                {
                    menuSelected.transform.GetChild(3).GetChild(0).transform.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ActivateCollectivity()
    {
        if (isSelected == true && hasMenu == true)
        {
            for (int i = 0; i < menuSelected.transform.GetChild(2).childCount; i++)
            {
                if (menuSelected.transform.GetChild(2).GetChild(i).transform.gameObject.activeSelf)
                {
                    menuSelected.transform.GetChild(2).GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            frameString = gameController.count_frame.ToString();
            if (frames.Contains(frameString))
            {

                if (float.Parse(collectivities.ElementAt(frames.IndexOf(frameString))) >= 0.5f)
                {
                    menuSelected.transform.GetChild(2).GetChild(0).transform.gameObject.SetActive(true);
                }
                else if (float.Parse(collectivities.ElementAt(frames.IndexOf(frameString))) < 0.5f)
                {
                    menuSelected.transform.GetChild(2).GetChild(1).transform.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ActivateVelocity()
    {
        if (isSelected == true && hasMenu == true)
        {
            for (int i = 0; i < menuSelected.transform.GetChild(1).childCount; i++)
            {
                if (menuSelected.transform.GetChild(1).GetChild(i).transform.gameObject.activeSelf)
                {
                    menuSelected.transform.GetChild(1).GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            frameString = gameController.count_frame.ToString();
            if (frames.Contains(frameString))
            {

                if (float.Parse(agent_velocities.ElementAt(frames.IndexOf(frameString))) >= 2.0f)
                {
                    menuSelected.transform.GetChild(1).GetChild(1).transform.gameObject.SetActive(true);
                }
                else if (float.Parse(collectivities.ElementAt(frames.IndexOf(frameString))) < 2.0f)
                {
                    menuSelected.transform.GetChild(1).GetChild(0).transform.gameObject.SetActive(true);
                }
            }
        }
    }

    public void AgentSocialization()
    {
        for (int i = 1; i < isolationsAgent.transform.childCount; i++)
        {
            if (isolationsAgent.transform.GetChild(i).transform.gameObject.activeSelf)
            {
                isolationsAgent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        frameString = gameController.count_frame.ToString();

        if (gameController.isSocializationvsIsolation == true)
        {
            var lookPos = maincam.transform.position - isolationAgent.transform.parent.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            if (frames.Contains(frameString))
            {
                if (float.Parse(isolations.ElementAt(frames.IndexOf(frameString))) > float.Parse(socializations.ElementAt(frames.IndexOf(frameString))))
                {
                    isolationAgent.SetActive(true);
                    isolationAgent.transform.parent.transform.rotation = rotation;
                }
                else if (float.Parse(isolations.ElementAt(frames.IndexOf(frameString))) <= float.Parse(socializations.ElementAt(frames.IndexOf(frameString))))
                {
                    socializationAgent.SetActive(true);
                    socializationAgent.transform.parent.transform.rotation = rotation;
                }
            }
        }
    }

    public void AgentCollectivity()
    {
        frameString = gameController.count_frame.ToString();

        for (int i = 1; i < collectivitiesAgent.transform.childCount; i++)
        {
            if (collectivitiesAgent.transform.GetChild(i).transform.gameObject.activeSelf)
            {
                collectivitiesAgent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (gameController.isCollectivity == true)
        {
            var lookPos = maincam.transform.position - collectivitiesAgent.transform.parent.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            //var blablabla = Quaternion.
            if (frames.Contains(frameString))
            {
                if (float.Parse(collectivities.ElementAt(frames.IndexOf(frameString))) >= 0.5f)
                {
                    collectivityAgent.SetActive(true);
                    collectivityAgent.transform.parent.transform.rotation = rotation;
                }
                else if (float.Parse(collectivities.ElementAt(frames.IndexOf(frameString))) < 0.5f)
                {
                   
                    notCollectivityAgent.SetActive(true);
                    notCollectivityAgent.transform.parent.transform.rotation = rotation;
                }
            }
        }
    }

    public void AgentEmotions()
    {
        frameString = gameController.count_frame.ToString();

        for (int i = 1; i < emotions.transform.GetChild(0).transform.childCount; i++)
        {
            if (emotions.transform.GetChild(0).transform.GetChild(i).transform.gameObject.activeSelf)
            {
                emotions.transform.GetChild(0).transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (gameController.isEmotion == true)
        {
            if (frames.Contains(frameString))
            {
                Dictionary<string, int> listEmotions = new Dictionary<string, int>();
                listEmotions.Add("hap", Int32.Parse(emotions_happiness.ElementAt(frames.IndexOf(frameString))));
                listEmotions.Add("sad", Int32.Parse(emotions_sadness.ElementAt(frames.IndexOf(frameString))));
                listEmotions.Add("anger", Int32.Parse(emotions_anger.ElementAt(frames.IndexOf(frameString))));
                listEmotions.Add("fear", Int32.Parse(emotions_fear.ElementAt(frames.IndexOf(frameString))));

                List<string> listEmotionsNames = new List<string>();
                int maxValue = listEmotions.Values.Max();
                var guidForMaxDate = listEmotions.FirstOrDefault(x => x.Value == listEmotions.Values.Max()).Key;
                var emotionsNames = "";
                for (int i = 0; i <= 3; i++)
                {
                    if (listEmotions.Values.ElementAt(i) == maxValue)
                    {
                        listEmotionsNames.Add(listEmotions.Keys.ElementAt(i));
                        emotionsNames = emotionsNames + listEmotions.Keys.ElementAt(i) + "-";
                    }
                }
                for (int i = 0; i < emotions.transform.GetChild(0).transform.childCount; i++)
                {
                    if (emotionsNames.Equals(emotions.transform.GetChild(0).GetChild(i).name))
                    {
                        var lookPos = maincam.transform.position - emotions.transform.position;
                        lookPos.y = 0;
                        var rotation = Quaternion.LookRotation(lookPos);
                        emotions.transform.GetChild(0).GetChild(i).transform.gameObject.SetActive(true);
                        emotions.transform.rotation = rotation;
                    }
                }
                
                listEmotions.Clear();
                emotionsNames = "";
                listEmotionsNames.Clear();
            }
        }
    }

    public void ActivateEmotions()
    {
        if(isSelected == true && hasMenu == true)
        {
            for (int i = 0; i < menuSelected.transform.GetChild(4).childCount; i++)
            {
                if (menuSelected.transform.GetChild(4).GetChild(i).transform.gameObject.activeSelf)
                {
                    menuSelected.transform.GetChild(4).GetChild(i).transform.gameObject.SetActive(false);
                }
            }
            frameString = gameController.count_frame.ToString();
            if (frames.Contains(frameString))
            {
                Dictionary<string, int> listEmotions = new Dictionary<string, int>();
                listEmotions.Add("hap", Int32.Parse(emotions_happiness.ElementAt(frames.IndexOf(frameString))));
                listEmotions.Add("sad", Int32.Parse(emotions_sadness.ElementAt(frames.IndexOf(frameString))));
                listEmotions.Add("anger", Int32.Parse(emotions_anger.ElementAt(frames.IndexOf(frameString))));
                listEmotions.Add("fear", Int32.Parse(emotions_fear.ElementAt(frames.IndexOf(frameString))));
                
                List<string> listEmotionsNames = new List<string>();
                int maxValue = listEmotions.Values.Max();
                var guidForMaxDate = listEmotions.FirstOrDefault(x => x.Value == listEmotions.Values.Max()).Key;
                var emotionsNames = "";
                for (int i = 0; i <= 3; i++)
                {
                    if (listEmotions.Values.ElementAt(i) == maxValue)
                    {
                        listEmotionsNames.Add(listEmotions.Keys.ElementAt(i));
                        emotionsNames = emotionsNames + listEmotions.Keys.ElementAt(i) + "-";
                    }
                }
                for(int i = 0; i < menuSelected.transform.GetChild(4).childCount; i++)
                {
                    if (emotionsNames.Equals(menuSelected.transform.GetChild(4).GetChild(i).name))
                    {
                        menuSelected.transform.GetChild(4).GetChild(i).transform.gameObject.SetActive(true);
                    }
                }
                listEmotions.Clear();
                emotionsNames = "";
                listEmotionsNames.Clear();
            }
        }
    }*/

    public Agent(int newStartingFrame, int newEndingFrame, Vector3 newStartingPosition, Vector3 newEndingPosition, List<Vector3> newMovements)
    {
        startingFrame = newStartingFrame;
        endingFrame = newEndingFrame;
        startingPosition = newStartingPosition;
        endingPosition = newEndingPosition;
        movements = newMovements;
    }
    
    public void CameraHeadPosition()
    {
        if (gameController.toggleName.Equals("HumanoidToggle") || gameController.toggleName.Equals("BothToggle"))
        {
            maincam.transform.localPosition = new Vector3(0, 1.6f, 0);
            maincam.transform.GetChild(0).transform.localPosition = new Vector3(maincam.transform.GetChild(0).transform.localPosition.x, -1.64f, maincam.transform.GetChild(0).transform.localPosition.z);
        }
        else if (gameController.toggleName.Equals("CylinderToggle"))
        {
            maincam.transform.localPosition = new Vector3(0, 1.03f, 0);
            maincam.transform.GetChild(0).transform.localPosition = new Vector3(maincam.transform.GetChild(0).transform.localPosition.x, -1.07f, maincam.transform.GetChild(0).transform.localPosition.z);
        }
    }
    public void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                maincam.GetComponent<Camera>().orthographic = false;
                maincam.transform.parent = transform;
                CameraHeadPosition();
                if (frames.Contains((gameController.count_frame + 20).ToString()))
                {
                    Vector3 relativePos = movements.ElementAt(frames.IndexOf(gameController.count_frame.ToString()) + 1) - movements.ElementAt(frames.IndexOf(gameController.count_frame.ToString()));

                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    maincam.transform.rotation = rotation;
                    transform.rotation = rotation;
                }


            }
            /*if (Input.GetMouseButtonDown(1) && isSelected == false)
            {
                for (int i = 0; i <= 3; i++)
                {
                    AgentSelectedMenu agentSelectedMenu = gameController.optionsMenu.transform.GetChild(i).GetComponent<AgentSelectedMenu>();
                    //Debug.Log(agentSelectedMenu.name + (agentSelectedMenu.isUsing == false && hasMenu == false));
                    if (agentSelectedMenu.isUsing == false && hasMenu == false)
                    {
                        color = m_Renderer.material.color;

                        m_Renderer.materials[1].color = agentSelectedMenu.transform.GetChild(0).GetComponent<Text>().color;
                        agentSelectedMenu.agentSelected = GetComponent<Agent>();
                        agentSelectedMenu.isUsing = true;
                        isSelected = true;
                        menuSelected = agentSelectedMenu;
                        hasMenu = true;
                        agentSelectedMenu.transform.GetChild(0).gameObject.SetActive(true);
                        agentSelectedMenu.transform.GetChild(0).GetComponent<Text>().text = transform.name;

                        //break;
                    }
                }
            }
            else if (Input.GetMouseButtonDown(1) && isSelected == true && hasMenu == true)
            {

                menuSelected.transform.GetChild(0).GetComponent<Text>().text = "";
                menuSelected.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                menuSelected.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
                menuSelected.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                menuSelected.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                menuSelected.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                menuSelected.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                for (int i = 0; i < menuSelected.transform.GetChild(4).childCount; i++)
                {
                    menuSelected.transform.GetChild(4).GetChild(i).transform.gameObject.SetActive(false);
                }

                menuSelected.agentSelected = null;
                menuSelected.isUsing = false;
                isSelected = false;
                menuSelected = null;
                hasMenu = false;
                m_Renderer.materials[1].color = color;
                gameController.radialEventsController.transform.gameObject.SetActive(false);
                gameController.radialEventsController.isActivated = false;
            }*/
        }
    }
    
}
