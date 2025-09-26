using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;

[System.Serializable]

public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase Base 
    {
        get
        {
            return _base;
        }
    }
    public int Level
    {
        get
        {
            return level;
        }
    }
    public int HP { get; set; }
    public List<Move> Moves { get; set; }
    public Move CurrentMove { get; set; }
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoosts { get; private set; }
    public Condition Status { get; private set; }
    public int StatusTime { get; set; }

    public Condition VolatileStatus { get; private set; }
    public int VolatileStatusTime { get; set; }



    public Queue<string> StatusChanges { get; private set; } = new Queue<string>();

    public bool HpChanged { get; set; }

    public event System.Action OnStatusChanged;

    public void Init()
    {
        Moves= new List<Move>();
        foreach (var move in Base.LearneableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }

            if (Moves.Count >= 4)
            {
                break;
            }
        }
        CalculateStats();
        HP = VidaFinal;

        ResetStatBoosts();
        VolatileStatus= null;
    }

    public int NaturalezaNum
    {
        get
        {
            return Base.natValue;
        }
    }

    void CalculateStats()
    {
        float NatCalcAtk = 0;
        float NatCalcDef = 0;
        float NatCalcSpAtk = 0;
        float NatCalcSpDef = 0;
        float NatCalcVel = 0;

        if (NaturalezaNum == 0) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1f; Debug.Log("el pkmn no tiene naturaleza bro"); }
        else if (NaturalezaNum == 1) { NatCalcAtk = 1f; NatCalcDef = 0.9f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1.1f; }
        else if (NaturalezaNum == 2) { NatCalcAtk = 1f; NatCalcDef = 0.9f; NatCalcSpAtk = 1.1f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 3) { NatCalcAtk = 1f; NatCalcDef = 1.1f; NatCalcSpAtk = 0.9f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 4) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 0.9f; NatCalcSpDef = 1f; NatCalcVel = 1.1f; }
        else if (NaturalezaNum == 5) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1.1f; NatCalcSpDef = 0.9f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 6) { NatCalcAtk = 1f; NatCalcDef = 0.9f; NatCalcSpAtk = 1f; NatCalcSpDef = 1.1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 7) { NatCalcAtk = 1.1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 0.9f; }
        else if (NaturalezaNum == 8) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 0.9f; NatCalcSpDef = 1.1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 9) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 10) { NatCalcAtk = 1.1f; NatCalcDef = 1f; NatCalcSpAtk = 0.9f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 11) { NatCalcAtk = 1f; NatCalcDef = 1.1f; NatCalcSpAtk = 1f; NatCalcSpDef = 0.9f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 12) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 13) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1.1f; NatCalcVel = 0.9f; }
        else if (NaturalezaNum == 14) { NatCalcAtk = 1.1f; NatCalcDef = 0.9f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 15) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 0.9f; NatCalcVel = 1.1f; }
        else if (NaturalezaNum == 16) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1.1f; NatCalcSpDef = 1f; NatCalcVel = 0.9f; }
        else if (NaturalezaNum == 17) { NatCalcAtk = 0.9f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1.1f; }
        else if (NaturalezaNum == 18) { NatCalcAtk = 0.9f; NatCalcDef = 1f; NatCalcSpAtk = 1.1f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 19) { NatCalcAtk = 0.9f; NatCalcDef = 1.1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 20) { NatCalcAtk = 1f; NatCalcDef = 1.1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 0.9f; }
        else if (NaturalezaNum == 21) { NatCalcAtk = 1.1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 0.9f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 22) { NatCalcAtk = 0.9f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 23) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1.1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 24) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1f; }
        else if (NaturalezaNum == 25) { NatCalcAtk = 1f; NatCalcDef = 1f; NatCalcSpAtk = 1f; NatCalcSpDef = 1f; NatCalcVel = 1f; }

        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, Mathf.FloorToInt(((((2 * Base.StatAtaque + Base.IvStatAtaque + (Base.EvStatAtaque / 4)) * Level) / 100) + 5) * NatCalcAtk));
        Stats.Add(Stat.Defense, Mathf.FloorToInt(((((2 * Base.StatDefensa + Base.IvStatDefensa + (Base.EvStatDefensa / 4)) * Level) / 100) + 5) * NatCalcDef));
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt(((((2 * Base.StatAtaqueEspecial + Base.IvStatAtaqueEspecial + (Base.EvStatAtaqueEspecial / 4)) * Level) / 100) + 5) * NatCalcSpAtk));
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt(((((2 * Base.StatDefensaEspecial + Base.IvStatDefensaEspecial + (Base.EvStatDefensaEspecial / 4)) * Level) / 100) + 5) * NatCalcSpDef));
        Stats.Add(Stat.Speed, Mathf.FloorToInt(((((2 * Base.StatVelocidad + Base.IvStatVelocidad + (Base.EvStatVelocidad / 4)) * Level) / 100) + 5) * NatCalcVel));
    
        VidaFinal = Mathf.FloorToInt((((2 * Base.StatVida + Base.IvStatVida + (Base.EvStatVida / 4)) * Level) / 100) + Level + 10);
    }

    void ResetStatBoosts()
    {
        StatBoosts = new Dictionary<Stat, int>()
        {
            {Stat.Attack, 0},
            {Stat.Defense, 0},
            {Stat.SpAttack, 0},
            {Stat.SpDefense, 0},
            {Stat.Speed, 0},
            {Stat.Accuracy, 0},
            {Stat.Evasion, 0}
        };
    }

    int GetStat(Stat stat)
    {
        int statVal = Stats[stat];

        // apply stat boost
        int boost = StatBoosts[stat];
        var boostValues = new float[] { 2f / 2f, 3f / 2f, 4f / 2f, 5f / 2f, 6f / 2f, 7f / 2f, 8f / 2f };

        if (boost >= 0)
        {
            statVal= Mathf.FloorToInt(statVal * boostValues[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal / boostValues[-boost]);
        }

        return statVal;
    }

    public void ApplyBoost(List<StatBoost> statBoosts)
    {
        foreach (var statBoost in statBoosts)
        {
            var stat = statBoost.stat;
            var boost = statBoost.boost;

            StatBoosts[stat] = Mathf.Clamp(StatBoosts[stat] + boost, -6, 6);

            if ((int)stat == 0)
            {
                if (boost > 0)
                {
                    StatusChanges.Enqueue($"¡El Ataque de {Base.name} ha aumentado!");
                }
                else
                {
                    StatusChanges.Enqueue($"¡El Ataque de {Base.name} ha disminuido!");
                }
            }
            else if ((int)stat == 1)
            {
                if (boost > 0)
                {
                    StatusChanges.Enqueue($"¡La Defensa de {Base.name} ha aumentado!");
                }
                else
                {
                    StatusChanges.Enqueue($"¡La Defensa de {Base.name} ha disminuido!");
                }
            }
            else if ((int)stat == 2)
            {
                if (boost > 0)
                {
                    StatusChanges.Enqueue($"¡El Ataque Especial de {Base.name} ha aumentado!");
                }
                else
                {
                    StatusChanges.Enqueue($"¡El Ataque Especial de {Base.name} ha disminuido!");
                }
            }
            else if ((int)stat == 3)
            {
                if (boost > 0)
                {
                    StatusChanges.Enqueue($"¡La Defensa Especial de {Base.name} ha aumentado!");
                }
                else
                {
                    StatusChanges.Enqueue($"¡La Defensa Especial de {Base.name} ha disminuido!");
                }
            }
            else if ((int)stat == 4)
            {
                if (boost > 0)
                {
                    StatusChanges.Enqueue($"¡La Velocidad de {Base.name} ha aumentado!");
                }
                else
                {
                    StatusChanges.Enqueue($"¡La Velocidad de {Base.name} ha disminuido!");
                }
            }

        }
    }


    public int VidaFinal { get; private set; }

    public int AtaqueFinal
    {
        get
        {
            return GetStat(Stat.Attack);
        }
    }

    public int DefensaFinal
    {
        get
        {
            return GetStat(Stat.Defense);
        }
    }

    public int AtaqueEspecialFinal
    {
        get
        {
            return GetStat(Stat.SpAttack);
        }
    }

    public int DefensaEspecialFinal
    {
        get
        {
            return GetStat(Stat.SpDefense);
        }
    }

    public int VelocidadFinal
    {
        get
        {
            return GetStat(Stat.Speed);
        }
    }

    public DamageDetails TakeDamage(Move move, Pokemon attaker)
    {
        float critical = 1f;
        float randomcrit = UnityEngine.Random.Range(0f, 1f);
        //Debug.Log(randomcrit + " " + "-" + " " + 1f / 24f);

        if (randomcrit <= 1f/24f)
        {
            critical = 1.5f;
        }

        float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.type1) * TypeChart.GetEffectiveness(move.Base.Type, this.Base.type2);

        var damageDetails = new DamageDetails()
        {
            TypeEffectiveness = type,
            Critical = critical,
            Fainted = false,
        };

        float STAB =1f;
        if (move.Base.Type == attaker.Base.type1 || move.Base.Type == attaker.Base.type2)
        {
            STAB = 1.5f;
        }


        float aleatorio = UnityEngine.Random.Range(0.85f, 1f);
        float Modifiers = aleatorio * type * critical * STAB;

        float Ataque = (move.Base.fee == FisEspEst.Especial) ? attaker.AtaqueEspecialFinal : attaker.AtaqueFinal ;
        float Defensa = (move.Base.fee == FisEspEst.Especial) ? DefensaEspecialFinal : DefensaFinal; ;

        float a = (((2 * attaker.Level) / 5) + 2);
        float coredmg = ((a * move.Base.Daño * (Ataque / Defensa)) / 50) + 2;
        int damage = Mathf.FloorToInt(coredmg * Modifiers);

        UpdateHP(damage);

        return damageDetails;
    }

    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, VidaFinal);
        HpChanged = true;
    }

    public void SetStatus(ConditionID conditionId, bool isEnemyUnit)
    {
        if (Status != null) return;

        Status = ConditionsDB.Conditions[conditionId];
        Status?.OnStart?.Invoke(this);

        if (!isEnemyUnit) 
        {
            StatusChanges.Enqueue($"¡{Base.name} {Status.StartMessage}");
        }
        if (isEnemyUnit)
        {
            StatusChanges.Enqueue($"¡El {Base.name} enemigo {Status.StartMessage}");
        }

        OnStatusChanged?.Invoke();
    }
    public void CureStatus()
    {
        Status = null;
        OnStatusChanged?.Invoke();
    }

    public void SetVolatileStatus(ConditionID conditionId, bool isEnemyUnit)
    {
        if (VolatileStatus != null) return;

        VolatileStatus = ConditionsDB.Conditions[conditionId];
        VolatileStatus?.OnStart?.Invoke(this);

        if (!isEnemyUnit)
        {
            StatusChanges.Enqueue($"¡{Base.name} {VolatileStatus.StartMessage}");
        }
        if (isEnemyUnit)
        {
            StatusChanges.Enqueue($"¡El {Base.name} enemigo {VolatileStatus.StartMessage}");
        }
    }
    public void CureVolatileStatus()
    {
        VolatileStatus = null;
    }


    public Move GetRandomMove()
    {
        int r= UnityEngine.Random.Range(1, Moves.Count);
        return Moves[r];
    }

    public bool OnBeforeTurn()
    {
        bool canPerformMove = true;

        if (Status?.OnBeforeMove != null)
        {
            if(!Status.OnBeforeMove(this))
            {
                canPerformMove = false;
            }
        }

        if (VolatileStatus?.OnBeforeMove != null)
        {
            if (!VolatileStatus.OnBeforeMove(this))
            {
                canPerformMove = false;
            }
        }

        return canPerformMove;
    }

    public void OnAfterTurn()
    {
        Status?.OnAfterTurn?.Invoke(this);
        VolatileStatus?.OnAfterTurn?.Invoke(this);
    }

    public void OnBattleOver()
    {
        VolatileStatus = null;
        ResetStatBoosts();
    }

}

public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}
