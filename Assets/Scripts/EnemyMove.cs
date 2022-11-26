using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GeneralData Gd;

    public float speed = 500f;

    private float distance;
    private Vector3 targetPos;

    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        //Canvas内のMapパネルに入れる
        target = GameObject.Find("Canvas").transform.Find("Map").gameObject;

        distance = Global.MAP_SIZE; //移動量
        targetPos = transform.localPosition;
    }

    void Update()
    {
        if (Input.GetButton("Jump") || Input.GetButton("Fire3"))
        {
            Gd.enemy[Gd.enemy_num].info.move.x = 1;
            Gd.enemy[Gd.enemy_num].info.move.y = 0;
            Debug.Log("ぼたん");
        }
    }

    void FixedUpdate()
    {
        if (Gd.enemy[Gd.enemy_num].info.move != Vector2.zero && transform.localPosition == targetPos && Gd.enemy[Gd.enemy_num].info.move_flag == 0) //移動開始
        {
            Debug.Log("enePos：" + transform.localPosition.x + "：" + transform.localPosition.y + "：num" + Gd.enemy_num);
            targetPos += new Vector3(Gd.enemy[Gd.enemy_num].info.move.x * distance, Gd.enemy[Gd.enemy_num].info.move.y * distance, 0);
            Debug.Log("nextmapPos：" + targetPos.x + "：" + targetPos.y);
            Gd.enemy[Gd.enemy_num].info.move_flag = 1;
        }
        else if (transform.localPosition != targetPos && Gd.enemy[Gd.enemy_num].info.move_flag == 1) //移動中
        {
            Move(targetPos);
        }
        else if (transform.localPosition == targetPos && Gd.enemy[Gd.enemy_num].info.move_flag == 1) //移動終わり
        {
            Gd.enemy[Gd.enemy_num].info.move.x = 0;
            Gd.enemy[Gd.enemy_num].info.move.y = 0;
            Gd.enemy[Gd.enemy_num].info.move_flag = 0;
        }
    }

    private void Move(Vector3 targetPosition)
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
    }
}
