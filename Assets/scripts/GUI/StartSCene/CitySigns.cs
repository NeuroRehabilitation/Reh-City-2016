using UnityEngine;
using System.Collections;
using Assets.scripts.Controller;

public class CitySigns : MonoBehaviour {

    public static bool ShowSigns;
    public GameObject CitySignsGroup;

	private void Update ()
    {
        
        if(Application.loadedLevelName == "City")
        {
            CitySignsGroup.SetActive(ShowSigns);

            if (Controller.B2())
            {
                ShowSigns = !ShowSigns;
            }
        }

	    
    }
}
