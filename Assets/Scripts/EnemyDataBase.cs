using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyDataBase : ScriptableObject
{

    [SerializeField]
    private List<EnemyData> EnemyDataLists = new List<EnemyData>();

    //敵のデータリストを返す
    public List<EnemyData> GetEnemyDataLists()
    {
        return EnemyDataLists;
    }
}
