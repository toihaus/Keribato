using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵の移動
public class EnemyPush : MonoBehaviour, IEnemy
{
    public GeneralData Gd;

    public float speed = 500f;

    private float distance;
    private Vector3 targetPos;

    int now_num;
    int move_flag;

    void Start()
    {
        distance = Global.MAP_SIZE; //移動量
        targetPos = transform.localPosition;
    }

/*
    void FixedUpdate()
    {
        if (Gd.enemy[now_num].info.move != Vector2.zero && transform.localPosition == targetPos && move_flag == 0) //移動開始
        {
            Debug.Log("いどう" + now_num + "かいし");
            targetPos += new Vector3(Gd.enemy[now_num].info.move.x * distance, Gd.enemy[now_num].info.move.y * distance, 0);
            Debug.Log("targetPos：" + targetPos.x + "：" + targetPos.y);
            move_flag = 1;
        }
        else if (transform.localPosition != targetPos && move_flag == 1) //移動中
        {
            Debug.Log("いどうtyuu" + now_num + "い");
            Move(targetPos);
        }
        else if (transform.localPosition == targetPos && move_flag == 1) //移動終わり
        {
            Gd.enemy[now_num].info.move.x = 0;
            Gd.enemy[now_num].info.move.y = 0;
            move_flag = 0;
        }
    }

    private void Move(Vector3 targetPosition)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
        Debug.Log("localPos：" + transform.localPosition.x + "：" + transform.localPosition.y);
    }

//    */

    public void MoveDir(int num)
    {
        Debug.Log("おされた" + num);

        Gd.enemy_num = num;

        distance = Global.MAP_SIZE; //移動量

        Gd.enemy[num].info.move.x = 0;
        Gd.enemy[num].info.move.y = 0;

        transform.localPosition = new Vector3(Gd.map_yoko_init + Global.MAP_SIZE * Gd.enemy[num].info.x, Gd.map_tate_init - Global.MAP_SIZE * Gd.enemy[num].info.y, 0);
        targetPos = transform.localPosition;

        //上方向
        if (Gd.enemy[num].info.dir == 8)
        {
            Debug.Log("ue");
            Gd.enemy[num].info.move.x = 0;
            Gd.enemy[num].info.move.y = 1;
        }
        //下方向
        else if (Gd.enemy[num].info.dir == 2)
        {
            Debug.Log("sita");
            Gd.enemy[num].info.move.x = 0;
            Gd.enemy[num].info.move.y = -1;
        }
        //左方向
        else if (Gd.enemy[num].info.dir == 4)
        {
            Debug.Log("hidari");
            Gd.enemy[num].info.move.x = -1;
            Gd.enemy[num].info.move.y = 0;
        }
        //右方向
        else if (Gd.enemy[num].info.dir == 6)
        {
            Debug.Log("migi");
            Gd.enemy[num].info.move.x = 1;
            Gd.enemy[num].info.move.y = 0;
        }
        else
        {
            Gd.enemy[num].info.move.x = 0;
            Gd.enemy[num].info.move.y = 0;
        }

        Debug.Log("Pos：" + transform.localPosition.x + "：" + transform.localPosition.y);

        Debug.Log("nextPos：" + targetPos.x + "：" + targetPos.y + "：" + Gd.enemy_num);

        targetPos += new Vector3(Gd.enemy[num].info.move.x * distance, Gd.enemy[num].info.move.y * distance, 0);
        Gd.enemy[num].info.move_flag = 0;

        Debug.Log("targetPos：" + targetPos.x + "：" + targetPos.y);

    }
}
