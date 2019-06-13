using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendereredFrontOfCamera : MonoBehaviour {

    private Agent agent;
	// Use this for initialization
	void Start ()
    {
        agent = transform.parent.parent.GetComponent<Agent>();
	}

    void OnBecameVisible()
    {
        // Render different meshes for the object depending on whether
        // the main camera or minimap camera is viewing.

        if (Camera.current.name == "Main Camera")
        {
            //Debug.Log(transform.parent.parent.name +  "siiiiiiiiiiiiiiim");
            agent.isRendering = true;
            //StartCoroutine("CheckForVisible");
        }
    }

    /*void OnBecameInvisible()
    {
        if (Camera.current.name == "Main Camera")
        {
            Debug.Log(transform.parent.parent.name + "naoooooooooooooooooo");
            agent.isRendering = false;
            StopCoroutine("CheckForVisible");
        }
    }*/

    /*IEnumerator CheckForVisible()
    {
        var mainCamera = Camera.main.transform;
        while (true)
        {
            var direction = (transform.position - mainCamera.position);
            var distance = direction.magnitude;
            if (!Physics.Raycast(mainCamera.position, direction, distance - 0.4f)) //Or some other factor
            {
                SendMessage("Seen");
                 
            }
             yield return new WaitForSeconds(0.2f);
        }
    }*/
    
    // Update is called once per frame
    void Update () {
		
	}
}
