using UnityEngine;
using System.Collections;

public class plusone : MonoBehaviour {
    private float elapsedTime = 0;
    private TextMesh textmesh;
    private Color white;
	void Start () {
        white = Color.white;
        textmesh = GetComponent<TextMesh>();
        Destroy(gameObject, 2.0f);
	}
	void Update () {
        elapsedTime += Time.deltaTime;
        white.a = Mathf.PingPong(elapsedTime, 1);
        textmesh.color = white;  
	}
}