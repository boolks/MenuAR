using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;
using System;
using System.Text.RegularExpressions;

public class touchScript : MonoBehaviour {

    // Information about user
    public DatabaseReference reference;
    public FirebaseAuth managerAuth;
    public FirebaseUser loginUser;
    public string trname;

	// targetObj
	public GameObject targetObj;
	bool is_active_now;

	// button
	private Renderer rend;
	private bool button_active_flag;
	private GameObject[] buttonList;

	void Start () {
        // Set Database URL
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://menuardb-7330a.firebaseio.com/");

        // Get the root reference location of the database and get current user id
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        managerAuth = FirebaseManager.auth;
        loginUser = managerAuth.CurrentUser;

		if (targetObj != null) {
			targetObj.SetActive (false);
			is_active_now = targetObj.activeSelf;
		}

		rend = GetComponent<Renderer> ();
		button_active_flag = false;
		buttonList = GameObject.FindGameObjectsWithTag ("button");

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
	void buttonActive(){
		
		if (button_active_flag == false) {
			rend.material.color = Color.red;
			button_active_flag = true;
            string[] objectName = Regex.Split(targetObj.ToString(), "\\s+");
            string food;

            reference.Child("Database").Child(objectName[0].ToString()).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception.Message);

                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    food = snapshot.Child("name").Value.ToString();
                    writeNewUser(objectName[0].ToString(), food);
                }
            });
            
            for (int i = 0; i < buttonList.Length; i++) {
				if (buttonList [i] != this.gameObject) {
					buttonList [i].SetActive (false);
				}
			}

		} else {
			rend.material.color = Color.green;
			button_active_flag = false;
			for (int i = 0; i < buttonList.Length; i++) {
				
				buttonList [i].SetActive (true);

			}

		}
	}


	void OnMouseDown(){
        
		targetObjACtive ();
		buttonActive ();
	}

	void OnMouseUp(){

	}

	// Update is called once per frame
	void Update () {

	}

    public class User
    {
        public string userID;
        public string date;
        public string trackingObject;
        public string name;
        public User() {
            // Constructor
        }

        public User(touchScript id, string trackingObject, string food, string date)
        {
            this.userID = id.loginUser.UserId;
            this.trackingObject = trackingObject;
            this.date = date;
            this.name = food;
        }
    }

    public void writeNewUser(string TObject, string food)
    {
        Debug.Log(trname + " two");
        User user = new User(this, TObject, food, DateTime.Now.ToString("yyyyMMdd"));
        //User user = new User(name, email);
        string json = JsonUtility.ToJson(user);
        string userid = loginUser.UserId;
        //string key = reference.Child(userid).Push().Key;
        reference.Child(userid).Child(TObject).SetRawJsonValueAsync(json);

        //reference.Child("users").Child("user2").SetRawJsonValueAsync(json);
        //reference.Child("users").Child("user2").Push();
        //reference.Child("users").Child("user2").Push();
    }
}

