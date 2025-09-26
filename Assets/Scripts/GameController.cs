using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Menu,
    Battle,
}
public class GameController : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    [SerializeField] BattleSystem battleSystem;

    [SerializeField] GameObject cajaPokemons;

    GameState state;

    private void Awake()
    {
        ConditionsDB.Init();
    }

    private void Start()
    {
        mainMenu.WildEncounter += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    void StartBattle()
    {
        state= GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);

        var playerParty = cajaPokemons.GetComponent<PokemonParty>();
        var wildPokemon = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomWildPokemon();

        battleSystem.StartBattle(playerParty, wildPokemon);
    }

    void EndBattle(bool won)
    {
        state= GameState.Menu;
        battleSystem.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);

    }

    private void Update()
    {
        if (state == GameState.Menu)
        {

        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
    }
}
