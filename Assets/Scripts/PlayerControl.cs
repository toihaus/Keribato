using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Nike�Ɋւ���R�[�h�S��
public class PlayerControl : MonoBehaviour
{
    public GeneralData Gd;

    public Animator nike_anime= null;

    public Text now_level;
    public Text player_level;

    public Text now_exp;
    public Image exp_gauge;

    public Text now_hp;
    public Text now_maxhp;
    public Image hp_gauge;


    GameObject massage_window;

    int nike_atk_flag;

    void Start()
    {
        PlaterInit();

        //���b�Z�[�W�E�B���h�E��T��
        massage_window = GameObject.Find("Window");
        massage_window.SetActive(false);

        Debug.Log("player init" + Gd.player_maxhp + ":" + Gd.player_dir);

    }

    void Update()
    {
        PlayerStatus();

        //�}�b�v�ړ����ȊO�̎��̂ݓ��͂��󂯕t����
        if (Gd.move_flag != 1)
        {

            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            // �L�[�������ꂽ���i�����j
            if (x == 1 || y == 1 || x == -1 || y == -1)
            {
                NikeChangeDir(x, y);
            }
            else if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire3"))) //�U��
            {
                massage_window.SetActive(false);
                NikeAttack();
            }
        }
    }

    //�v���C���[�̏�����
    void PlaterInit()
    {
        Gd.player_dir = 2;

        Gd.player_level = 1;

        Gd.player_nextexp = Gd.NextExp(Gd.player_level);
        Gd.player_exp = 0;
        Gd.player_maxhp = 15;
        Gd.player_hp = 15;

        Gd.player_status = Global.STAY;

        Gd.move_flag = 0;

    }

    //�v���C���[�̃X�e�[�^�X�\��
    void PlayerStatus()
    {
        exp_gauge.fillAmount = (float)(Gd.player_exp) / (float)(Gd.player_nextexp);
        hp_gauge.fillAmount = (float)(Gd.player_hp) / (float)(Gd.player_maxhp);

        float next_exp = Gd.player_nextexp - Gd.player_exp;

        //���x����\��
        now_level.text = Gd.player_level.ToString();

        //�A�C�R����̃��x����\��
        player_level.text = Gd.player_level.ToString();

        //�K�v�Ȍo���l��\��
        now_exp.text = next_exp.ToString();
        //        now_exp.text = Gd.player_exp.ToString();

        //�̗͂�\��
        now_hp.text = Gd.player_hp.ToString();

        //�ő�̗͂�\��
        now_maxhp.text = Gd.player_maxhp.ToString();

    }

    //�j�P�̕����]��
    void NikeChangeDir(float x, float y)
    {
        int nx = 0;
        int ny = 0;

        //��������
        if (x < 0) // ��
        {
            Gd.player_dir = 4;
            nx = -1;
            PlayerMoveCheck(nx, ny);
            PushLeft();
        }
        else if (x > 0) // �E
        {
            Gd.player_dir = 6;
            nx = 1;
            PlayerMoveCheck(nx, ny);
            PushRight();
        }
        else if (y > 0) // ��
        {
            Gd.player_dir = 8;
            ny = -1;
            PlayerMoveCheck(nx, ny);
            PushUp();
        }
        else // ��
        {
            Gd.player_dir = 2;
            ny = 1;
            PlayerMoveCheck(nx, ny);
            PushDown();
        }

    }

    //�ړ��\���`�F�b�N����̋����ύX
    void PlayerMoveCheck(int nx,int ny)
    {
        //�ړ��悪�ړ��\���`�F�b�N
        if (Gd.NextMoveCheck(Gd.player_x + nx, Gd.player_y + ny))
        {
            int num = Gd.map_enemy_num[Gd.player_x + nx, Gd.player_y + ny];
            int enemy_dir = Gd.enemy[num].info.dir;

            Debug.Log("�v���C���[num:"+num+":lv:"+Gd.enemy[num].info.level + ":" + Gd.player_x +"+"+ nx +":"+Gd.player_y +"+"+ ny);

            if (enemy_dir == Gd.player_dir)
            {
                //�j�P���L�b�N���
                nike_atk_flag = 1;
            }
            else
            {
                nike_atk_flag = 0;
            }
        }
    }

    //�j�P�̃A�j���[�V�����؂�ւ������B�v���P�P
    public void PushUp()
    {
        ResetDir();
        if (nike_atk_flag == 0)
        {
            nike_anime.SetBool("Up", true);
        }
        else
        {
            nike_anime.SetBool("UpKick", true);
        }
    }

    public void PushDown()
    {
        ResetDir();
        if (nike_atk_flag == 0)
        {
            nike_anime.SetBool("Down", true);
        }
        else
        {
            nike_anime.SetBool("DownKick", true);
        }
    }

    public void PushLeft()
    {
        ResetDir();
        if (nike_atk_flag == 0)
        {
            nike_anime.SetBool("Left", true);
        }
        else
        {
            nike_anime.SetBool("LeftKick", true);
        }
    }

    public void PushRight()
    {
        ResetDir();
        if (nike_atk_flag == 0)
        {
            nike_anime.SetBool("Right", true);
        }
        else
        {
            nike_anime.SetBool("RightKick", true);
        }
    }

    void ResetDir()
    {
        nike_anime.SetBool("Up", false);
        nike_anime.SetBool("Down", false);
        nike_anime.SetBool("Left", false);
        nike_anime.SetBool("Right", false);
        nike_anime.SetBool("UpKick", false);
        nike_anime.SetBool("DownKick", false);
        nike_anime.SetBool("LeftKick", false);
        nike_anime.SetBool("RightKick", false);
    }
    //�v���P�P�����܂�

    //�j�P���ʍU��
    void NikeAttack()
    {
        int nx = 0;
        int ny = 0;

        //�j�P�̌���
        if (Gd.player_dir == 4) // ��
        {
            nx = -1;
            PlayerAttackCheck(nx, ny);
        }
        else if (Gd.player_dir == 6) // �E
        {
            nx = 1;
            PlayerAttackCheck(nx, ny);
        }
        else if (Gd.player_dir == 8) // ��
        {
            ny = -1;
            PlayerAttackCheck(nx, ny);
        }
        else // ��
        {
            ny = 1;
            PlayerAttackCheck(nx, ny);
        }

    }

    //�ړ��\���̃`�F�b�N����̍U���F�v���P�Q
    void PlayerAttackCheck(int nx, int ny)
    {
        //�ړ��悪�ړ��\���`�F�b�N
        if (Gd.NextMoveCheck(Gd.player_x + nx, Gd.player_y + ny))
        {
            int num = Gd.map_enemy_num[Gd.player_x + nx, Gd.player_y + ny];
            int enemy_dir = Gd.enemy[num].info.dir;

            Gd.move_flag = 0;

            if (enemy_dir == Gd.player_dir)
            {
                //�L�b�N
                Gd.backattack_enemy_num = num;
                Gd.player_status = Global.KICK;

                massage_window.SetActive(true);
                GameObject.Find("Canvas").GetComponent<MassageWindow>().ResetMessage();
            }
            else
            {
                //�����
                if (Gd.player_dir == 8)
                {
                    Gd.move.x = 0;
                    Gd.move.y = -1;
                }
                //������
                else if (Gd.player_dir == 2)
                {
                    Gd.move.x = 0;
                    Gd.move.y = 1;
                }
                //������
                else if (Gd.player_dir == 4)
                {
                    Gd.move.x = 1;
                    Gd.move.y = 0;
                }
                //�E����
                else if (Gd.player_dir == 6)
                {
                    Gd.move.x = -1;
                    Gd.move.y = 0;
                }
                else
                {
                    Gd.move.x = 0;
                    Gd.move.y = 0;
                }

                //���ݒn��ۊ�
                Gd.player_old_x = Gd.player_x;
                Gd.player_old_y = Gd.player_y;

                //�v���C���[�ʒu�ɔ��f
                Gd.player_x -= (int)Gd.move.x;
                Gd.player_y += (int)Gd.move.y;

                massage_window.SetActive(true);
                GameObject.Find("Canvas").GetComponent<MassageWindow>().ResetMessage();

            }
        }
    }


}
