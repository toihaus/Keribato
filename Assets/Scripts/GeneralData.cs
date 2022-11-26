using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GeneralData : ScriptableObject
{
    public float map_tate_init; //y軸の基準位置（左上）
    public float map_yoko_init; //x軸の基準位置（左上）

    public int map_tate_num; //yマスの数
    public int map_yoko_num; //xマスの数

    //マップの情報（壁と床）
    public string[,] map_data;

    //マップの情報（敵の番号を格納：マップ情報に統合の予定）
    public int[,] map_enemy_num;

    //敵の構造体を格納するリスト
    public List<Enemy> enemy = new List<Enemy>();

    public int enemy_num;
    public int target_enemy_num;
    public int backattack_enemy_num;
    public Enemy temp_enemy_data;

    //    public List<EnemyData> enemy_info;
    /*
        public static List<ENEMY> enemy_info = new List<ENEMY>();


        //敵の情報を格納
        public struct ENEMY
        {
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
        }

    //*/


    //床の数
    public int floor_num;

    //プレイヤーの向き
    public int player_dir;

    //プレイヤーの位置座標
    public int player_x;
    public int player_y;

    //プレイヤーの元座標
    public int player_old_x;
    public int player_old_y;

    //プレイヤーのステータス
    public int player_level;
    public int player_maxhp;
    public int player_hp;
    public int player_nextexp;
    public int player_exp;

    public int player_status;

    //プレイヤーの移動方向
    public Vector2 move;
    public int move_flag;

    //ターン数
    public int now_turn;
    public int rotate_turn;
    public int rotate_cnt;
    public int enemy_cnt;


    //次に必要な経験値
    public int NextExp(int lv)
    {
        return lv * lv + lv * 6;
    }

    //次の移動場所が移動可能かどうか
    public bool NextMoveCheck(int x, int y)
    {
        //マップ範囲外に出る場合はダメ（念のため）
        if(0 > x || x >= map_yoko_num)
        {
            return false;
        }

        if (0 > y || y >= map_tate_num)
        {
            return false;
        }

        //壁の場合はダメ
        if (map_data[x,y] == "0")
        {
            return false;
        }

        //nullの場合はダメ（念のため）
        if (map_data[x, y] == null)
        {
            return false;
        }

        return true;
    }

}
