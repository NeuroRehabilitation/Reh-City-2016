using UnityEngine;
using System.Collections;

public class ObjRange : MonoBehaviour {

	void Update ()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 15);
	}
}
