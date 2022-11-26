using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo
{
    public int num;

    //敵キャラの名前
    public string name;

    //敵キャラのレベル
    public int level;

    //敵キャラのPrefab
    public GameObject prefab;

    //敵キャラの種類
    public int kind;

    //敵キャラの向き
    public int dir;

    public Vector2 move;
    public int move_flag;

    public int x;
    public int y;

    public int rotate_flag;
}
