using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour //인트로 화면 Mgr 화면 연출용
{
    public Image rightRedBG;
    public Image blackBG;
    public Image loading;
    public Image loadingCircle;

    public Image btn;
    public Sprite[] btn_Images;
    private int btn_index = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ButtonMove();
        GameStart();
    }

    void ButtonMove() // 인트로 화면 선택된 버튼 변경
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (++btn_index >= 4)
            {
                btn_index = 0;
            }
            btn.sprite = btn_Images[btn_index];
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (--btn_index <= -1)
            {
                btn_index = 3;
            }
            btn.sprite = btn_Images[btn_index];
        }
    }

    void GameStart() // 게임 시작 상태에서 스페이스바 누를 시 호출
    {
        if(btn_index == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeoutScene());
        }
    }

    IEnumerator FadeoutScene() // 빨간 배경 애니메이션
    {
        Debug.Log(Time.deltaTime);
        while(true)
        {
            rightRedBG.rectTransform.Translate(-600 * Time.deltaTime, 0, 0);
            if(rightRedBG.rectTransform.position.x < 1860)
            {
                break;
            }
            yield return null;
        }
        StartCoroutine(CircleSpin()); // Loading bar
        StartCoroutine(Fadeout()); // 리얼 black fade out
    }

    IEnumerator Fadeout()
    {
        float alpha = 0;
        while(true)
        {
            alpha += Time.deltaTime;
            blackBG.color = new Color(0, 0, 0, alpha);
            if (blackBG.color.a >= 1.0f)
            {
                break;
            }
            yield return null;
        }
        alpha = 0;
        while (true)
        {
            alpha += Time.deltaTime;
            loading.color = new Color(1, 1, 1, alpha);
            loadingCircle.color = new Color(1, 1, 1, alpha);
            if (loading.color.a >= 1.0f)
            {
                break;
            }
            yield return null;
        }
    }

    IEnumerator CircleSpin()
    {
        while(true)
        {
            loadingCircle.rectTransform.Rotate(0, 0, -32 * Time.deltaTime);
            yield return null;
        }
    }
}