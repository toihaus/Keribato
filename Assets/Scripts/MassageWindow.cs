using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//メッセージウィンドウ内に文字表示
public class MassageWindow : MonoBehaviour
{
    public GeneralData Gd;

    public EnemyDataBase Edb;
    EnemyData Ed;

    public Text massage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayerEndMessage(string enemy_name,int exp,int damage)
    {
        massage.text = enemy_name + "を倒した\n" + "EXP" + exp.ToString() + "を獲得\n" + "<color=#FF0000>ダメージ" + damage.ToString() + "を受けた</color>\n";

        massage.text += "<color=#00FFFF>ニケは力尽きた・・・</color>";
    }

    public void EnemyEndMessage(string enemy_name, int exp, int damage, bool lv)
    {
        massage.text = enemy_name + "を倒した\n" + "EXP" + exp.ToString() + "を獲得\n" + "<color=#FF0000>ダメージ" + damage.ToString() + "を受けた</color>\n";

        if (lv)
        {
            massage.text += "<color=#00FFFF>レベルが" + Gd.player_level.ToString() + "に上がった</color>";
        }
    }

    public void EnemyFusionMessage(string enemy_name,int lv)
    {
        massage.text = enemy_name + "は合体した\n" + "<color=#00FFFF>" + enemy_name + "のレベルが" + lv + "に上がってしまった</color>";
    }

    public void ResetMessage()
    {
        massage.text = "";
    }

}
