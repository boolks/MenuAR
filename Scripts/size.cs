using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class size : MonoBehaviour {

    public float delayTime = 3;
    // Use this for initialization
    IEnumerator Start()
    {
       // Screen.SetResolution(1440,2560, true);
        yield return new WaitForSeconds(delayTime);

        SceneManager.LoadScene("loginScene");
    }

}
