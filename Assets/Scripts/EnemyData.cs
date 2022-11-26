using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//敵キャラのデータ
[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    //敵キャラの名前
    public string enemy_name;

    //敵キャラのレベル
    public int enemy_level;

    //敵キャラのPrefab
    public GameObject enemy_prefab;

    //敵キャラの種類
    public int enemy_kind;

    //敵キャラのランク
    public int enemy_rank;

    //敵キャラの向き
    private int enemy_dir;

    public void SetLevel(int level)
    {
        enemy_level = level;
    }

    public int GetLevel()
    {
        return enemy_level;
    }

    public void SetDir(int dir)
    {
        enemy_dir = dir;
    }

    public int GetDir()
    {
        return enemy_dir;
    }


}
