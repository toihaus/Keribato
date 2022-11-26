using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//ゲームを制御する部分
public class GameControl : MonoBehaviour
{
    public GeneralData Gd;

    int old_turn;

    public Text now_turn;

    public float fadeSpeed = 0.005f;        //透明度が変わるスピードを管理
    float red, green, blue, alfa;   //パネルの色、不透明度を管理

    public Image fadeImage;                //透明度を変更するパネルのイメージ

    void Start()
    {
        old_turn = 0;
        Gd.now_turn = 1;

        red = fadeImage.color.r;
        green = fadeImage.color.g;
        blue = fadeImage.color.b;
        alfa = fadeImage.color.a;
        fadeImage.enabled = false;
    }


    void Update()
    {
        if(Gd.player_status == Global.END)
        {
            //ゲームオーバー処理
            fadeImage.enabled = true;
            StartFadeOut();
        }
        //動きがない場合はパス
        if (old_turn >= Gd.now_turn) return;

        old_turn = Gd.now_turn;

        //ターン数を表示
        now_turn.text = old_turn.ToString();

    }

    void StartFadeOut()
    {
        alfa += fadeSpeed;         // b)不透明度を徐々にあげる
        SetAlpha();
        if (alfa >= 1.55)
        {
            SceneManager.LoadScene("ResultScene");
        }

    }

    void SetAlpha()
    {
        fadeImage.color = new Color(red, green, blue, alfa);
    }


}
