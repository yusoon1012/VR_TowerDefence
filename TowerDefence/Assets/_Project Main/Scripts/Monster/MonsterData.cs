using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MonsterData : MonoBehaviour
{
    #region 몬스터 속성 값
    public int hp = default;                 // 몬스터 체력
    public int power = default;              // 자폭 데미지
    public int speed = default;              // 몬스터 속도
    public int recognitionRange = default;   // 인식 범위
    public int attackRange = default;        // 공격 가능 범위
    public int explosionRange = default;     // 폭발 범위
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

    #region FastMonster
    public virtual (int, int, int, int, int, int) NormalUpgradeMonster(int _hp, int _power, int _speed, int _recognitionRange, int _attackRange, int _explosionRange)
    {
        int.TryParse(GameManager.instance.monsterData["Hp"][2], out _hp);
        int.TryParse(GameManager.instance.monsterData["Power"][2], out _power);
        int.TryParse(GameManager.instance.monsterData["Speed"][2], out _speed);
        int.TryParse(GameManager.instance.monsterData["Recognition_Range"][2], out _recognitionRange);
        int.TryParse(GameManager.instance.monsterData["Attack_Range"][2], out _attackRange);
        int.TryParse(GameManager.instance.monsterData["Explosion_Range"][2], out _explosionRange);

        return (_hp, _power, _speed, _recognitionRange, _attackRange, _explosionRange);
    }
    #endregion

    #region FastMonster
    public virtual (int, int, int, int, int, int) FastUpgradeMonster(int _hp, int _power, int _speed, int _recognitionRange, int _attackRange, int _explosionRange)
    {
        int.TryParse(GameManager.instance.monsterData["Hp"][3], out _hp);
        int.TryParse(GameManager.instance.monsterData["Power"][3], out _power);
        int.TryParse(GameManager.instance.monsterData["Speed"][3], out _speed);
        int.TryParse(GameManager.instance.monsterData["Recognition_Range"][3], out _recognitionRange);
        int.TryParse(GameManager.instance.monsterData["Attack_Range"][3], out _attackRange);
        int.TryParse(GameManager.instance.monsterData["Explosion_Range"][3], out _explosionRange);

        return (_hp, _power, _speed, _recognitionRange, _attackRange, _explosionRange);
    }
    #endregion
}
