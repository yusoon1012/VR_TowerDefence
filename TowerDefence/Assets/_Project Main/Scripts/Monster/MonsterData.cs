using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MonsterData : MonoBehaviour
{
    #region ���� �Ӽ� ��
    public int hp = default;                 // ���� ü��
    public int power = default;              // ���� ������
    public int speed = default;              // ���� �ӵ�
    public int recognitionRange = default;   // �ν� ����
    public int attackRange = default;        // ���� ���� ����
    public int explosionRange = default;     // ���� ����
    #endregion

    #region NormalMonster
    public virtual (int, int, int, int, int, int) NormalMonster(int _hp, int _power, int _speed, int _recognitionRange, int _attackRange, int _explosionRange)
    {
        int.TryParse(GameManager.instance.monsterData["Hp"][0], out _hp);
        int.TryParse(GameManager.instance.monsterData["Power"][0], out _power);
        int.TryParse(GameManager.instance.monsterData["Speed"][0], out _speed);
        int.TryParse(GameManager.instance.monsterData["Recognition_Range"][0], out _recognitionRange);
        int.TryParse(GameManager.instance.monsterData["Attack_Range"][0], out _attackRange);
        int.TryParse(GameManager.instance.monsterData["Explosion_Range"][0], out _explosionRange);

        return (_hp, _power, _speed, _recognitionRange, _attackRange, _explosionRange);
    }
    #endregion

    #region FastMonster
    public virtual (int, int, int, int, int, int) FastMonster(int _hp, int _power, int _speed, int _recognitionRange, int _attackRange, int _explosionRange)
    {
        int.TryParse(GameManager.instance.monsterData["Hp"][1], out _hp);
        int.TryParse(GameManager.instance.monsterData["Power"][1], out _power);
        int.TryParse(GameManager.instance.monsterData["Speed"][1], out _speed);
        int.TryParse(GameManager.instance.monsterData["Recognition_Range"][1], out _recognitionRange);
        int.TryParse(GameManager.instance.monsterData["Attack_Range"][1], out _attackRange);
        int.TryParse(GameManager.instance.monsterData["Explosion_Range"][1], out _explosionRange);

        return (_hp, _power, _speed, _recognitionRange, _attackRange, _explosionRange);
    }
    #endregion
}
