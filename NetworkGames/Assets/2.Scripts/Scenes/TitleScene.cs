using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : SceneBase
{
    [SerializeField]
    private GameObject[] panels = new GameObject[4];

    [SerializeField]
    private Image[] fadeImage = new Image[3];
    private float _a;

    private void Start()
    {
        Init();
        InitUI();
        StartCoroutine(Loading());
    }

    protected override void Init()
    {
        _type = Define.Scene.Title;
        _name = "Title";

        _a = 1;
    }

    protected override void InitUI() { }

    private IEnumerator Loading()
    {
        int index = 1;
        yield return new WaitForSeconds(2);
        panels[index].gameObject.SetActive(true);
        Managers.Audio.PlaySFX(Define.SFX.TitleSlash, transform);
        index++;

        yield return new WaitForSeconds(3);
        panels[index].gameObject.SetActive(true);
        Managers.Audio.PlaySFX(Define.SFX.TitleDark, transform);
        Managers.Audio.PlayBGM(Define.BGM.Title);
        index++;

        yield return new WaitForSeconds(2);
        panels[index].gameObject.SetActive(true);
        Color color;

        while (_a > 0)
        {
            _a -= Time.deltaTime;

            color = fadeImage[0].color;
            color.a = _a;
            fadeImage[0].color = color;

            color = fadeImage[1].color;
            color.a = _a;
            fadeImage[1].color = color;

            color = fadeImage[2].color;
            color.a = _a;
            fadeImage[2].color = color;

            yield return null;
        }

        fadeImage[0].gameObject.SetActive(false);
        fadeImage[1].gameObject.SetActive(false);
        fadeImage[2].gameObject.SetActive(false);
        yield return null;
    }

    protected override void OnLoad()
    {
        Managers.Scene.OnLoad(this);
    }

    public void OnClickStart()
    {
        Managers.Audio.StopBGM();
        Managers.Audio.PlaySFX(Define.SFX.Entry, transform);
        Managers.Scene.LoadScene(Define.Scene.Lobby, true);
    }
}
