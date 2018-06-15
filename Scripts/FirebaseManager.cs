using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 로그인을 관리하는 메인 클래스.
public class FirebaseManager : MonoBehaviour
{
    // 인증을 관리할 객체
    public static Firebase.Auth.FirebaseAuth auth;
    string authCode;
    // Use this for initialization
    void Awake()
    {
        // 인증을 관리할 객체를 초기화 한다.
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // 구글 로그인을 위한 메소드.
    public void OnClickGoogleLogin()
    {
        InitGoogle();

        Social.localUser.Authenticate(success =>
        {

            StartCoroutine(gmailLogin());
        });
    }
    
    IEnumerator gmailLogin()
    {
        while (System.String.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
            yield return null;

        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);

        // auth의 값이 파이어베이스에 제대로 전달 될 경우 로그인을 실행하는 작업.
        auth.SignInWithCredentialAsync(credential).ContinueWith(
            task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignIn canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignIn error: " + task.Exception);
                    return;
                }

                if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                {
                    // User is now signed in.
                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    SceneManager.LoadScene("menuScene");
                }
            });

        // 여러 인증 제공업체를 통해 로그인을 할 경우 한 가지로 관리하기 위한 설정.
        auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("LinkWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            SceneManager.LoadScene("menuScene");
        });
    }

    // 구글 로그인을 위해 PlayGamePlatform으로 부터 초기화하고 토큰을 받아옴.
    void InitGoogle()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void loadEmailScene()
    {
        SceneManager.LoadScene("emailScene");
    }

    public void EmailLoginScene()
    {
        SceneManager.LoadScene("EmailLoginScene");
    }
}