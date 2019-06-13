using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform currentObject;
    public Agent currentAgent;
    public bool wasFixedOnTheAgent = false;
    public GameController gameController;
    private Vector3 screenMousePosition;
    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    public float frameCounter = 15;

    Quaternion originalRotation;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;


    // Use this for initialization
    void Start ()
    {
        originalRotation = transform.localRotation;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (transform.parent != null)
        {
            if (gameController.isPlay == true)
            {
                wasFixedOnTheAgent = true;
                currentObject = transform.parent;
                currentAgent = currentObject.GetComponent<Agent>();

                if (currentAgent.frames.Contains((gameController.count_frame + 20).ToString()) && currentAgent.agentActivatedOnTheScene == true && currentAgent.frames.Contains((gameController.count_frame + 1).ToString()))
                {
                    Vector3 relativePos3 = currentAgent.movements.ElementAt(currentAgent.frames.IndexOf((gameController.count_frame).ToString()));
                    Vector3 relativePos2 = currentAgent.movements.ElementAt(currentAgent.frames.IndexOf((gameController.count_frame).ToString()));//Vector3 relativePos2 = movements.ElementAt(frames.IndexOf((gameController.count_frame + 10).ToString())) - movements.ElementAt(frames.IndexOf(gameController.count_frame.ToString()));

                    for (int i = 1; i <= 4; i++)
                    {
                        relativePos2 = relativePos2 + currentAgent.movements.ElementAt(currentAgent.frames.IndexOf((gameController.count_frame + i).ToString()));
                    }

                    for (int i = 0; i < gameController.agents_in_scene_list.Count; i++)
                    {
                        //Debug.Log(gameController.agents_in_scene_list.ElementAt(i).transform.name + "antes do if!!");
                        if (gameController.agents_in_scene_list.ElementAt(i).transform.name.Equals(currentAgent.transform.name))
                        {
                            //Debug.Log(gameController.agents_in_scene_list.ElementAt(i).transform.name + "depois do if!!");
                            if((i+1) < gameController.agents_in_scene_list.Count)
                            {
                                if (gameController.agents_in_scene_list.Contains(gameController.agents_in_scene_list.ElementAt(i + 1)))
                                {
                                    relativePos3 = gameController.agents_in_scene_list.ElementAt(i + 1).transform.position;
                                }
                            }
                            
                            else
                            {
                                relativePos3 = gameController.agents_in_scene_list.ElementAt(0).transform.position;
                            }
                        }
                    }
                    //Vector3 relativePos = currentAgent.movements.ElementAt(currentAgent.frames.IndexOf((gameController.count_frame + 1).ToString())) - currentAgent.movements.ElementAt(currentAgent.frames.IndexOf(gameController.count_frame.ToString()));
                    //Quaternion rotationAgent = Quaternion.LookRotation(((((relativePos2 / 5)*0.2f)+ (relativePos3) * 0.8f) /2) - currentAgent.movements.ElementAt(currentAgent.frames.IndexOf((gameController.count_frame).ToString())), Vector3.up);
                    Quaternion rotationAgent = Quaternion.LookRotation( relativePos3 - currentAgent.movements.ElementAt(currentAgent.frames.IndexOf((gameController.count_frame).ToString())), Vector3.up);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotationAgent, Mathf.SmoothStep(0.0f, 1.5f, Time.deltaTime));
                }
                else
                {
                    //Debug.Log("hhhhhhhhhhhhhhhhhhhhhhhaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                    if (currentAgent.frames.Contains(gameController.count_frame.ToString()))
                    {
                        Vector3 relativePos = currentAgent.movements.ElementAt(currentAgent.frames.IndexOf(currentAgent.frames.Count.ToString())) - currentAgent.movements.ElementAt(currentAgent.frames.IndexOf(gameController.count_frame.ToString()));
                        Quaternion rotationAgent = Quaternion.LookRotation(relativePos, Vector3.up);
                        transform.rotation = Quaternion.Lerp(transform.rotation, rotationAgent, Mathf.SmoothStep(0.0f, 1.5f, Time.deltaTime));
                    }
                }
            }

            else
            {
                screenMousePosition.x = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
                screenMousePosition.y = Camera.main.ScreenToViewportPoint(Input.mousePosition).y;
                Quaternion rotationAgent = Quaternion.LookRotation(new Vector3(screenMousePosition.x, 0, 0));
                
                
                if (screenMousePosition.x < 1 && screenMousePosition.x >= 0.87 && screenMousePosition.y <= 0.7562 && screenMousePosition.y >= 0.2052)
                {
                    //rotationAgent.w = -rotationAgent.w;
                    //rotationAgent.x = ClampAngle(rotationAgent.x, minimumX, maximumX);
                    //transform.rotation = Quaternion.Lerp(transform.rotation, rotationAgent, Time.deltaTime * 0.5f);

                    rotAverageX = 0f;
                    rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                    /*rotArrayX.Add(rotationX);
                    if (rotArrayX.Count >= frameCounter)
                    {
                        rotArrayX.RemoveAt(0);
                    }
                    for (int i = 0; i < rotArrayX.Count; i++)
                    {
                        rotAverageX += rotArrayX[i];
                    }
                    rotAverageX /= rotArrayX.Count;*/
                    rotAverageX = ClampAngle(rotationX, minimumX, maximumX);
                    Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up).normalized;
                    Quaternion lerpQuaternion = Quaternion.Lerp(transform.rotation, xQuaternion, Time.deltaTime * 0.5f);//originalRotation * xQuaternion;
                    transform.rotation = originalRotation * lerpQuaternion;

                }
                else if (screenMousePosition.x >= 0 && screenMousePosition.x <= 0.2 && screenMousePosition.y <= 0.7562 && screenMousePosition.y >= 0.2052)
                {
                    //rotationAgent.x = ClampAngle(rotationAgent.x, minimumX, maximumX);
                    //transform.rotation = Quaternion.Lerp(transform.rotation, rotationAgent, Time.deltaTime * 0.5f);

                    
                    rotAverageX = 0f;
                    rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                    /*rotArrayX.Add(rotationX);
                    if (rotArrayX.Count >= frameCounter)
                    {
                        rotArrayX.RemoveAt(0);
                    }
                    for (int i = 0; i < rotArrayX.Count; i++)
                    {
                        rotAverageX += rotArrayX[i];
                    }
                    rotAverageX /= rotArrayX.Count;*/
                    rotAverageX = ClampAngle(rotationX, minimumX, maximumX);
                    Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up).normalized;
                    Quaternion lerpQuaternion = Quaternion.Lerp(transform.rotation, xQuaternion, Time.deltaTime * 0.5f);//originalRotation * xQuaternion;
                    transform.rotation = originalRotation * lerpQuaternion;
                }
            }
        }
        else
        {
            wasFixedOnTheAgent = false;
            currentAgent = null;
            currentObject = null;
            gameController.ResetPositionCamera();
        }
	}

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}
