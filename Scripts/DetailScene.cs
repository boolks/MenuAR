using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 요리의 상세 정보를 보여주기 위한 클래스.
public class DetailScene : MonoBehaviour {
    /// <summary>
    /// 아이템이 추가될 오브젝트
    /// </summary>
    public string urltext;
    public Image img;
    public Text foodname;
    public Text explain;
    public Text recipe;

    // 파이어베이스의 현재 사용자의 정보를 얻어오기 위한 변수.
    public DatabaseReference reference;
    public FirebaseAuth managerAuth;
    public FirebaseUser loginUser;
    Firebase.Storage.StorageReference gs_reference;
    Firebase.Storage.FirebaseStorage storage;

    // Use this for initialization
    void Start () {
        storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://menuardb-7330a.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        managerAuth = FirebaseManager.auth;
        loginUser = managerAuth.CurrentUser;
        getData();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("ReviewScene");
        }
    }

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // 이전 씬에서 클릭한 버튼의 오브젝트 명을 토대로 데이터베이스로부터 설명하기 위한 값들을 받아옴.
    private void getData()
    {
        reference.Child("Database").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.Message);

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                
                urltext = snapshot.Child(ReviewDatabase.namepass).Child("img").Value.ToString();
                gs_reference = storage.GetReferenceFromUrl(urltext);
                Debug.Log(gs_reference);
                parsing();

                foodname.text = snapshot.Child(ReviewDatabase.namepass).Child("name").Value.ToString();
                Debug.Log(snapshot.Child(ReviewDatabase.namepass).Child("name").Value.ToString());
                explain.text = snapshot.Child(ReviewDatabase.namepass).Child("explain").Value.ToString();
                recipe.text = snapshot.Child(ReviewDatabase.namepass).Child("recipe").Value.ToString();
            }
        });
    }

    // 이미지를 데이터베이스의 stroage로 부터 파싱하여 화면 상의 sprite로 보여주기 위한 메소드
    private void parsing()
    {
        gs_reference.GetDownloadUrlAsync().ContinueWith((Task<Uri> task) => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(AccessURL(task.Result));
            }
            else
            {
                Debug.Log("URL Download Fault");
            }
        });
    }

    IEnumerator AccessURL(Uri result)
    {
        // 파이어베이스에서 이미지 파싱.
        WWW www = new WWW(result.ToString());
        Debug.Log("Download URL: " + result.ToString());
        
        // 다운로드가 완료될 때 까지 대기.
        yield return www;

        // texture로 받은 URL을 다시 sprite로 변경하여 화면의 이미지에 연결.
        Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.DXT1, false);
        www.LoadImageIntoTexture(texture);
        Rect rec = new Rect(0, 0, texture.width, texture.height);
        Sprite spriteToUse = Sprite.Create(texture, rec, new Vector2(0.5f, 0.5f), 100);
        img.sprite = spriteToUse;

        www.Dispose();
        www = null;
    }
}
