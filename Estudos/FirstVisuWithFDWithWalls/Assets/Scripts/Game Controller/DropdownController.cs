using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour {

    //Create a List of new Dropdown options
    // TextAsset videoTextAsset2 = Resources.Load < TextAsset >
    public GameController gameController;
    //List<string> m_DropOptions = new List<string> { "Option 1", "Option 2" };
    List<string> m_DropOptions;
    //This is the Dropdown
    Dropdown m_Dropdown;

    void Start()
    {
        //TextAsset videoTextAsset2 = Resources.Load<TextAsset>("NewDataSet/DATA_BR-01");
        var videoTextAsset = Resources.LoadAll("NewDataSet", typeof(TextAsset));
        m_DropOptions = new List<string> { };
        foreach (var t in videoTextAsset)
        {
            m_DropOptions.Add(t.name);
        }
        //Fetch the Dropdown GameObject the script is attached to
        m_Dropdown = GetComponent<Dropdown>();
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });
        //Clear the old options of the Dropdown menu
        m_Dropdown.ClearOptions();
        //Add the options created in the List above
        //m_Dropdown.AddOptions(m_DropOptions);
        m_Dropdown.AddOptions(m_DropOptions);
    }

    void DropdownValueChanged(Dropdown change)
    {
        gameController.textVideo = "" + change.value;
    }
}
