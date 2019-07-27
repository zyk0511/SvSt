using UnityEngine;
using System.Collections;

public class EventManger : MonoBehaviour {

    CEventDispacher cEventDispacher;

	// Use this for initialization
	void Start () {
        cEventDispacher = CEventDispacher.GetInstance();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        cEventDispacher.RemoveAll();
    }
}
