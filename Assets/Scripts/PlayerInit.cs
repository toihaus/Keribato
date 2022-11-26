using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの初期設定
public class PlayerInit : MonoBehaviour
{
    public GeneralData Gd;

    void Start()
    {
//        Gd.player_old_x = Gd.player_x;
//        Gd.player_old_y = Gd.player_y;

        Gd.player_dir = 2;

        Gd.player_level = 1;

        Gd.player_nextexp = Gd.NextExp(Gd.player_level);
        Gd.player_exp = 0;
        Gd.player_maxhp = 15;
        Gd.player_hp = 15;

        Gd.player_status = Global.STAY;

        Gd.move_flag = 0;
    }

}
