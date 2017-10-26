using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// This file should not be used, but is just a demo to show modifying prefabs at runtime.

public class Tester : MonoBehaviour {
    private void Start()
    {
        var pref = Instantiate(Resources.Load<GameObject>("Dummy"));

        pref.transform.Translate(1,0,0);
        
        PrefabUtility.ReplacePrefab(pref, Resources.Load<GameObject>("Dummy"));
    }
}
