using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    // enabled = false;
    //private const string video_path = @"C:\Users\victo\Desktop\Interactions - Victor\Crowd Dataset\Códigos e Datasets\DataSet novo\DATA_BR-03.txt";
    //public string allLines;
    //private List<List<string>> videovalues = new List<List<string>>();
    //public Transform prefab;
    [HideInInspector]
    public Agent current_agent;
    public bool isMeanSpeed = false, isCollectivity = false, isInterpersonalDistance = false, isSocializationvsIsolation = false, isHofstede = false, isOCEAN = false, isEmotion = false;
    public bool isUsingRadialMenu = false, isCulturalD = false;
    private bool isUsingWorldSpaceAgent = false;
    public int radial_menu_stage;
    public RMF_RadialMenu radial_menu, radial_menu_emotions, radial_menu_ocean;
    public RadialMenuEventsController radialEventsController;
    public Slider slider;
    public Text sliderValue;
    public string textVideoList = "", textVideo = "";

    private string video_path;
    private string[] frames_videos;
    [HideInInspector]
    public List<Agent> agents = new List<Agent>(), all_agents = new List<Agent>();
    public List<GameObject> agents_in_scene_list = new List<GameObject>();
    private Agent agent;
    [HideInInspector]
    public List<string> agents_in_scene = new List<string>(), agents_names = new List<string>();
    
    public GameObject agent_prefab, optionsMenu, cylinder_prefab, selected_prefab, both_avatar_selected;
    public Camera mini_camera;
    public MainCamera main_camera;
    public Button m_LoadSceneButton, play_button;
    private Button btn1;
    private bool wasClicked = false;
    public bool isPlay = false, isPressing = false;
    public PlayPauseButtonClick ppbuttonclick;
    private int test;
    private float test2 = 1;
    private GameObject newAgent;
    private Vector3 position;
    [HideInInspector]
    public float max_slider_value = 0, min_slider_value = 0, max = 0;
    [HideInInspector]
    public float max_x = 0, max_y = 0, count = 0;
    [HideInInspector]
    public int count_frame = 1;
    [HideInInspector]
    public List<string> countries_videos = new List<string>(), videos_names = new List<string>(), videos_frames = new List<string>(),
        agents_id_videos = new List<string>(), pos_x_videos = new List<string>(), pos_y_videos = new List<string>(),
        pos_x_perspective = new List<string>(), pos_y_perspective = new List<string>(), activated_agents_videos = new List<string>(),
        agents_velocities_videos = new List<string>(), agents_directions_videos = new List<string>(),
        dist_from_others_videos = new List<string>(), o_agents_videos = new List<string>(), c_agents_videos = new List<string>(),
        e_agents_videos = new List<string>(), a_agents_videos = new List<string>(), n_agents_videos = new List<string>(), agents_id = new List<string>(),
        areas_around_videos = new List<string>(), collectivities_videos = new List<string>(), disturbances_videos = new List<string>(),
        isolations_videos = new List<string>(), socializations_videos = new List<string>(), px_to_m_factors_videos = new List<string>(),
        emotions_anger_videos = new List<string>(), emotions_fear_videos = new List<string>(), emotions_happiness_videos = new List<string>(),
        emotions_sadness_videos = new List<string>(), hofstede_pdi_videos = new List<string>(), hofstede_idv_videos = new List<string>(),
        hofstede_mas_videos = new List<string>(), hofstede_lto_videos = new List<string>(), hofstede_ind_videos = new List<string>();
    [HideInInspector]
    public string sceneName = "";
    public string m_MyFirstScene, m_MySecondScene;
    public Scene m_Scene;
    public Button backButton, resetPosCam;
    //public DropdownController dropdownController;
    //Create a List of new Dropdown options
    // TextAsset videoTextAsset2 = Resources.Load < TextAsset >
    //public GameController gameController;
    //List<string> m_DropOptions = new List<string> { "Option 1", "Option 2" };
    List<string> m_DropOptions;
    //This is the Dropdown
    public Dropdown m_Dropdown;
    public SceneTransitionController sceneTransition;
    public string toggleName = "";
    public GameObject fd;
    public GameObject loadingObject;
    public Toggle humanoidToggle, cylinderToggle, bothToggle;
    public ToggleController toggleController;
    private Vector3 camInitialPosition;
    private Quaternion camInitialRotation;

    void Awake()
    {
        //DontDestroyOnLoad(this);
        //m_MyButton.onClick.AddListener(SceneTransition);
        m_Scene = SceneManager.GetActiveScene();
        if (m_Scene.name == "Scene2")
        {
            humanoidToggle.isOn = true;
            toggleName = "HumanoidToggle";
            //m_MyButton.GetComponentInChildren<Text>().text = "Load Next Scene";
        }

        //Otherwise change the Text to "Load Previous Scene"
        else
        {
            GameObject sceneTransitionObject = GameObject.Find("SceneTransition");
            SceneTransitionController sceneTransition = sceneTransitionObject.GetComponent<SceneTransitionController>();
            // m_MyButton.GetComponentInChildren<Text>().text = "Load Previous Scene";
            play_button.onClick.AddListener(PlaySimulation);
            backButton.onClick.AddListener(BackToFirstScene);
            resetPosCam.onClick.AddListener(ResetPositionCamera);
            toggleName = sceneTransition.toggleDecision;
            Destroy(sceneTransition.gameObject);
            PrefabSelected();
            //use video selected in first scene
            textVideoList = "NewDataSet/" + sceneTransition.videoName;
            sceneTransition.toggleDecision = "";
            string nameTest = "";
            for (int i = 0; i < sceneTransition.videoName.Length; i++)
            {
                if (i < 2)
                {
                    nameTest += sceneTransition.videoName[i].ToString();
                    //Debug.Log(nameTest);
                }
            }
            //Debug.Log(nameTest + "----------------------teste");
            sceneTransition.videoName = "";
            TextAsset videoTextAsset2 = Resources.Load<TextAsset>(textVideoList);

            //Debug.Log(videoTextAsset2.ToString());
            video_path = videoTextAsset2.ToString();
            //DATA_CN-01
            //string[] lines = System.IO.File.ReadAllLines(video_path);
            string[] lines = videoTextAsset2.text.Split('\n');
            frames_videos = new string[lines.Length];

            for (int i = 1; i < lines.Length; i++)
            {
                 //Debug.Log(lines[i]);
                if (lines[i] != null && lines[i] != "")
                {
                    string[] entries = lines[i].Split(';');
                    List<string> test = new List<string>();
                    countries_videos.Add(entries[0]);
                    videos_names.Add(entries[1]);
                    //Debug.Log(videos_frames[i].ToString());
                    videos_frames.Add(entries[2]);
                    agents_id_videos.Add(entries[3]);
                    pos_x_videos.Add(entries[4]);
                    pos_y_videos.Add(entries[5]);
                    pos_x_perspective.Add(entries[6]);
                    pos_y_perspective.Add(entries[7]);
                    activated_agents_videos.Add(entries[8]);
                    agents_velocities_videos.Add(entries[9]);
                    agents_directions_videos.Add(entries[10]);
                    dist_from_others_videos.Add(entries[11]);
                    areas_around_videos.Add(entries[12]);
                    collectivities_videos.Add(entries[13]);
                    disturbances_videos.Add(entries[14]);
                    isolations_videos.Add(entries[15]);
                    socializations_videos.Add(entries[16]);
                    px_to_m_factors_videos.Add(entries[17]);
                    emotions_anger_videos.Add(entries[18]);
                    emotions_fear_videos.Add(entries[19]);
                    emotions_happiness_videos.Add(entries[20]);
                    emotions_sadness_videos.Add(entries[21]);
                    hofstede_pdi_videos.Add(entries[22]);
                    hofstede_idv_videos.Add(entries[23]);
                    hofstede_mas_videos.Add(entries[24]);
                    hofstede_lto_videos.Add(entries[25]);
                    hofstede_ind_videos.Add(entries[26]);
                    o_agents_videos.Add(entries[27]);
                    c_agents_videos.Add(entries[28]);
                    e_agents_videos.Add(entries[29]);
                    a_agents_videos.Add(entries[30]);
                    n_agents_videos.Add(entries[31]);

                }
            }

            for (int i = 0; i < agents_id_videos.Count; i++)
            {
                //Debug.Log(!agents_id.Contains(agents_id_videos.ElementAt(i)));
                if (!agents_id.Contains(agents_id_videos.ElementAt(i)))
                {
                    agents_id.Add(agents_id_videos.ElementAt(i));
                }

            }


            foreach (string posx in pos_x_videos)
            {
                if (float.Parse(posx) / float.Parse(px_to_m_factors_videos.ElementAt(0)) > max_x)
                {
                    max_x = float.Parse(posx) / float.Parse(px_to_m_factors_videos.ElementAt(0));
                }
            }

            foreach (string posy in pos_y_videos)
            {
                if (float.Parse(posy) / float.Parse(px_to_m_factors_videos.ElementAt(0)) > max_y)
                {
                    max_y = float.Parse(posy) / float.Parse(px_to_m_factors_videos.ElementAt(0));
                }
            }
            foreach (string maxframe in videos_frames)
            {
                if (int.Parse(maxframe) > max)
                {
                    max = int.Parse(maxframe);
                }
            }

            foreach (string agent_new in agents_id)
            {
                agent = new Agent(agent_new, hofstede_pdi_videos.ElementAt(0), hofstede_idv_videos.ElementAt(0), hofstede_mas_videos.ElementAt(0), hofstede_lto_videos.ElementAt(0), hofstede_ind_videos.ElementAt(0));

                for (int i = 0; i < agents_id_videos.Count; i++)
                {
                    //Debug.Log(!agents_id.Contains(agents_id_videos.ElementAt(i)));
                    if (agent_new.Equals(agents_id_videos.ElementAt(i)) && activated_agents_videos.ElementAt(i).Equals("yes"))
                    {
                        agent.activate_list.Add(activated_agents_videos.ElementAt(i));
                        agent.agent_directions.Add(agents_directions_videos.ElementAt(i));
                        agent.agent_velocities.Add(agents_velocities_videos.ElementAt(i));
                        agent.a_agent_list.Add(a_agents_videos.ElementAt(i));
                        agent.c_agent_list.Add(c_agents_videos.ElementAt(i));
                        agent.dist_from_others_videos.Add(dist_from_others_videos.ElementAt(i));
                        agent.e_agent_list.Add(e_agents_videos.ElementAt(i));
                        agent.frames.Add(videos_frames.ElementAt(i));
                        agent.n_agent_list.Add(n_agents_videos.ElementAt(i));
                        agent.o_agent_list.Add(o_agents_videos.ElementAt(i));
                        agent.movements.Add(new Vector3(float.Parse(pos_x_videos.ElementAt(i)) / float.Parse(px_to_m_factors_videos.ElementAt(0)), 0, max_y - float.Parse(pos_y_videos.ElementAt(i)) / float.Parse(px_to_m_factors_videos.ElementAt(0))));
                        agent.movements_perspective.Add(new Vector3(float.Parse(pos_x_perspective.ElementAt(i)) / float.Parse(px_to_m_factors_videos.ElementAt(0)), 0, float.Parse(pos_y_perspective.ElementAt(i)) / float.Parse(px_to_m_factors_videos.ElementAt(0))));
                        agent.areas_around.Add(areas_around_videos.ElementAt(i));
                        agent.collectivities.Add(collectivities_videos.ElementAt(i));
                        agent.disturbances.Add(disturbances_videos.ElementAt(i));
                        agent.isolations.Add(isolations_videos.ElementAt(i));
                        agent.socializations.Add(socializations_videos.ElementAt(i));
                        agent.px_to_m_factors.Add(px_to_m_factors_videos.ElementAt(i));
                        agent.emotions_anger.Add(emotions_anger_videos.ElementAt(i));
                        agent.emotions_fear.Add(emotions_fear_videos.ElementAt(i));
                        agent.emotions_happiness.Add(emotions_happiness_videos.ElementAt(i));
                        agent.emotions_sadness.Add(emotions_sadness_videos.ElementAt(i));
                    }
                }
                agents.Add(agent);
            }


            //Vector3 maxmin_scale = new Vector3(max_x / 10, 0, max_y / 10);
            //fd.transform.GetChild(0).transform.localScale = maxmin_scale;
            

            mini_camera.transform.position = new Vector3((max_x / 2) + 0.6f, mini_camera.transform.position.y, (max_y / 2));
            fd.transform.position = new Vector3((max_x / 2)+ 0.6f, fd.transform.position.y, (max_y / 2) - 0.1f);

            //Debug.Log("siiiiim" + nameTest + "  "  +nameTest.Equals("GE"));
            if (nameTest.Equals("GE"))
            {
                fd.transform.transform.GetChild(0).localScale = new Vector3(1.2f, 1, 1.2f);
                fd.transform.transform.GetChild(1).localScale = new Vector3(1.15f, 1, 1.12f);
            }
            
            //fd.transform.GetChild(0).transform.localScale = new Vector3(1.9f ,1,1.13f);

            /*GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.localScale = maxmin_scale;
            mini_camera.transform.position = new Vector3(max_x / 2, mini_camera.transform.position.y, max_y / 2);
            plane.transform.position = new Vector3(max_x / 2, plane.transform.position.y, max_y / 2);
            var teste = Resources.Load("Google_Earth_Snapshot");
            Material material = Material.Instantiate(Resources.Load<Material>("Material"));
            plane.transform.GetComponent<Renderer>().material = material;*/
        }



    }

    public void PrefabSelected()
    {
        if (toggleName.Equals("HumanoidToggle"))
        {
            selected_prefab = agent_prefab;
        }else if (toggleName.Equals("CylinderToggle"))
        {
            selected_prefab = cylinder_prefab;
        }else if (toggleName.Equals("BothToggle"))
        {
            selected_prefab = both_avatar_selected;
        }
    }

    void SceneTransition()
    {
        //Check if the current Active Scene's name is your first Scene
        if (m_Scene.name == "Scene2")
        {
            //Load your second Scene
            //SceneManager.LoadScene(m_MySecondScene);
        }

        //Check if the current Active Scene's name is your second Scene's name
        if (m_Scene.name == "Scene1")
        {
            //Load your first Scene
            //SceneManager.LoadScene(m_MyFirstScene);
        }
    }

    public void ResetPositionCamera()
    {
        main_camera.transform.parent = null;
        main_camera.transform.localPosition = camInitialPosition;
        main_camera.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    void DropdownValueChanged(Dropdown change)
    {
        textVideo = "" + change.value;
        sceneTransition.toggleDecision = toggleName;
        sceneTransition.videoName = "" + m_Dropdown.options[change.value].text;
        loadingObject.transform.gameObject.SetActive(true);
        SceneManager.LoadScene("Scene1");

    }
    void Start()
    {
        if (m_Scene.name == "Scene2")
        {
            //TextAsset videoTextAsset2 = Resources.Load<TextAsset>("NewDataSet/DATA_BR-01");
            var videoTextAsset = Resources.LoadAll("NewDataSet", typeof(TextAsset));
            m_DropOptions = new List<string> { };
            m_DropOptions.Add("Select Video");
            foreach (var t in videoTextAsset)
            {
                m_DropOptions.Add(t.name);
            }

            //Fetch the Dropdown GameObject the script is attached to
            m_Dropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(m_Dropdown);
            });
            //Clear the old options of the Dropdown menu
            m_Dropdown.ClearOptions();
            //Add the options created in the List above
            m_Dropdown.AddOptions(m_DropOptions);
        }

        else
        {
            
            camInitialPosition = main_camera.transform.localPosition;
            //camInitialRotation = main_camera.transform.rotation;
            slider.minValue = ((1 * 100) / max) / 100;
            slider.maxValue = 1;
            //m_MyButton.GetComponentInChildren<Text>().text = "Load Previous Scene";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Return the current Active Scene in order to get the current Scene's name
        m_Scene = SceneManager.GetActiveScene();

        //Check if the current Active Scene's name is your first Scene
        if (m_Scene.name == "Scene2")
        {
            //m_MyButton.GetComponentInChildren<Text>().text = "Load Next Scene";
        }


        else
        {
            //m_MyButton.GetComponentInChildren<Text>().text = "Load Previous Scene";
            //sliderValue.text = (videos_frames.Contains(count_frame.ToString()) && isPlay).ToString() + " - c.va " + count_frame.ToString() + "- s.va " + slider.value.ToString() + "- t.va " + test2;
            if (videos_frames.Contains(count_frame.ToString()) && isPlay)
            {
                count = ((count_frame * 100) / max) / 100;
                //count_frame = (int)test2;
                slider.value = count;

                for (int j = 1; j < videos_frames.Count(); j++)
                {
                    if (count_frame == int.Parse(videos_frames.ElementAt(j)))
                    {
                        foreach (Agent agent_new in agents)
                        {
                            if (agents_id_videos.ElementAt(j).Equals(agent_new.id))
                            {

                                string agentname = "Agent" + agents_id_videos.ElementAt(j);
                                if (!agents_in_scene.Contains(agents_id_videos.ElementAt(j)) && activated_agents_videos.ElementAt(j).Equals("yes") && !agents_names.Contains(agentname))
                                {
                                    agents_in_scene.Add(agents_id_videos.ElementAt(j));
                                    position = new Vector3(float.Parse(pos_x_videos.ElementAt(j)) / float.Parse(px_to_m_factors_videos.ElementAt(0)), 0, max_y - float.Parse(pos_y_videos.ElementAt(j)) / float.Parse(px_to_m_factors_videos.ElementAt(0)));
                                    newAgent = Instantiate(selected_prefab, position, Quaternion.identity);
                                    newAgent.name = agentname;
                                    agents_names.Add(newAgent.name);
                                    newAgent.transform.position = position;
                                    agents_in_scene_list.Add(newAgent);
                                    current_agent = GameObject.Find(newAgent.name).GetComponent<Agent>();
                                    current_agent.agentActivatedOnTheScene = true;
                                    //Debug.Log("Agent ------- " + current_agent.name  + " Anger value ----- " + emotions_anger_videos.ElementAt(count_frame));
                                }

                                if (agents_in_scene.Contains(agents_id_videos.ElementAt(j)) && activated_agents_videos.ElementAt(j).Equals("yes") && agents_names.Contains(agentname))
                                {
                                    position = new Vector3(float.Parse(pos_x_videos.ElementAt(j)) / float.Parse(px_to_m_factors_videos.ElementAt(0)), 0, max_y - float.Parse(pos_y_videos.ElementAt(j)) / float.Parse(px_to_m_factors_videos.ElementAt(0)));
                                    newAgent = GameObject.Find(agentname);
                                    current_agent = GameObject.Find(newAgent.name).GetComponent<Agent>();
                                    newAgent.transform.position = position;
                                }

                                if (agents_in_scene.Contains(agents_id_videos.ElementAt(j)) && !activated_agents_videos.ElementAt(j).Equals("yes") && agents_names.Contains(agentname))
                                {
                                    agents_in_scene.Remove(agents_id_videos.ElementAt(j));
                                    newAgent = GameObject.Find(agentname);
                                    agents_names.Remove(newAgent.name);
                                    agents_in_scene_list.Remove(newAgent);
                                    if (main_camera.wasFixedOnTheAgent == true && main_camera.currentAgent.name == agentname)
                                    {
                                        main_camera.transform.parent = null;
                                    }
                                    //main_camera.transform.parent = null;
                                    //current_agent = GameObject.Find(newAgent.name).GetComponent<Agent>();
                                    //current_agent.mainCamActivated = false;
                                    current_agent = GameObject.Find(newAgent.name).GetComponent<Agent>();
                                    current_agent.agentActivatedOnTheScene = false;
                                    current_agent = null;
                                    Destroy(newAgent);
                                    newAgent = null;
                                    
                                }
                            }
                        }
                    }
                }
                count_frame++;
            }
            else if (!isPlay)
            {
                test2 = max * slider.value;

                count_frame = Mathf.RoundToInt(test2);

            }
        }

        
    }

    


    public void PlaySimulation()
    {
        if(!isPlay)
        {
            isPlay = true;
            //sliderValue.text = "true";
            //Debug.Log(isPlay);

        }
        else
        {
            isPlay = false;
            //sliderValue.text = "false";
            //Debug.Log(isPlay);
        }
    }

    public void BackToFirstScene()
    {
        SceneManager.LoadScene("Scene2");
    }

 
    
}

        