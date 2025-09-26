using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move", menuName ="Pokémon/Crear nuevo Move")]
public class MovimientoBase : ScriptableObject
{
    [SerializeField] string nombre;
    [TextArea]
    [SerializeField] string descripción;
    [SerializeField] PokemonType type;
    [SerializeField] int daño;
    [SerializeField] int precisión;
    [SerializeField] bool alwaysHits;
    [SerializeField] int pP;
    [SerializeField] int priority;

    [SerializeField] FisEspEst FisicoEspecialEstado;

    [SerializeField] MoveEffects effects;
    [SerializeField] List<SecondaryEffects> secondaries;
    [SerializeField] MoveTarget target;

    public FisEspEst fee
    {
        get { return FisicoEspecialEstado; }
    }

    public int feeint
    {
        get { return (int)FisicoEspecialEstado; }
    }

    public string Nombre { get { return nombre; } }
    public string Descripción { get { return descripción; } }
    public PokemonType Type { get { return type; } }
    public int Daño { get { return daño; } }
    public int Precisión { get { return precisión; } }
    public bool AlwaysHits { get { return alwaysHits; } }

    public int PP { get { return pP; } }
    public int Priority { get { return priority; } }

    public MoveEffects Effects { get { return effects; } }
    public List<SecondaryEffects> Secondaries { get { return secondaries; } }
    public MoveTarget Target { get { return target;} }
}

[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    [SerializeField] ConditionID status;
    [SerializeField] ConditionID volatileStatus;

    public List<StatBoost> Boosts { get { return boosts; } }

    public ConditionID Status { get { return status; } }
    public ConditionID VolatileStatus { get { return volatileStatus; } }

}

[System.Serializable]
public class SecondaryEffects : MoveEffects
{
    [SerializeField] int chance;
    [SerializeField] MoveTarget target; 

    public int Chance { get { return chance; } }
    public MoveTarget Target { get { return target; } }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}

public enum FisEspEst
{
    None,
    Fisico,
    Especial,
    Estado
}

public enum MoveTarget
{
    Foe, Self
}
