using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G�Ɋւ���R�[�h�S��
public class EnemyControl : MonoBehaviour
{
    public GeneralData Gd;

    void Start()
    {
        
    }


    void Update()
    {
        //�G�̉�]
        if (Gd.player_status == Global.ROTATE)
        {
            //�G�L�����̉�]
//            EnemyRotate();
        }

    }


}
