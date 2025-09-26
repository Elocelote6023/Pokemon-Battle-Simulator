using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move
{
    public MovimientoBase Base { get; set; }
    public int PP { get; set; }
    public int Damage { get; set; }
    public int Precision { get; set; }
    public int Feen { get; set; }

    public Move(MovimientoBase @base)
    {
        Base = @base;
        PP = @base.PP;
        Damage = @base.Daño;
        Precision = @base.Precisión;
        Feen = @base.feeint;
    }
}
