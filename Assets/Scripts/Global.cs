using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//共通の固定データ関係
public class Global
{

    public const float MAP_SIZE = 144f; //マップ画像のサイズ

    //プレイヤーのステータス
    public const int STAY = 0;
    public const int MOVE = 1;
    public const int ATTACK = 2;
    public const int KICK = 3;
    public const int SKILL = 4;
    public const int ITEM = 5;
    public const int END = 99;
    public const int ROTATE = 10;
    public const int BACKATTACK = 20;

    //ゲームのステータス
    public const int PLAY_GAME_START = 10;
    public const int PLAY_MAP_MOVE = 20;
    public const int PLAY_GAME_END = 30;
}
