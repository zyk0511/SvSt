using UnityEngine;
using System.Collections;

public class EventManger : MonoBehaviour {

    CEventDispacher cEventDispacher;

	// Use this for initialization
	void Start () {
        cEventDispacher = CEventDispacher.GetInstance();
	}

    void OnDestroy()
    {
        cEventDispacher.RemoveAll();
    }
}
