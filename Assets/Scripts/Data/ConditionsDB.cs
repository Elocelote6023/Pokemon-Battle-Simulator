using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDB
{
    public static void Init()
    {
        foreach (var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.ID= conditionId;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        {
            ConditionID.psn,
            new Condition()
            {
                Name = "Veneno",
                StartMessage = "ha sido Envenenado",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.HP / 8);
                    pokemon.StatusChanges.Enqueue($"¡El veneno resta PS a {pokemon.Base.name}!");
                }
            }
        },
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Quemadura",
                StartMessage = "se ha quemado!",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.HP / 16);
                    pokemon.StatusChanges.Enqueue($"¡{pokemon.Base.name} se resiente de las quemaduras!");
                }
            }
        },
        {
            ConditionID.par,
            new Condition()
            {
                Name = "Paralizado",
                StartMessage = "sufre parálisis!\nQuizás no se pueda mover.",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (Random.Range( 1, 5) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"¡{pokemon.Base.name} se ha paralizado!\nNo se puede mover.");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        },
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Congelado",
                StartMessage = "se ha congelado!",
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (Random.Range( 1, 6) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"¡{pokemon.Base.name} ya no está congelado!");
                        return true;
                    }
                    else
                    {
                        pokemon.StatusChanges.Enqueue($"¡{pokemon.Base.name} está congelado y no se puede mover!");
                        return false;
                    }

                }
            }
        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name = "Dormido",
                StartMessage = "se ha dormido!",
                OnStart = (Pokemon pokemon) =>
                {
                    //sleep for 1-3 turns
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log("se dormirá por " + pokemon.StatusTime+ " turnos");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"¡{pokemon.Base.name} se ha despertado!");
                        return true;
                    }

                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"¡{pokemon.Base.name} está profundamente dormido!");
                    return false;
                }
            }
        },

        // Volatile Status conditions
        {
            ConditionID.confusion,
            new Condition()
            {
                Name = "Confuso",
                StartMessage = "está confuso!",
                OnStart = (Pokemon pokemon) =>
                {
                    //sleep for 1-4 turns
                    pokemon.VolatileStatusTime = Random.Range(1,5);
                    Debug.Log("se confundirá por " + pokemon.VolatileStatusTime+ " turnos");
                },
                OnBeforeMove = (Pokemon pokemon) =>
                {
                    if (pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"¡{pokemon.Base.name} ya no está confuso!");
                        return true;
                    }
                    pokemon.VolatileStatusTime--;
                    pokemon.StatusChanges.Enqueue($"¡{pokemon.Base.name} está confuso!");

                    //perform a move 66.66% (2/3)
                    if (Random.Range(0f,1f) == 2f/3f)
                    {
                        return true;
                    }
                    else //33,33%. to hit itself (1/3)
                    {
                        pokemon.UpdateHP((((((2*pokemon.Level)/5)+2)*40*(pokemon.AtaqueFinal/pokemon.DefensaFinal))/50)+2);
                        pokemon.StatusChanges.Enqueue($"¡{pokemon.Base.name} está tan confuso que se golpeó a si mismo!");
                        return false;
                    }
                }
            }
        }
    };
}

public enum ConditionID
{
    none,psn,brn,slp,par,frz,psn2,
    confusion
}
