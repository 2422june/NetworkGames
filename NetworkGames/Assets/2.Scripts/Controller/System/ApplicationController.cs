using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임의 엔트리 포인트이며 게임에 전체적으로 작동할 것 들을 셋팅하고 게임을 제어한다.

[RequireComponent(typeof(Managers))] // 필수 컴포넌트로 지정
public class ApplicationController : MonoBehaviour
{
    [SerializeField]
    private bool isDevelop;
    [SerializeField]
    private Define.Scene startScene;

    private void Awake()
    {
        Managers.Root = Util.GetOrAddComponent<Managers>(gameObject);
        Managers.Root.Init();
        DontDestroyOnLoad(gameObject);

        EnterGame();
    }

    private void EnterGame()
    {
        //Cursor.visible = false;


        if (isDevelop)
        {
            //Managers.Scene.LoadScene(startScene);
            return;
        }
        Managers.Scene.LoadScene(Define.Scene.Title);
    }
}
