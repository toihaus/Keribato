using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マップの移動
public class MapMove : MonoBehaviour
{
    public GeneralData Gd;

    public float speed;

    private float distance;
    private Vector3 targetPos;

    void Start()
    {
        distance = Global.MAP_SIZE; //移動量
        targetPos = transform.localPosition;
        Gd.move.x = 0;
        Gd.move.y = 0;
    }

    void FixedUpdate()
    {
        if (Gd.move != Vector2.zero && transform.localPosition == targetPos && Gd.move_flag == 0) //移動開始
        {
            targetPos += new Vector3(Gd.move.x * distance, Gd.move.y * distance, 0);
            Gd.move_flag = 1;
        }
        else if (Gd.move_flag == 1 && transform.localPosition != targetPos) //移動中
        {
            Gd.player_status = Global.MOVE;
            Move(targetPos);
        }
        else if (Gd.move_flag == 1 && transform.localPosition == targetPos) //移動終わり
        {
            Gd.move_flag = 0;
            Gd.move.x = 0;
            Gd.move.y = 0;

            Gd.player_status = Global.ATTACK;
            //ターンを増やす
            Gd.now_turn++;
        }
    }

    private void Move(Vector3 targetPosition)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
    }
}
