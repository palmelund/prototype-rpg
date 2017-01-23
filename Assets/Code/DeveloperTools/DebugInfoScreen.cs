using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfoScreen : MonoBehaviour
{
    private bool _active = false;
    public GameObject MouseTracker; // Set in editor

	// Use this for initialization
	void Start () {
        MouseTracker.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F5))
		{
            MouseTracker.gameObject.SetActive(!_active);

		    _active = !_active;
		}
	}
}
