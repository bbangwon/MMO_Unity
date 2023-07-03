using UnityEngine;

public class PlayerStat : Stat
{
    [SerializeField]
    protected int _exp;

    [SerializeField]
    protected int _gold;

    public int Exp { 
        get => _exp; 
        set 
        {
            _exp = value;

            int level = Level;
            while (true)
            {
                if (Managers.Data.StatDict.TryGetValue(level + 1, out var stat) == false)
                    break;
                if(_exp < stat.totalExp) 
                    break;
                level++;
            }

            if(level != Level)
            {
                Debug.Log("Level up");
                Level = level;
                SetStat();
            }
        }
    }
    public int Gold { get => _gold; set => _gold = value; }

    private void Start()
    {
        _level = 1;
        _defense = 5;
        _moveSpeed = 5.0f;
        _gold = 0;
        _exp = 0;

        SetStat();
    }

    public void SetStat()
    {
        var dict = Managers.Data.StatDict;
        var stat = dict[_level];

        _hp = stat.maxHp;
        _maxHp = stat.maxHp;
        _attack = stat.attack;
    }

    protected override void OnDead(Stat attacker)
    {
        Debug.Log("Player Dead");
    }
}
