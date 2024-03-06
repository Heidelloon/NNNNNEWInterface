using UnityEngine;

//�޴��� �߰��ϱ� ���� �κ�
[CreateAssetMenu(fileName = "Item Data",
    menuName = "Scriptable Object/Item Data",
    order = int.MaxValue)]

//������ ����
//������ �̸�, �������̹���, ������ ����, ����, �ִ� ����........
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite image;
    [TextArea(3, 10)]
    public string itemText;
    //public float weight;
    //public int maxCount;
}
