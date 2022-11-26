using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの攻撃（右のボタン）未実装
public class PlayerAttack : MonoBehaviour
{
    public GeneralData Gd;

    void Update()
    {
        PlayerAtk();
    }

    public void PlayerAtk()
    {
        //動きがない場合はパス
        if (Gd.player_status != Global.ATTACK) return;

        if (Gd.player_hp < 0)
        {
            //ゲームオーバー
            return;
        }
    }
}
