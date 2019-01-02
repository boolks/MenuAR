using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReviewDatabase : MonoBehaviour
{
    /// <summary>
    /// 아이템으로 사용할 오브젝트(프리팹).
    /// </summary>
    public GameObject ItemObject;
    /// <summary>
    /// 아이템이 추가될 오브젝트.
    /// </summary>
    public Transform Content;

    // 인증을 관리할 객체.
    public DatabaseReference reference;
    public FirebaseAuth managerAuth;
    public FirebaseUser loginUser;
    Firebase.Storage.StorageReference gs_reference;
    Firebase.Storage.FirebaseStorage storage;

    // 버튼의 값을 DetailScene로 넘겨주기 위한 전역변수.
    public static string namepass;

    // 아이템리스트를 생성하기 위한 변수.
    GameObject btnItemTemp;
    ItemObject itemobjectTemp;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Use this for initialization
    void Start()
    {
        storage = Firebase.Storage.FirebaseStorage.DefaultInstance;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://menuardb-7330a.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        managerAuth = FirebaseManager.auth;
        loginUser = managerAuth.CurrentUser;

        this.Binding();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("menuScene");
        }
    }

    /// <summary>
    /// 아이템 리스트를 지정된 프리팹으로 변환하여 추가.
    /// </summary>
    private void Binding()
    {
        reference.Child(loginUser.UserId).OrderByChild("date").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.Message);

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (var item in snapshot.Children)
                {
                    btnItemTemp = Instantiate(this.ItemObject) as GameObject;
                    //오브젝트가 가지고 있는 'ItemObject'를 찾음.
                    itemobjectTemp = btnItemTemp.GetComponent<ItemObject>();

                    //리스트뷰 추가 부분.
                    itemobjectTemp.date.text = item.Child("date").Value.ToString();
                    Debug.Log(item.Child("date").Value.ToString());
                    itemobjectTemp.foodname.text = item.Child("name").Value.ToString();
                    itemobjectTemp.OnItemClick.AddListener(delegate {
                        namepass = item.Child("trackingObject").Value.ToString();
                        Debug.Log(item.Child("trackingObject").Value.ToString());
                        SceneManager.LoadScene("DetailScene");
                    });
                    itemobjectTemp.Item.onClick = itemobjectTemp.OnItemClick;
                    btnItemTemp.transform.SetParent(this.Content);
                }
            }
        });
    }
}