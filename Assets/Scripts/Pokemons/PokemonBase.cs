using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokémon", menuName = "Pokémon/Crear nuevo Pokémon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string nombre;

    [TextArea]
    [SerializeField] string descripción;
    [SerializeField] public bool IsMale;
    [SerializeField] public bool IsShiny;

    [SerializeField] public Sprite frontSprite;
    [SerializeField] public Sprite backSprite;

    [SerializeField] public PokemonType type1;
    [SerializeField] public PokemonType type2;

    [SerializeField] PokemonNaturaleza naturaleza;

    public int natValue
    {
        get { return (int)naturaleza; }
    }


    [SerializeField] int maxHp;
    [SerializeField] int ataque;
    [SerializeField] int defensa;
    [SerializeField] int ataqueEspecial;
    [SerializeField] int defensaEspecial;
    [SerializeField] int velocidad;

    [SerializeField] int IvsHp;
    [SerializeField] int EvsHp;
    [SerializeField] int IvsAtaque;
    [SerializeField] int EvsAtaque;
    [SerializeField] int IvsDefensa;
    [SerializeField] int EvsDefensa;
    [SerializeField] int IvsAtaqueSp;
    [SerializeField] int EvsAtaqueSp;
    [SerializeField] int IvsDefensaSp;
    [SerializeField] int EvsDefensaSp;
    [SerializeField] int IvsVelocidad;
    [SerializeField] int EvsVelocidad;

    [SerializeField] List<MovimientosAprendibles> movimientosAprendibles;
    public string Nombre { get { return nombre; } }
    public string Descripción { get { return descripción; } }
    public int StatVida { get { return maxHp; } }
    public int StatAtaque { get { return ataque; } }
    public int StatDefensa { get { return defensa; } }
    public int StatAtaqueEspecial { get { return ataqueEspecial; } }
    public int StatDefensaEspecial { get { return defensaEspecial; } }
    public int StatVelocidad { get { return velocidad; } }
    public int IvStatVida { get { return IvsHp; } }
    public int IvStatAtaque { get { return IvsAtaque; } }
    public int IvStatDefensa { get { return IvsDefensa; } }
    public int IvStatAtaqueEspecial { get { return IvsAtaqueSp; } }
    public int IvStatDefensaEspecial { get { return IvsDefensaSp; } }
    public int IvStatVelocidad { get { return IvsVelocidad; } }
    public int EvStatVida { get { return EvsHp; } }
    public int EvStatAtaque { get { return EvsAtaque; } }
    public int EvStatDefensa { get { return EvsDefensa; } }
    public int EvStatAtaqueEspecial { get { return EvsAtaqueSp; } }
    public int EvStatDefensaEspecial { get { return EvsDefensaSp; } }
    public int EvStatVelocidad { get { return EvsVelocidad; } }

    public List<MovimientosAprendibles> LearneableMoves
    {
        get { return movimientosAprendibles; }
    }
}
[System.Serializable]
public class MovimientosAprendibles
{
    [SerializeField] MovimientoBase movimientoBase;
    [SerializeField] int level;

    public MovimientoBase Base
    {
        get { return movimientoBase; }
    }

    public int Level
    {
        get { return level; }
    }
}
public enum PokemonType
    {
        None,
        Acero,
        Agua,
        Bicho,
        Dragón,
        Eléctrico,
        Fantasma,
        Fuego,
        Hada,
        Hielo,
        Lucha,
        Normal,
        Planta,
        Psíquico,
        Roca,
        Siniestro,
        Tierra, 
        Veneno,
        Volador,
    }

public enum Stat
{
    Attack,
    Defense,
    SpAttack,
    SpDefense,
    Speed,

    // no son stats reales, se usan para mejorar el moveAccuracy
    Accuracy,
    Evasion
}

public class TypeChart
{
    static float[][] chart =
    {
        
        //                        {Acer,Agua,Bich,Drag,Eléc,Fant,Fueg,Hada,Hiel,Luch,Norm,Plan,Psíq,Roca,Sini,Tier,Vene,Vola,Astr}
        /*Acero*/     new float[] {0.5f,0.5f,  1f,  1f,0.5f,  1f,0.5f,  2f,  2f,  1f,  1f,  1f,  1f,  2f,  1f,  1f,  1f,  1f},
        /*Agua*/      new float[] {  1f,0.5f,  1f,0.5f,  1f,  1f,  2f,  1f,  1f,  1f,  1f,0.5f,  1f,  2f,  1f,  2f,  1f,  1f},
        /*Bicho*/     new float[] {0.5f,  1f,  1f,  1f,  1f,0.5f,0.5f,0.5f,  1f,0.5f,  1f,  2f,  2f,  1f,  2f,  1f,0.5f,0.5f},
        /*Dragón*/    new float[] {0.5f,  1f,  1f,  2f,  1f,  1f,  1f,  0f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f},
        /*Eléctrico*/ new float[] {  1f,  2f,  1f,0.5f,0.5f,  1f,  1f,  1f,  1f,  1f,  1f,0.5f,  1f,  1f,  1f,  0f,  1f,  2f},
        /*Fantasma*/  new float[] {  1f,  1f,  1f,  1f,  1f,  2f,  1f,  1f,  1f,  1f,  0f,  1f,  2f,  1f,0.5f,  1f,  1f,  1f},
        /*Fuego*/     new float[] {  2f,0.5f,  2f,0.5f,  1f,  1f,0.5f,  1f,  2f,  1f,  1f,  2f,  1f,0.5f,  1f,  1f,  1f,  1f},
        /*Hada*/      new float[] {0.5f,  1f,  1f,  2f,  1f,  1f,0.5f,  1f,  1f,  2f,  1f,  1f,  1f,  1f,  2f,  1f,0.5f,  1f},
        /*Hielo*/     new float[] {0.5f,0.5f,  1f,  2f,  1f,  1f,0.5f,  1f,0.5f,  1f,  1f,  2f,  1f,  1f,  1f,  2f,  1f,  2f},
        /*Lucha*/     new float[] {  2f,  1f,0.5f,  1f,  1f,  0f,  1f,0.5f,  2f,  1f,  2f,  1f,0.5f,  2f,  2f,  1f,0.5f,0.5f},
        /*Normal*/    new float[] {0.5f,  1f,  1f,  1f,  1f,  0f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,0.5f,  1f,  1f,  1f,  1f},
        /*Planta*/    new float[] {0.5f,  2f,0.5f,  1f,  1f,  1f,0.5f,  1f,  1f,  1f,  1f,0.5f,  1f,  2f,  1f,  2f,0.5f,0.5f},
        /*Psíquico*/  new float[] {0.5f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  1f,  2f,  1f,  1f,0.5f,  1f,  0f,  1f,  2f,  1f},
        /*Roca*/      new float[] {0.5f,  1f,  2f,  1f,  1f,  1f,  2f,  1f,  2f,0.5f,  1f,  1f,  1f,  1f,  1f,0.5f,  1f,  2f},
        /*Siniestro*/ new float[] {  1f,  1f,  1f,  1f,  1f,  2f,  1f,0.5f,  1f,0.5f,  1f,  1f,  2f,  1f,0.5f,  1f,  1f,  1f},
        /*Tierra*/    new float[] {  2f,  1f,0.5f,  1f,  2f,  1f,  2f,  1f,  1f,  1f,  1f,0.5f,  1f,  2f,  1f,  1f,  2f,  0f},
        /*Veneno*/    new float[] {  0f,  1f,  1f,  1f,  1f,0.5f,  1f,  2f,  1f,  1f,  1f,  2f,  1f,0.5f,  1f,0.5f,0.5f,  1f},
        /*Volador*/   new float[] {0.5f,  1f,  2f,  1f,0.5f,  1f,  1f,  1f,  1f,  2f,  1f,  2f,  1f,0.5f,  1f,  1f,  1f,  1f},

    };

    public static float GetEffectiveness(PokemonType attackType, PokemonType defenseType)
    {
        if (attackType==PokemonType.None || defenseType ==PokemonType.None) 
            return 1f;

        int row = (int)attackType-1;
        int col = (int)defenseType-1;


        return chart[row][col];
    }

}

public enum PokemonNaturaleza
{
    /* 0 */ None, // N/A
    /* 1 */ Activa, // +vel-def
    /* 2 */ Afable, // +spatk-def
    /* 3 */ Agitada, // +def-spatk
    /* 4 */ Alegre, // +vel-spatk
    /* 5 */ Alocada, // +spatk-spdef
    /* 6 */ Amable, // +spdef-def
    /* 7 */ Audaz, // +atk-vel
    /* 8 */ Cauta, // +spdef-spatk
    /* 9 */ Dócil, // neutro
    /* 10 */ Firme, // +atk-spatk
    /* 11 */ Floja, // +def-spdef
    /* 12 */ Fuerte, // neutro
    /* 13 */ Grosera, // +spdef-vel
    /* 14 */ Huraña, // +atk-def
    /* 15 */ Ingenua, // +vel-spdef
    /* 16 */ Mansa, // +spatk-vel
    /* 17 */ Miedosa, // +vel-atk
    /* 18 */ Modesta, // +spatk-atk
    /* 19 */ Osada, // +def-atk
    /* 20 */ Plácida, // +def-vel
    /* 21 */ Pícara, // +atk-spdef
    /* 22 */ Rara, // neutro
    /* 23 */ Serena, // +spdef-atk
    /* 24 */ Seria, // neutro
    /* 25 */ Tímida, // neutro
}

