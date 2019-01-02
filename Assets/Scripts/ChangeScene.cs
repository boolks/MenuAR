using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬 전환을 위한 클래스.
public class ChangeScene : FirebaseManager {

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 첫 화면에서 뒤로가기를 누를 경우 로그아웃과 함께 어플리케이션 종료.
            auth.SignOut();
            SceneManager.LoadScene("loginScene");
        }
    }
#endif

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
    public void SceneChange1()
    {
        SceneManager.LoadScene("CapChef_ARCamera");
    }

    public void SceneChange2()
    {
        SceneManager.LoadScene("ReviewScene");
    }
}
