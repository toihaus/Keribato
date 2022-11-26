using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボタン操作の挙動関係
public class ButtonScript : MonoBehaviour
{
    public GeneralData Gd;

    public EnemyDataBase Edb;

    public Animator animator01 = null;

    GameObject massage_window;

    void Start()
    {
        //メッセージウィンドウを探す
        massage_window = GameObject.Find("Window");
//        massage_window.SetActive(false);
    }

    public void PushUp()
    {
        //移動中はパス
        if (Gd.move_flag == 1) return;

        ResetDir();
        animator01.SetBool("Up", true);
        Gd.player_dir = 8;
        massage_window.SetActive(false);
    }

    public void PushDown()
    {
        //移動中はパス
        if (Gd.move_flag == 1) return;

        ResetDir();
        animator01.SetBool("Down", true);
        Gd.player_dir = 2;
        massage_window.SetActive(false);
    }

    public void PushLeft()
    {
        //移動中はパス
        if (Gd.move_flag == 1) return;

        ResetDir();
        animator01.SetBool("Left", true);
        Gd.player_dir = 4;
        massage_window.SetActive(false);
    }

    public void PushRight()
    {
        //移動中はパス
        if (Gd.move_flag == 1) return;

        ResetDir();
        animator01.SetBool("Right", true);
        Gd.player_dir = 6;
        massage_window.SetActive(false);
    }

    void ResetDir()
    {
        animator01.SetBool("Up", false);
        animator01.SetBool("Down", false);
        animator01.SetBool("Left", false);
        animator01.SetBool("Right", false);
    }

    public void PushMove()
    {
        //移動中はパス
        if (Gd.move_flag == 1) return;

        Gd.move.x = 0;
        Gd.move.y = 0;

        //上向き
        if (Gd.player_dir == 8)
        {
            int num = Gd.map_enemy_num[Gd.player_x, Gd.player_y - 1];
            int dir = Gd.enemy[num].info.dir;
        }
        //下向き
        else if(Gd.player_dir == 2)
        {
            int num = Gd.map_enemy_num[Gd.player_x, Gd.player_y + 1];
            int dir = Gd.enemy[num].info.dir;
        }
        //左向き
        else if(Gd.player_dir == 4)
        {
            int num = Gd.map_enemy_num[Gd.player_x - 1 , Gd.player_y];
            int dir = Gd.enemy[num].info.dir;
        }
        //右向き
        else
        {
            int num = Gd.map_enemy_num[Gd.player_x + 1 , Gd.player_y];
            int dir = Gd.enemy[num].info.dir;
        }

        //現在地を保管
        Gd.player_old_x = Gd.player_x;
        Gd.player_old_y = Gd.player_y;

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

        Gd.player_x -= (int)Gd.move.x;
        Gd.player_y += (int)Gd.move.y;

        //移動先が移動可能かチェック
        if (Gd.NextMoveCheck(Gd.player_x, Gd.player_y))
        {
            int num = Gd.map_enemy_num[Gd.player_x, Gd.player_y];

            int enemy_dir = Gd.enemy[num].info.dir;

            massage_window.SetActive(true);
            GameObject.Find("Canvas").GetComponent<MassageWindow>().ResetMessage();

            /*
            //敵のうしろを取った場合（向き同じ）
            if (enemy_dir == Gd.player_dir)
            {
                //ニケはキックなので移動無し
                Gd.player_x = Gd.player_old_x;
                Gd.player_y = Gd.player_old_y;

                Gd.move.x = 0;
                Gd.move.y = 0;
            }
            else if ((10 - enemy_dir) == Gd.player_dir) //向き合った場合
            {
                massage_window.SetActive(true);
                GameObject.Find("Canvas").GetComponent<MassageWindow>().ResetMessage();
            }
            else
            {
                massage_window.SetActive(true);
                GameObject.Find("Canvas").GetComponent<MassageWindow>().ResetMessage();
            }
            //*/
        }
        else
        {
            //移動できない場合は座標を戻す
            Gd.player_x = Gd.player_old_x;
            Gd.player_y = Gd.player_old_y;

            Gd.move.x = 0;
            Gd.move.y = 0;
        }
    }

    //未実装
    void MoveDir(int num)
    {
        Gd.enemy_num = num;

        Gd.enemy[num].info.move.x = 0;
        Gd.enemy[num].info.move.y = 0;

        //上方向
        if (Gd.enemy[num].info.dir == 8)
        {
            Gd.enemy[num].info.move.x = 0;
            Gd.enemy[num].info.move.y = 1;
        }
        //下方向
        else if (Gd.enemy[num].info.dir == 2)
        {
            Gd.enemy[num].info.move.x = 0;
            Gd.enemy[num].info.move.y = -1;
        }
        //左方向
        else if (Gd.enemy[num].info.dir == 4)
        {
            Gd.enemy[num].info.move.x = -1;
            Gd.enemy[num].info.move.y = 0;
        }
        //右方向
        else if (Gd.enemy[num].info.dir == 6)
        {
            Gd.enemy[num].info.move.x = 1;
            Gd.enemy[num].info.move.y = 0;
        }

        Gd.enemy[num].info.move_flag = 0;
    }

}
