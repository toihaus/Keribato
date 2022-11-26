using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Nikeに関するコード全般
public class PlayerControl : MonoBehaviour
{
    public GeneralData Gd;

    public Animator nike_anime= null;

    public Text now_level;
    public Text player_level;

    public Text now_exp;
    public Image exp_gauge;

    public Text now_hp;
    public Text now_maxhp;
    public Image hp_gauge;


    GameObject massage_window;

    int nike_atk_flag;

    void Start()
    {
        PlaterInit();

        //メッセージウィンドウを探す
        massage_window = GameObject.Find("Window");
        massage_window.SetActive(false);

        Debug.Log("player init" + Gd.player_maxhp + ":" + Gd.player_dir);

    }

    void Update()
    {
        PlayerStatus();

        //マップ移動中以外の時のみ入力を受け付ける
        if (Gd.move_flag != 1)
        {

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            // キーが押された時（向き）
            if (x == 1 || y == 1 || x == -1 || y == -1)
            {
                NikeChangeDir(x, y);
            }
            else if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire3"))) //攻撃
            {
                massage_window.SetActive(false);
                NikeAttack();
            }
        }
    }

    //プレイヤーの初期化
    void PlaterInit()
    {
        Gd.player_dir = 2;

        Gd.player_level = 1;

        Gd.player_nextexp = Gd.NextExp(Gd.player_level);
        Gd.player_exp = 0;
        Gd.player_maxhp = 15;
        Gd.player_hp = 15;

        Gd.player_status = Global.STAY;

        Gd.move_flag = 0;

    }

    //プレイヤーのステータス表示
    void PlayerStatus()
    {
        exp_gauge.fillAmount = (float)(Gd.player_exp) / (float)(Gd.player_nextexp);
        hp_gauge.fillAmount = (float)(Gd.player_hp) / (float)(Gd.player_maxhp);

        float next_exp = Gd.player_nextexp - Gd.player_exp;

        //レベルを表示
        now_level.text = Gd.player_level.ToString();

        //アイコン上のレベルを表示
        player_level.text = Gd.player_level.ToString();

        //必要な経験値を表示
        now_exp.text = next_exp.ToString();
        //        now_exp.text = Gd.player_exp.ToString();

        //体力を表示
        now_hp.text = Gd.player_hp.ToString();

        //最大体力を表示
        now_maxhp.text = Gd.player_maxhp.ToString();

    }

    //ニケの方向転換
    void NikeChangeDir(float x, float y)
    {
        int nx = 0;
        int ny = 0;

        //向き決め
        if (x < 0) // 左
        {
            Gd.player_dir = 4;
            nx = -1;
            PlayerMoveCheck(nx, ny);
            PushLeft();
        }
        else if (x > 0) // 右
        {
            Gd.player_dir = 6;
            nx = 1;
            PlayerMoveCheck(nx, ny);
            PushRight();
        }
        else if (y > 0) // 上
        {
            Gd.player_dir = 8;
            ny = -1;
            PlayerMoveCheck(nx, ny);
            PushUp();
        }
        else // 下
        {
            Gd.player_dir = 2;
            ny = 1;
            PlayerMoveCheck(nx, ny);
            PushDown();
        }

    }

    //移動可能かチェックからの挙動変更
    void PlayerMoveCheck(int nx,int ny)
    {
        //移動先が移動可能かチェック
        if (Gd.NextMoveCheck(Gd.player_x + nx, Gd.player_y + ny))
        {
            int num = Gd.map_enemy_num[Gd.player_x + nx, Gd.player_y + ny];
            int enemy_dir = Gd.enemy[num].info.dir;

            Debug.Log("プレイヤーnum:"+num+":lv:"+Gd.enemy[num].info.level + ":" + Gd.player_x +"+"+ nx +":"+Gd.player_y +"+"+ ny);

            if (enemy_dir == Gd.player_dir)
            {
                //ニケをキック状態
                nike_atk_flag = 1;
            }
            else
            {
                nike_atk_flag = 0;
            }
        }
    }

    //ニケのアニメーション切り替え部分。要改善１
    public void PushUp()
    {
        ResetDir();
        if (nike_atk_flag == 0)
        {
            nike_anime.SetBool("Up", true);
        }
        else
        {
            nike_anime.SetBool("UpKick", true);
        }
    }

    public void PushDown()
    {
        ResetDir();
        if (nike_atk_flag == 0)
        {
            nike_anime.SetBool("Down", true);
        }
        else
        {
            nike_anime.SetBool("DownKick", true);
        }
    }

    public void PushLeft()
    {
        ResetDir();
        if (nike_atk_flag == 0)
        {
            nike_anime.SetBool("Left", true);
        }
        else
        {
            nike_anime.SetBool("LeftKick", true);
        }
    }

    public void PushRight()
    {
        ResetDir();
        if (nike_atk_flag == 0)
        {
            nike_anime.SetBool("Right", true);
        }
        else
        {
            nike_anime.SetBool("RightKick", true);
        }
    }

    void ResetDir()
    {
        nike_anime.SetBool("Up", false);
        nike_anime.SetBool("Down", false);
        nike_anime.SetBool("Left", false);
        nike_anime.SetBool("Right", false);
        nike_anime.SetBool("UpKick", false);
        nike_anime.SetBool("DownKick", false);
        nike_anime.SetBool("LeftKick", false);
        nike_anime.SetBool("RightKick", false);
    }
    //要改善１ここまで

    //ニケ正面攻撃
    void NikeAttack()
    {
        int nx = 0;
        int ny = 0;

        //ニケの向き
        if (Gd.player_dir == 4) // 左
        {
            nx = -1;
            PlayerAttackCheck(nx, ny);
        }
        else if (Gd.player_dir == 6) // 右
        {
            nx = 1;
            PlayerAttackCheck(nx, ny);
        }
        else if (Gd.player_dir == 8) // 上
        {
            ny = -1;
            PlayerAttackCheck(nx, ny);
        }
        else // 下
        {
            ny = 1;
            PlayerAttackCheck(nx, ny);
        }

    }

    //移動可能かのチェックからの攻撃：要改善２
    void PlayerAttackCheck(int nx, int ny)
    {
        //移動先が移動可能かチェック
        if (Gd.NextMoveCheck(Gd.player_x + nx, Gd.player_y + ny))
        {
            int num = Gd.map_enemy_num[Gd.player_x + nx, Gd.player_y + ny];
            int enemy_dir = Gd.enemy[num].info.dir;

            Gd.move_flag = 0;

            if (enemy_dir == Gd.player_dir)
            {
                //キック
                Gd.backattack_enemy_num = num;
                Gd.player_status = Global.KICK;

                massage_window.SetActive(true);
                GameObject.Find("Canvas").GetComponent<MassageWindow>().ResetMessage();
            }
            else
            {
                //上方向
                if (Gd.player_dir == 8)
                {
                    Gd.move.x = 0;
                    Gd.move.y = -1;
                }
                //下方向
                else if (Gd.player_dir == 2)
                {
                    Gd.move.x = 0;
                    Gd.move.y = 1;
                }
                //左方向
                else if (Gd.player_dir == 4)
                {
                    Gd.move.x = 1;
                    Gd.move.y = 0;
                }
                //右方向
                else if (Gd.player_dir == 6)
                {
                    Gd.move.x = -1;
                    Gd.move.y = 0;
                }
                else
                {
                    Gd.move.x = 0;
                    Gd.move.y = 0;
                }

                //現在地を保管
                Gd.player_old_x = Gd.player_x;
                Gd.player_old_y = Gd.player_y;

                //プレイヤー位置に反映
                Gd.player_x -= (int)Gd.move.x;
                Gd.player_y += (int)Gd.move.y;

                massage_window.SetActive(true);
                GameObject.Find("Canvas").GetComponent<MassageWindow>().ResetMessage();

            }
        }
    }


}
