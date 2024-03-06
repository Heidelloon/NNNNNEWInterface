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

        //코루틴은 여러개 작동할 수 있다.
        //해당 컴포넌트가 꺼져 있어도 작동한다.
        //if (!onTyping && Input.GetKeyDown(KeyCode.Space))
       // {
        //    StartCoroutine(textView(textList[textNum]));

            //textNum++;
 //       }

    }
    //_text를 한글자씩 charDist시간 마다 나오게 하는 함수
    IEnumerator textView(string _text)
    {
        onTyping = true;

        textViewer.text = "";
        TextBox.SetActive(true);

        for (int i = 0; i < _text.Length; i++)
        {
            //한글자 입력하는 코드
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
            //Debug.Log("모든 대화끝");
            if (!string.IsNullOrEmpty(SetNextScene))
            { 
                SceneManager.LoadScene(SetNextScene);
            }
        }    

        yield break;//코루틴 종료
        //특정조건이 만족할 때 break시켜
        //나머지 코드를 실행하지 않게 할 수 있음
        Debug.Log("이부분은 실행되지 않음");
    }

}
