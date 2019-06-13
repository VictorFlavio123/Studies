using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour {

    //public List<Agent> agents = new List<Agent>();
    public Agent agent;
    public GameController game_controller;
    public GameObject gameObject;

    public AgentController(Agent agent_)
    {
        agent = agent_;
    }

    void Start ()
    {
        
    }

    public void Movement()
    {
        Vector3 newPosition = new Vector3(transform.position.x, 0, transform.position.z);

        transform.LookAt(newPosition);

        transform.position = newPosition;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
