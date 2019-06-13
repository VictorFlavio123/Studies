using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarOptionsController : MonoBehaviour
{
    public GameObject optionMenu;
    private Animator anim;
    public bool isActivated = false;
    public GameController gameController;
    public Toggle emotion, isolation, collectivity;

    // Use this for initialization
    void Start ()
    {
        transform.SetAsLastSibling();
        //Debug.Log(transform.GetSiblingIndex() + " baroption");

        Time.timeScale = 1;
        //get the animator component
        anim = optionMenu.GetComponent<Animator>();
        //disable it on start to stop it from playing the default animation
        anim.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(isActivated == false && Camera.main.ScreenToViewportPoint(Input.mousePosition).x <= 1 && Camera.main.ScreenToViewportPoint(Input.mousePosition).x >= 0 && Camera.main.ScreenToViewportPoint(Input.mousePosition).y <= 0.2 && Camera.main.ScreenToViewportPoint(Input.mousePosition).y >= 0)
        {
            isActivated = true;
           // Debug.Log("Mouse is over GameObject.");
            //enable the animator component
            anim.enabled = true;
            //play the Slidein animation
            anim.Play("MenuTransition");
            Time.timeScale = 0;
            
            if (gameController.isEmotion == true && emotion.isOn == true)
            {
                isolation.isOn = false;
                collectivity.isOn = false;
            }
            else if (gameController.isSocializationvsIsolation == true && isolation.isOn == true)
            {
                emotion.isOn = false;
                collectivity.isOn = false;
            }
            else if (gameController.isCollectivity == true && collectivity.isOn == true)
            {
                emotion.isOn = false;
                isolation.isOn = false;
            }

        }
        else if(isActivated == true && Camera.main.ScreenToViewportPoint(Input.mousePosition).x >= 1 || Camera.main.ScreenToViewportPoint(Input.mousePosition).x <= 0 || Camera.main.ScreenToViewportPoint(Input.mousePosition).y >= 0.2 || Camera.main.ScreenToViewportPoint(Input.mousePosition).y <= 0)
        {
            isActivated = false;
            //transform.GetChild(0).transform.gameObject.SetActive(false);
            //transform.GetChild(1).transform.gameObject.SetActive(false);
            //Debug.Log("Mouse is no longer on GameObject.");
            anim.Play("MenuTransition2");
            //set back the time scale to normal time scale
            Time.timeScale = 1;
            
        }
        
    }

    public void CheckPointerEnter(PointerEventData eventData)
    {
        isActivated = true;
       // Debug.Log("Mouse is over GameObject.");
        //enable the animator component
        anim.enabled = true;
        //play the Slidein animation
        anim.Play("MenuTransition");
        Time.timeScale = 0;
    }

    public void CheckPointerExit(PointerEventData eventData)
    {
        isActivated = false;
        transform.GetChild(0).transform.gameObject.SetActive(false);
        transform.GetChild(1).transform.gameObject.SetActive(false);
        Debug.Log("Mouse is no longer on GameObject.");
        anim.Play("MenuTransition2");
        //set back the time scale to normal time scale
        Time.timeScale = 1;
    }
}
