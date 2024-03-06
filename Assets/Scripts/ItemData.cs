using UnityEngine;

//메뉴에 추가하기 위한 부분
[CreateAssetMenu(fileName = "Item Data",
    menuName = "Scriptable Object/Item Data",
    order = int.MaxValue)]

//아이템 정보
//아이템 이름, 아이콘이미지, 아이템 설명, 무게, 최대 수량........
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite image;
    [TextArea(3, 10)]
    public string itemText;
    //public float weight;
    //public int maxCount;
}
