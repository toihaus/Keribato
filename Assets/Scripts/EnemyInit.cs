using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//敵キャラの初期設定
public class EnemyInit : MonoBehaviour
{
    public GeneralData Gd;

    public EnemyDataBase Edb;

    //敵キャラの画像の種類
    int enemy_kind;
    public Text enemy_lv;
    int enemy_lv_num;

    //敵キャラの位置座標
    int enemy_x;
    int enemy_y;

    //敵キャラの向き
    int enemy_dir;

    public Animator enemy_animator = null;

    int enemy_num;

    float speed = 500f;

    private float distance;
    private Vector3 targetPos;

    //プレイヤーの移動方向
    Vector2 move;
    int move_flag;

    void Start()
    {
        move.x = 0;
        move.y = 0;
    }

    void Update()
    {
        //敵キャラレベルの色変更
        ChangeLevelColor();

        //攻撃された時
        if (Gd.player_status == Global.ATTACK)
        {
            //敵キャラの回転
//            EnemyRotate();

            //敵キャラ消滅
            EnemyLost();
        }
        //敵の回転
        else if (Gd.player_status == Global.ROTATE)
        {
            //敵キャラの回転
            EnemyRotate();
        }
        else if (Gd.player_status == Global.KICK && Gd.backattack_enemy_num == enemy_num)
        {
            if (move == Vector2.zero)
            {
                EnemyMoveStart();
            }
            EnemyMove();
        }
        else if (Gd.player_status == Global.BACKATTACK && Gd.target_enemy_num == enemy_num)
        {
            BackAttackEnemyDamage();
        }

/*        
        if (Gd.player_status == Global.STAY)
        {
            Gd.enemy[enemy_num].info.rotate_flag = 0;
        }
*/
    }

    //敵の向きで画像を変更する
    public void SetRotate(int dir)
    {
        //向きを決める
        if (dir == 8)
        {
            enemy_animator.SetBool("Up", true);
            Gd.enemy[enemy_num].info.dir = 8;
        }
        else if (dir == 2)
        {
            enemy_animator.SetBool("Down", true);
            Gd.enemy[enemy_num].info.dir = 2;
        }
        else if (dir == 4)
        {
            enemy_animator.SetBool("Left", true);
            Gd.enemy[enemy_num].info.dir = 4;
        }
        else if (dir == 6)
        {
            enemy_animator.SetBool("Right", true);
            Gd.enemy[enemy_num].info.dir = 6;
        }
        Gd.enemy[enemy_num].info.rotate_flag = 0;
//        Debug.Log("num:dir " + enemy_num + ":" + Gd.enemy[enemy_num].info.dir);
    }

    //敵のレベルを表示
    public void SetLevel(int x,int y)
    {
        enemy_num = Gd.map_enemy_num[x, y];

        enemy_lv_num = Gd.enemy[enemy_num].info.level;

        ChangeLevelColor();

        enemy_x = x;
        enemy_y = y;

        Gd.enemy[enemy_num].info.x = x;
        Gd.enemy[enemy_num].info.y = y;
    }

    //敵キャラレベルの色変更
    void ChangeLevelColor()
    {
        enemy_lv_num = Gd.enemy[enemy_num].info.level;

        if (enemy_lv_num < Gd.player_level)
        {
            enemy_lv.color = Color.green;
        }
        else if (enemy_lv_num == Gd.player_level)
        {
            enemy_lv.color = Color.white;
        }
        else if (enemy_lv_num > Gd.player_level)
        {
            enemy_lv.color = Color.red;
        }

        enemy_lv.text = enemy_lv_num.ToString();
    }

    //敵キャラを消す
    void EnemyLost()
    {
        if (enemy_x != Gd.player_x) return;
        if (enemy_y != Gd.player_y) return;

        int damage = 0;

        //敵のうしろを取った場合（向き同じ）
        if (Gd.enemy[enemy_num].info.dir == Gd.player_dir)
        {
            damage = 0;
        }
        else if((10 - Gd.enemy[enemy_num].info.dir) == Gd.player_dir) //敵と向き合った場合
        {
            //ダメージは元のレベル
            damage = Gd.enemy[enemy_num].info.kind;
        }
        else
        {
            //移動後の場所の敵のレベルが自分より低い場合
            if (Gd.player_level >= Gd.enemy[enemy_num].info.level)
            {
                //ダメージは１
                damage = 1;
            }
            else //Lvが高い場合
            {
                //ダメージは敵のレベルから自分のレベルを引いた数値
                damage = Gd.enemy[enemy_num].info.kind - Gd.player_level;
            }
        }

        Gd.player_hp -= damage;

        //体力が０以下
        if(Gd.player_hp <= 0)
        {
            Gd.player_hp = 0;
            GameObject.Find("Canvas").GetComponent<MassageWindow>().PlayerEndMessage(Gd.enemy[enemy_num].info.name, Gd.enemy[enemy_num].info.kind, damage);
            Gd.player_status = Global.END;
            return;
        }

        //経験値入手
        GetExp(enemy_num, damage);

        Gd.player_status = Global.ROTATE;

        Destroy(gameObject);

        for (int i = 0; i < Gd.enemy_cnt; i++)
        {
            Gd.enemy[i].info.rotate_flag = 0;
        }


        /*        if (Random.Range(0, 100) < 5)//宝箱設置
                {
                    SetTeasureBox(Gd.player_old_x, Gd.player_old_y);
                }
                else
                {
                    //元の場所に新しい敵を配置*/
        EnemyReMake(Gd.player_old_x, Gd.player_old_y, EnemyGacha() + Gd.player_level,0);
//        }

        Gd.rotate_cnt = 1;
    }

    //経験値入手
    void GetExp(int exp_num,int damage)
    {
        bool lv_up = false;

        Gd.player_exp += Gd.enemy[exp_num].info.kind;

        //レベルアップする時
        if (Gd.player_nextexp <= Gd.player_exp)
        {
            Gd.player_level++;
            Gd.player_exp -= Gd.player_nextexp;

            Gd.player_maxhp += 5;

            Gd.player_hp += 5;

            Gd.player_nextexp = Gd.NextExp(Gd.player_level);

            lv_up = true;
        }

        GameObject.Find("Canvas").GetComponent<MassageWindow>().EnemyEndMessage(Gd.enemy[exp_num].info.name, Gd.enemy[exp_num].info.kind, damage, lv_up);

    }

    //割合通りに敵キャラ排出
    int EnemyGacha()
    {
        int kind;

        //とりあえずベタに１～１００で
        int gacha = Random.Range(1, 101);

        if (90 <= gacha && gacha <= 100)
        {
            kind = 4;
        }
        else if (60 <= gacha && gacha < 90)
        {
            kind = 3;
        }
        else if (30 <= gacha && gacha < 60)
        {
            kind = 2;
        }
        else
        {
            kind = 1;
        }

        return kind;
    }

    //敵キャラを作成（ｘ、ｙ、種類、蹴られた後）
    void EnemyReMake(int x, int y, int kind,int kick)
    {
        //Canvas内のMapパネルに入れる
        GameObject target = GameObject.Find("Canvas").transform.Find("Map").gameObject;
        GameObject map_pos;

        Vector3 pos = new Vector3(Gd.map_yoko_init + Global.MAP_SIZE * x, Gd.map_tate_init - Global.MAP_SIZE * y, 0f);
        transform.localPosition = pos;

        enemy_num = Gd.map_enemy_num[x, y];

        EnemyData Ed;
        Ed = Edb.GetEnemyDataLists()[kind];
        Ed.enemy_kind = kind;
        if (kick == 0)
        {
            Ed.SetDir(Gd.player_dir);
        }
        else
        {
            Ed.SetDir(Random.Range(1, 5) * 2);
        }

        Gd.enemy[enemy_num] = new Enemy(enemy_num, Ed.enemy_name, Ed.GetLevel(), Ed.enemy_prefab, Ed.enemy_kind, Ed.GetDir(),0);

        map_pos = Instantiate(Ed.enemy_prefab, transform.position, Quaternion.identity, target.transform);

        map_pos.transform.localPosition = pos;
        map_pos.GetComponent<EnemyInit>().SetLevel(x, y);
        map_pos.GetComponent<EnemyInit>().SetRotate(Ed.GetDir());
        Gd.enemy[enemy_num].info.rotate_flag = 1;

    }

    public void EnemyRotate()
    {
        if (Gd.enemy[enemy_num].info.rotate_flag == 1) return;

        ResetDir();

        if (Gd.enemy[enemy_num].info.dir == 4)
        {
            enemy_animator.SetBool("Up", true);
            Gd.enemy[enemy_num].info.dir = 8;
        }
        else if(Gd.enemy[enemy_num].info.dir == 6)
        {
            enemy_animator.SetBool("Down", true);
            Gd.enemy[enemy_num].info.dir = 2;
        }
        else if (Gd.enemy[enemy_num].info.dir == 2)
        {
            enemy_animator.SetBool("Left", true);
            Gd.enemy[enemy_num].info.dir = 4;
        }
        else if (Gd.enemy[enemy_num].info.dir == 8)
        {
            enemy_animator.SetBool("Right", true);
            Gd.enemy[enemy_num].info.dir = 6;
        }

        Gd.rotate_cnt++;
        Gd.enemy[enemy_num].info.rotate_flag = 1;

//        Debug.Log("newnum:dir " + enemy_num + ":" + Gd.enemy[enemy_num].info.dir + ":cnt:" + Gd.rotate_cnt);
        if (Gd.rotate_cnt - 1 > Gd.enemy_cnt)
        {
            Gd.rotate_cnt = 0;
            Gd.player_status = Global.STAY;
        }
    }

    void ResetDir()
    {
        enemy_animator.SetBool("Up", false);
        enemy_animator.SetBool("Down", false);
        enemy_animator.SetBool("Left", false);
        enemy_animator.SetBool("Right", false);
    }

    /*
        //宝箱の設置
        void SetTeasureBox(int x, int y)
        {
            //Canvas内のMapパネルに入れる
            GameObject target = GameObject.Find("Canvas").transform.Find("Map").gameObject;
            GameObject map_pos;

            Vector3 pos = new Vector3(Gd.map_yoko_init + Global.MAP_SIZE * x, Gd.map_tate_init - Global.MAP_SIZE * y, 0f);
            transform.localPosition = pos;

            map_pos = Instantiate(teasuer_prefab, transform.position, Quaternion.identity, target.transform);

            map_pos.transform.localPosition = pos;
        }
    //*/

    //キックされた時の動作
    void EnemyMoveStart()
    {
        if (Gd.backattack_enemy_num != enemy_num) return;

        int nx = 0;
        int ny = 0;

        distance = Global.MAP_SIZE; //移動量
        targetPos = transform.localPosition;

        //上方向
        if (Gd.enemy[enemy_num].info.dir == 8)
        {
            move.x = 0;
            move.y = 1;
            ny = -1;
            BackAttackEnemyCheck(nx, ny);
        }
        //下方向
        else if (Gd.enemy[enemy_num].info.dir == 2)
        {
            move.x = 0;
            move.y = -1;
            ny = 1;
            BackAttackEnemyCheck(nx, ny);
        }
        //左方向
        else if (Gd.enemy[enemy_num].info.dir == 4)
        {
            move.x = -1;
            move.y = 0;
            nx = -1;
            BackAttackEnemyCheck(nx, ny);
        }
        //右方向
        else if (Gd.enemy[enemy_num].info.dir == 6)
        {
            move.x = 1;
            move.y = 0;
            nx = 1;
            BackAttackEnemyCheck(nx, ny);
        }

        move_flag = 0;

    }

    int kind_flag;
    int old_x;
    int old_y;

    //蹴り飛ばされた敵の移動
    private void EnemyMove()
    {
        if (move != Vector2.zero && transform.localPosition == targetPos && move_flag == 0) //移動開始
        {
            targetPos += new Vector3(move.x * distance, move.y * distance, 0);
            move_flag = 1;
        }
        else if (move_flag == 1 && transform.localPosition != targetPos) //移動中
        {
            Move(targetPos);
        }
        else if (move_flag == 1 && transform.localPosition == targetPos) //移動終わり
        {
            move_flag = 0;
            move.x = 0;
            move.y = 0;

            Destroy(gameObject);

            if (kind_flag == 1)
            {
                Gd.player_status = Global.STAY;
            }
            else
            {
                Debug.Log("今："+enemy_num + "これから" + Gd.backattack_enemy_num + "が" + Gd.target_enemy_num + "をバックアタック");
                Gd.player_status = Global.BACKATTACK;
                Gd.temp_enemy_data = Gd.enemy[enemy_num];
            }

            //元の場所に新しい敵を配置*/
            EnemyReMake(old_x, old_y, EnemyGacha() + Gd.player_level, 1);

        }
    }

    private void Move(Vector3 targetPosition)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
    }

    //背後からの攻撃された先の状況確認：要改善２
    void BackAttackEnemyCheck(int nx, int ny)
    {
        old_x = Gd.enemy[enemy_num].info.x;
        old_y = Gd.enemy[enemy_num].info.y;

        kind_flag = 0;

        //移動先が移動可能かチェック
        if (Gd.NextMoveCheck(old_x + nx, old_y + ny))
        {
            int num = Gd.map_enemy_num[old_x + nx, old_y + ny];
            int next_enemy_level = Gd.enemy[num].info.level;

            //蹴り飛ばされた先の敵の番号
            Gd.target_enemy_num = num;

            Debug.Log("Next:" + num + ":Level:" + next_enemy_level);

            //種類が同じ時
            if (Gd.enemy[num].info.kind == Gd.enemy[enemy_num].info.kind)
            {
                kind_flag = 1;
                //レベルを加算
                Gd.enemy[num].info.level += Gd.enemy[enemy_num].info.level;
                GameObject.Find("Canvas").GetComponent<MassageWindow>().EnemyFusionMessage(Gd.enemy[num].info.name, Gd.enemy[num].info.level);

            }
            else if (next_enemy_level > Gd.enemy[enemy_num].info.level)
            {
                kind_flag = 1;
                Gd.enemy[num].info.level -= Gd.enemy[enemy_num].info.level;

                //経験値入手
                GetExp(enemy_num, 0);

                Debug.Log("高レベル");
            }
            else
            {
                Gd.enemy[enemy_num].info.level--;
                Gd.temp_enemy_data = Gd.enemy[enemy_num];
                Debug.Log("低レベル");
            }
        }
        else //蹴った先が壁などの場合
        {
            Gd.enemy[enemy_num].info.level--;
            if (Gd.enemy[enemy_num].info.level <= 0)
            {
                Gd.enemy[enemy_num].info.level = 1;
            }
            move.x = 0;
            move.y = 0;
            Debug.Log("壁");
            Gd.player_status = Global.STAY;
        }
    }

    //背後からの攻撃された先の状況確認：要改善２
    void BackAttackEnemyDamage()
    {
        Debug.Log("新しく：" + enemy_num + "を攻撃" + Gd.target_enemy_num + "をバックアタック");

        //経験値入手
        GetExp(Gd.target_enemy_num, 0);

        Destroy(gameObject);

        //Canvas内のMapパネルに入れる
        GameObject target = GameObject.Find("Canvas").transform.Find("Map").gameObject;
        GameObject map_pos;
        int x = Gd.enemy[enemy_num].info.x;
        int y = Gd.enemy[enemy_num].info.y;

        Vector3 pos = new Vector3(Gd.map_yoko_init + Global.MAP_SIZE * x, Gd.map_tate_init - Global.MAP_SIZE * y, 0f);
        transform.localPosition = pos;

//        enemy_num = Gd.map_enemy_num[x, y];

        EnemyData Ed;
        Ed = Edb.GetEnemyDataLists()[Gd.temp_enemy_data.info.kind];
        Ed.enemy_kind = Gd.temp_enemy_data.info.kind;
        Ed.SetDir(Gd.temp_enemy_data.info.dir);

        Gd.enemy[enemy_num] = new Enemy(enemy_num, Ed.enemy_name, Gd.temp_enemy_data.info.level, Ed.enemy_prefab, Gd.temp_enemy_data.info.kind, Gd.temp_enemy_data.info.dir, 0);

        map_pos = Instantiate(Ed.enemy_prefab, transform.position, Quaternion.identity, target.transform);

        map_pos.transform.localPosition = pos;
        map_pos.GetComponent<EnemyInit>().SetLevel(x, y);
        map_pos.GetComponent<EnemyInit>().SetRotate(Gd.temp_enemy_data.info.dir);

        Debug.Log("作り直し終わり");

        Gd.player_status = Global.STAY;
    }

}
