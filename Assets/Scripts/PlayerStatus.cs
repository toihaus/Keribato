using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//プレイヤーの情報関係
public class PlayerStatus : MonoBehaviour
{
    public GeneralData Gd;

    public Text now_level;
    public Text player_level;

    public Text now_exp;
    public Image exp_gauge;

    public Text now_hp;
    public Text now_maxhp;
    public Image hp_gauge;


    void Update()
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

}
