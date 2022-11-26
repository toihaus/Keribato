using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ステージ作成
public class MapMake: MonoBehaviour
{
    public GeneralData Gd;

    public TextAsset map_file;

    public GameObject wall_prefab;
    public GameObject floor_prefab;

    int enemy_num;

    void Start()
    {
        Gd.floor_num = 0;
        LoadMapData();

        //床の数（余分なデータがあるっぽいので引く）
        Gd.floor_num -= Gd.map_tate_num - 1;

        MapInitPos();

        MapSetting();

        Gd.rotate_turn = 1;

        //プレイヤーの初期位置
        Gd.player_x = Gd.map_yoko_num / 2;
        Gd.player_y = Gd.map_tate_num / 2;

        Debug.Log("プレイヤー初期位置" + Gd.player_x + ":" + Gd.player_y);

        enemy_num = 0;

        //        List<EnemyData> enemy_info = new List<EnemyData>();

        EnemyKindRate();
        EnemySetting();

        Debug.Log(enemy_num);
        Gd.enemy_cnt = enemy_num;
    }

    //マップデータの読み込み
    void LoadMapData()
    {
        string[] map_tate_data;

        //マップデータを全部保管
        string map_all_data = map_file.text;

        //改行位置で分割
        map_tate_data = map_all_data.Split('\n');

        //行数と列数の取得
        Gd.map_yoko_num = map_tate_data[0].Split(',').Length;
        Gd.map_tate_num = map_tate_data.Length;

        //マップデータ領域確保
        Gd.map_data = new string[Gd.map_yoko_num, Gd.map_tate_num];

        Gd.map_enemy_num = new int[Gd.map_yoko_num, Gd.map_tate_num];

        for (int y = 0; y < Gd.map_tate_num; y++)
        {
            string[] temp_data = map_tate_data[y].Split(',');

            for (int x = 0; x < Gd.map_yoko_num; x++)
            {
                Gd.map_data[x, y] = temp_data[x];

                //壁じゃない時
                if(Gd.map_data[x, y] != "0")
                {
                    Gd.floor_num++;
                }
            }
        }
    }

    //マップの基準位置座標を決める（左上が０，０）
    void MapInitPos()
    {
        Gd.map_tate_init = (Gd.map_tate_num / 2) * Global.MAP_SIZE;
        Gd.map_yoko_init = (Gd.map_yoko_num / 2) * Global.MAP_SIZE * -1;
    }

    //マップを並べる
    void MapSetting()
    {
        //Canvas内のMapパネルに入れる
        GameObject target = GameObject.Find("Canvas").transform.Find("Map").gameObject;

        for (int y = 0; y < Gd.map_tate_num; y++)
        {
            for (int x = 0; x < Gd.map_yoko_num; x++)
            {
                GameObject map_pos;

                Gd.map_enemy_num[x, y] = -1;

                Vector3 pos = new Vector3(Gd.map_yoko_init + Global.MAP_SIZE * x, Gd.map_tate_init - Global.MAP_SIZE * y, 0f);
                transform.localPosition = pos;

                if (Gd.map_data[x, y] == "0") //壁
                {
                    map_pos = Instantiate(wall_prefab, transform.position, Quaternion.identity, target.transform);
                }
                else if(Gd.map_data[x, y] == "1") //床
                {
                    map_pos = Instantiate(floor_prefab, transform.position, Quaternion.identity, target.transform);
                }
                else //該当データ無しの時は壁
                {
                    map_pos = Instantiate(wall_prefab, transform.position, Quaternion.identity, target.transform);
                    Gd.map_data[x, y] = "0";
                }

                map_pos.transform.localPosition = pos;
            }
        }
    }

    public EnemyDataBase Edb;

    //敵キャラの初期値
    int enemy_level1_num;
    int enemy_level2_num;
    int enemy_level3_num;
    int enemy_level4_num;

    //敵キャラの種類の割合計算
    void EnemyKindRate()
    {
        //床の数の1割がLv4の敵数
        enemy_level4_num = Gd.floor_num / 10;

        //床の数の2割がLv3の敵数
        enemy_level3_num = enemy_level4_num * 2;

        //床の数の3割がLv2の敵数
        enemy_level2_num = enemy_level4_num * 3;

        //残りがLv1の敵数
        enemy_level1_num = Gd.floor_num - enemy_level4_num - enemy_level3_num - enemy_level2_num;

    }

    //敵キャラを配置
    void EnemySetting()
    {
        int kind;

        for (int y = 0; y < Gd.map_tate_num; y++)
        {
            for (int x = 0; x < Gd.map_yoko_num; x++)
            {
                //床以外の場合はパス
                if (Gd.map_data[x, y] != "1") continue;

                //プレイヤーの初期位置の場合もパス
                if (Gd.player_x == x && Gd.player_y == y)
                {
                    Gd.map_enemy_num[x, y] = enemy_num;
                    Gd.enemy.Add(new Enemy(enemy_num, "", 0, null, 0, 0, 0));
                    enemy_num++;
                    continue;
                }

                //初手で手詰まり防止用（前後左右はLv1の敵：運悪く40％を使い切ってたらLv2以上のガチャ行き）
                if (Gd.player_x - 1 == x && Gd.player_y == y && enemy_level1_num > 0)
                {
                    kind = 1;
                    enemy_level1_num--;
                }
                else if (Gd.player_x + 1 == x && Gd.player_y == y && enemy_level1_num > 0)
                {
                    kind = 1;
                    enemy_level1_num--;
                }
                else if (Gd.player_x == x && Gd.player_y - 1 == y && enemy_level1_num > 0)
                {
                    kind = 1;
                    enemy_level1_num--;
                }
                else if (Gd.player_x == x && Gd.player_y + 1 == y && enemy_level1_num > 0)
                {
                    kind = 1;
                    enemy_level1_num--;
                }
                else
                {
                    //敵ガチャ
                    kind = EnemyGacha();
                }

                EnemyMake(x, y, kind);
            }
        }
    }

    //初期割合通りに敵キャラ排出
    int EnemyGacha()
    {
        int kind;

        //とりあえずベタに１～１００で
        int gacha = Random.Range(1, 101);

        if (90 <= gacha && gacha <= 100)
        {
            kind = 4;
            enemy_level4_num--;

            //打ち止めの場合は再度がちゃがちゃと
            if(enemy_level4_num < 0)
            {
                kind = EnemyGacha();
            }
        }
        else if (70 <= gacha && gacha < 90)
        {
            kind = 3;
            enemy_level3_num--;

            //打ち止めの場合は再度がちゃがちゃと
            if (enemy_level3_num < 0)
            {
                kind = EnemyGacha();
            }
        }
        else if (40 <= gacha && gacha < 70)
        {
            kind = 2;
            enemy_level2_num--;

            //打ち止めの場合は再度がちゃがちゃと
            if (enemy_level2_num < 0)
            {
                kind = EnemyGacha();
            }
        }
        else
        {
            kind = 1;
            enemy_level1_num--;

            //打ち止めの場合は再度がちゃがちゃと
            if (enemy_level1_num < 0)
            {
                kind = EnemyGacha();
            }
        }

        return kind;
    }

    //敵キャラを作成（ｘ、ｙ、種類）
    void EnemyMake(int x, int y, int kind)
    {

        //Canvas内のMapパネルに入れる
        GameObject target = GameObject.Find("Canvas").transform.Find("Map").gameObject;
        GameObject map_pos;

        Vector3 pos = new Vector3(Gd.map_yoko_init + Global.MAP_SIZE * x, Gd.map_tate_init - Global.MAP_SIZE * y, 0f);
        transform.localPosition = pos;

        Gd.map_enemy_num[x, y] = enemy_num;

        EnemyData Ed;
        Ed = Edb.GetEnemyDataLists()[kind];
        Ed.enemy_kind = kind;
        Ed.SetDir(Random.Range(1, 5) * 2);

        Gd.enemy.Add(new Enemy(enemy_num, Ed.enemy_name, Ed.GetLevel(), Ed.enemy_prefab, Ed.enemy_kind, Ed.GetDir(),0));

        map_pos = Instantiate(Ed.enemy_prefab, transform.position, Quaternion.identity, target.transform);

        map_pos.transform.localPosition = pos;
        map_pos.GetComponent<EnemyInit>().SetLevel(x, y);
        map_pos.GetComponent<EnemyInit>().SetRotate(Ed.GetDir());

        enemy_num++;

    }

}
