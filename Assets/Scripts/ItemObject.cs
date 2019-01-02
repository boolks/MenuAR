using UnityEngine;
using UnityEngine.UI;

// ReviewScene 에서 파이어베이스로부터 얻어온 데이터를 전부 출력해주기 위한 아이템리스트
public class ItemObject : MonoBehaviour
{
    public Button Item;
    public Text foodname;
    public Text date;
    public Button.ButtonClickedEvent OnItemClick;
}

