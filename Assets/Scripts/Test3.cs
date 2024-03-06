using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


using UnityEngine.SceneManagement;

public class Test3 : MonoBehaviour
{
    public string[] textList;
    public int textNum = 0;

    public Text textViewer;
    public GameObject TextBox;

    public float charDist = 0.07f;

    bool onTyping = false;

    public string SetNextScene;

    // Start is called before the first frame update
    void Start()
    {
        TextBox.SetActive(false);

        StartCoroutine(textView(textList[textNum]));
    }

    // Update is called once per frame
    void Update()
    {

        //�ڷ�ƾ�� ������ �۵��� �� �ִ�.
        //�ش� ������Ʈ�� ���� �־ �۵��Ѵ�.
        //if (!onTyping && Input.GetKeyDown(KeyCode.Space))
       // {
        //    StartCoroutine(textView(textList[textNum]));

            //textNum++;
 //       }

    }
    //_text�� �ѱ��ھ� charDist�ð� ���� ������ �ϴ� �Լ�
    IEnumerator textView(string _text)
    {
        onTyping = true;

        textViewer.text = "";
        TextBox.SetActive(true);

        for (int i = 0; i < _text.Length; i++)
        {
            //�ѱ��� �Է��ϴ� �ڵ�
            textViewer.text += _text[i];

            yield return new WaitForSeconds(charDist);
        }

        yield return new WaitForSeconds(1f);
        TextBox.SetActive(false);


        onTyping = false;

        textNum++;
        if (textNum < textList.Length)
            StartCoroutine(textView(textList[textNum]));
        else
        {
            //Debug.Log("��� ��ȭ��");
            if (!string.IsNullOrEmpty(SetNextScene))
            { 
                SceneManager.LoadScene(SetNextScene);
            }
        }    

        yield break;//�ڷ�ƾ ����
        //Ư�������� ������ �� break����
        //������ �ڵ带 �������� �ʰ� �� �� ����
        Debug.Log("�̺κ��� ������� ����");
    }

}
