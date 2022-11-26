using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public EnemyInfo info;

    public Enemy(int num,string name,int level, GameObject prefab,int kind,int dir,int flag)
    {
        info = new EnemyInfo();
        info.num = num;
        info.name = name;
        info.level = level;
        info.prefab = prefab;
        info.kind = kind;
        info.dir = dir;
        info.move_flag = flag;
    }
}
