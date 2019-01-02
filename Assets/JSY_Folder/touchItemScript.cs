using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchItemScript : MonoBehaviour {

	// targetObj
	public GameObject targetObj;
	bool is_active_now;


	void Start () {

		if (targetObj != null) {
			targetObj.SetActive (false);
			is_active_now = targetObj.activeSelf;
		}


	}

	void targetObjACtive(){
		if (is_active_now == false) {
			targetObj.SetActive (true);
			is_active_now = true;

		} else {
			targetObj.SetActive (false);
			is_active_now = false;
		}

	}

	void OnMouseDown(){

		targetObjACtive ();
	}

	void OnMouseUp(){

	}

	// Update is called once per frame
	void Update () {

	}
}
