using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo
{
    public int num;

    //�G�L�����̖��O
    public string name;

    //�G�L�����̃��x��
    public int level;

    //�G�L������Prefab
    public GameObject prefab;

    //�G�L�����̎��
    public int kind;

    //�G�L�����̌���
    public int dir;

    public Vector2 move;
    public int move_flag;

    public int x;
    public int y;

    public int rotate_flag;
}
