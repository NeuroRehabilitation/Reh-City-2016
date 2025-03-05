using UnityEngine;
using System.Collections;

public class WrongItem : MonoBehaviour {

    private GameObject left;
    private GameObject right;
    /// <summary>
    /// set from GameManager is the Item selected is wrong one
    /// </summary>
    public bool SelectedWrong = true;

    private float elapsedTime=0;
    public Color RedColor;
	// Use this for initialization
	void Start () {
        left = transform.FindChild("Left").gameObject;
        right = transform.FindChild("Right").gameObject;
        Destroy(this.gameObject, 2.0f);
    }
	// Update is called once per Frame
	void Update () {
        if (SelectedWrong)
        {
            elapsedTime += Time.deltaTime;
           // RedColor.a = Mathf.Abs(Mathf.Sin(Mathf.PingPong(elapsedTime,1)));
            RedColor.a = Mathf.PingPong(elapsedTime, 1);
            left.renderer.material.color = RedColor;
            right.renderer.material.color = RedColor;
            /* if (elapsedTime >= 2.0f)
            {
                RedColor.a = 0;
                left.renderer.material.color = RedColor;
                right.renderer.material.color = RedColor;
                SelectedWrong = false;
                elapsedTime = 0;
            }*/
        }
	}
}
