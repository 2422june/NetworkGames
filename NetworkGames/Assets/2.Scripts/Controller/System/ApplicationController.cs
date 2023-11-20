using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ��Ʈ�� ����Ʈ�̸� ���ӿ� ��ü������ �۵��� �� ���� �����ϰ� ������ �����Ѵ�.

[RequireComponent(typeof(Managers))] // �ʼ� ������Ʈ�� ����
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
