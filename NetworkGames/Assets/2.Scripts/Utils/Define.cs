using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 전체에서 활용될 수 있는 enum, struct, const를 저장합니다.
public class Define
{
    public struct UIData
    {
        public string name;
        public GameObject gameObject;
        public UnityEngine.Object component;
    }

    public enum Scene
    {
        Awake, Load, Title, Lobby,
        InGame
    }

    public enum BGM
    {
        Title, Lobby
    }

    public enum SFX
    {
        JumpLand,
        TitleSlash, TitleDark, Entry
    }

    public struct RoomData
    {
        public string _name;
        public int _maxCount;
        public int _curCount;
    }
}
