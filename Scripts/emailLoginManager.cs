using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class emailLoginManager : FirebaseManager {
    
    // 이메일 InputField.
    [SerializeField]
    InputField emailInput;
    // 비밀번호 InputField.
    [SerializeField]
    InputField passInput;
    // 결과를 알려줄 텍스트.
    [SerializeField]
    InputField passConfirm;

    [SerializeField]
    Text resultText;
    
    void Start () {
		
	}

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("loginScene");
        }
    }

    // 회원가입 버튼을 눌렀을 때 작동할 함수.
    public void SignUp()
    {
        string email = emailInput.text;
        string password = passInput.text;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
       
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            resultText.text = "빈칸이 존재합니다.";
            return;
        }
        else if (password.Equals(passConfirm.text) == false)
        {
            resultText.text = "비밀번호를 확인하세요";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>  // auth 객체를 이용해서 Email, Password 비동기식 생성.
        {
            if (task.IsFaulted)
            {
                resultText.text = "동일한 메일이 존재합니다";
            }
            else
            {
                FirebaseUser user = task.Result;
                user.SendEmailVerificationAsync().ContinueWith(t =>       // user 객체를 이용해서 Email Verification 메일을 보낸다.
                {
                    resultText.text = "확인 메일 전송";

                });
            }
        });
    }

    // 로그인 버튼을 눌렀을 때 작동할 함수
    public void SignIn()
    {
        auth = FirebaseAuth.DefaultInstance;
        string email = emailInput.text;
        string password = passInput.text;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if(task.IsFaulted || task.IsCanceled)
            {
                resultText.text = "메일이 존재하지 않거나 비밀번호가 다릅니다";
            }
            else
            {
                FirebaseUser user = task.Result;
                PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
                if (user.IsEmailVerified)
                {
                    SceneManager.LoadScene("menuScene");
                }        // 이메일이 검증되었다면 Scene 전환
                else
                {
                    resultText.text = "이메일 검증이 필요합니다";    // Email 검증이 필요하다는 메시지 출력.
                }
            }
        });
    }
}
