using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BattleState { Start, ActionSelection, MoveSelection, RunningTurn, Busy, PartyScreen, BattleOver }
public enum BattleAction { Move, SwitchPokemon, UseItem, Run}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;  
    [SerializeField] BattleDialogueBox dialogBox;
    [SerializeField] PartyScreen partyScreen;

    [SerializeField] private GameObject malesymbolnopertenece;
    [SerializeField] private GameObject femalesymbolnopertenece;
    [SerializeField] private GameObject malesymbolnopertenece2;
    [SerializeField] private GameObject femalesymbolnopertenece2;

    [SerializeField] private GameObject alltypeiconopertenece;
    [SerializeField] private GameObject alltypeiconopertenece2;

    public event Action<bool> OnBattleOver;

    BattleState state;
    BattleState? prevState;
    int currentAction;
    int currentMove;
    int currentMember;

    [SerializeField] PokemonParty playerParty;
    Pokemon wildPokemon;

    public void StartBattle(PokemonParty playerParty, Pokemon wildPokemon)
    {
        this.playerParty = playerParty;
        this.wildPokemon = wildPokemon;

        StartCoroutine(SetupBattle());
        dialogBox.enableMoveSelector(false);
    }
    public IEnumerator SetupBattle()
    {
        playerUnit.Setup(playerParty.GetHealthyPokemon());
        enemyUnit.Setup(wildPokemon);

        partyScreen.Init();

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return StartCoroutine(dialogBox.TypeDialog($"Un {enemyUnit.Pokemon.Base.name} enemigo ha aparecido"));

        ActionSelection();
    }

    void BattleOver(bool won)
    {
        state= BattleState.BattleOver;
        playerParty.Pokemons.ForEach(p => p.OnBattleOver());
        OnBattleOver(won);
    }

    void ActionSelection()
    {
        state = BattleState.ActionSelection;
        StartCoroutine(dialogBox.TypeDialog("Elige una acción"));
        dialogBox.enableActionSelector(true);
    }

    void OpenPartyScreen()
    {
        state = BattleState.PartyScreen;
        partyScreen.SetPartyData(playerParty.Pokemons);
        partyScreen.gameObject.SetActive(true);
        malesymbolnopertenece.SetActive(false);
        malesymbolnopertenece2.SetActive(false);
        femalesymbolnopertenece.SetActive(false);
        femalesymbolnopertenece2.SetActive(false);
        alltypeiconopertenece.SetActive(false);
        alltypeiconopertenece2.SetActive(false);


    }

    void MoveSelection()
    {
        state = BattleState.MoveSelection;
        dialogBox.enableActionSelector(false);
        dialogBox.enableDialogText(false);
        dialogBox.enableMoveSelector(true);
    }

    IEnumerator RunTurns(BattleAction playerAction)
    {
        state = BattleState.RunningTurn;

        if (playerAction == BattleAction.Move)
        {
            playerUnit.Pokemon.CurrentMove = playerUnit.Pokemon.Moves[currentMove];
            enemyUnit.Pokemon.CurrentMove = enemyUnit.Pokemon.GetRandomMove();

            int playerMovePriority = playerUnit.Pokemon.CurrentMove.Base.Priority;
            int enemyMovePriority = enemyUnit.Pokemon.CurrentMove.Base.Priority;

            //check who goes first
            bool playerGoesFirst = true;
            if (enemyMovePriority > playerMovePriority)
            {
                playerGoesFirst=false;
            }
            else if (enemyMovePriority == playerMovePriority)
            {
                playerGoesFirst = playerUnit.Pokemon.VelocidadFinal > enemyUnit.Pokemon.VelocidadFinal;
            }


            var firstUnit = (playerGoesFirst) ? playerUnit : enemyUnit;
            var secondUnit = (playerGoesFirst) ? enemyUnit : playerUnit;

            var secondPokemon = secondUnit.Pokemon;

            //first turn
            yield return RunMove(firstUnit, secondUnit, firstUnit.Pokemon.CurrentMove);
            yield return RunAfterTurn(firstUnit);
            if (state == BattleState.BattleOver)
            {
                yield break;
            }

            if (secondPokemon.HP > 0)
            {
                //second turn
                yield return RunMove(secondUnit, firstUnit, secondUnit.Pokemon.CurrentMove);
                yield return RunAfterTurn(secondUnit);
                if (state == BattleState.BattleOver)
                {
                    yield break;
                }
            }
        }
        else
        {
            if (playerAction == BattleAction.SwitchPokemon)
            {
                var selectedPokemon = playerParty.Pokemons[currentMember];
                state = BattleState.Busy;
                yield return SwitchPokemon(selectedPokemon);
            }

            //enemy turn
            var enemyMove = enemyUnit.Pokemon.GetRandomMove();
            yield return RunMove(enemyUnit, playerUnit, enemyMove);
            yield return RunAfterTurn(enemyUnit);
            if (state == BattleState.BattleOver)
            {
                yield break;
            }
        }

        if (state != BattleState.BattleOver)
        {
            ActionSelection();
        }
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit, Move move)
    {
        bool canRunMove = sourceUnit.Pokemon.OnBeforeTurn();

        if (!canRunMove)
        {
            yield return ShowStatusChanges(sourceUnit.Pokemon);
            yield return sourceUnit.Hud.UpdateHP();
            yield break;
        }
        yield return ShowStatusChanges(sourceUnit.Pokemon);

        move.PP--;
        dialogBox.enableMoveSelector(false);

        if (sourceUnit.IsPlayerUnit)
        {
            yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.name} ha usado {move.Base.name}");
        }
        else
        {
            yield return dialogBox.TypeDialog($"El {sourceUnit.Pokemon.Base.name} enemigo ha usado {move.Base.name}");
        }

        if (CheckIfMoveHits(move, sourceUnit.Pokemon, targetUnit.Pokemon))
        {
            sourceUnit.PlayAttackAnimation();
            yield return new WaitForSeconds(1f);
            targetUnit.playHitAnimation();

            if (move.Base.fee == FisEspEst.Estado)
            {
                yield return RunMoveEffects(move.Base.Effects, sourceUnit.Pokemon, targetUnit.Pokemon, move.Base.Target,sourceUnit.IsPlayerUnit);
            }
            else
            {
                var damageDetails = targetUnit.Pokemon.TakeDamage(move, sourceUnit.Pokemon);
                yield return targetUnit.Hud.UpdateHP();
                yield return ShowDamageDetails(damageDetails);
            }

            if (move.Base.Secondaries != null && move.Base.Secondaries.Count > 0 && targetUnit.Pokemon.HP > 0)
            {
                foreach (var secondary in move.Base.Secondaries)
                {
                    var rng = UnityEngine.Random.Range(1,101);
                    if (rng <= secondary.Chance)
                    {
                        yield return RunMoveEffects(secondary, sourceUnit.Pokemon, targetUnit.Pokemon, secondary.Target,sourceUnit.IsPlayerUnit);
                    }
                }
            }

            if (targetUnit.Pokemon.HP <= 0)
            {
                if (targetUnit.IsPlayerUnit)
                {
                    yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.name} se ha debilitado");
                }
                else
                {
                    yield return dialogBox.TypeDialog($"{targetUnit.Pokemon.Base.name} enemigo se ha debilitado");
                }

                targetUnit.playFaintAnimation();
                yield return new WaitForSeconds(2f);

                CheckForBattleOver(targetUnit);
            }
        }
        else
        {
            if (targetUnit.IsPlayerUnit)
            {
                yield return dialogBox.TypeDialog($"¡{targetUnit.Pokemon.Base.name} ha evitado el ataque!");
            }
            else
            {
                yield return dialogBox.TypeDialog($"¡El {targetUnit.Pokemon.Base.name} enemigo ha evitado el ataque!");
            }
            
        }
    }

    IEnumerator RunMoveEffects(MoveEffects effects, Pokemon source, Pokemon target, MoveTarget moveTarget,bool isPlayerUnit)
    {
        if (state == BattleState.BattleOver)
        {
            yield break;
        }

        // Stat Boosting
        if (effects.Boosts != null)
        {
            if (moveTarget == MoveTarget.Self)
            {
                source.ApplyBoost(effects.Boosts);
            }
            else if (moveTarget == MoveTarget.Foe)
            {
                target.ApplyBoost(effects.Boosts);
            }
            else
            {
                Debug.Log("El movimiento de efecto no targetea a foe o self");
            }
        }

        // Status Condition
        if (effects.Status != ConditionID.none)
        {
            target.SetStatus(effects.Status, isPlayerUnit);
        }

        // volatileStatus Condition
        if (effects.VolatileStatus != ConditionID.none)
        {
            target.SetVolatileStatus(effects.VolatileStatus, isPlayerUnit);
        }

        yield return ShowStatusChanges(source);
        yield return ShowStatusChanges(target);
    }

    IEnumerator RunAfterTurn(BattleUnit sourceUnit)
    {
        if (state == BattleState.BattleOver) { yield break; }
        yield return new WaitUntil(() => state == BattleState.RunningTurn);

        // Statuses like burn or psn will hurt the pkmn after the turn
        sourceUnit.Pokemon.OnAfterTurn();
        yield return ShowStatusChanges(sourceUnit.Pokemon);
        yield return sourceUnit.Hud.UpdateHP();

        if (sourceUnit.Pokemon.HP <= 0)
        {
            if (sourceUnit.IsPlayerUnit)
            {
                yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.name} se ha debilitado");
            }
            else
            {
                yield return dialogBox.TypeDialog($"{sourceUnit.Pokemon.Base.name} enemigo se ha debilitado");
            }

            sourceUnit.playFaintAnimation();
            yield return new WaitForSeconds(2f);

            CheckForBattleOver(sourceUnit);
        }
    }

    bool CheckIfMoveHits(Move move, Pokemon source, Pokemon target)
    {
        if (move.Base.AlwaysHits)
        {
            return true;
        }

        float moveAccuracy = move.Base.Precisión;

        int accuracy = source.StatBoosts[Stat.Accuracy];
        int evasion = target.StatBoosts[Stat.Evasion];

        var boostValues = new float[] { 3f/3f, 4f/3f, 5f/3f, 6f/3f, 7f/3f, 8f/3f, 9f/3f };

        if (accuracy > 0)
        {
            moveAccuracy *= boostValues[accuracy];
        }
        else
        {
            moveAccuracy /= boostValues[-accuracy];
        }

        if (evasion > 0)
        {
            moveAccuracy /= boostValues[evasion];
        }
        else
        {
            moveAccuracy *= boostValues[-evasion];
        }

        return UnityEngine.Random.Range(0, 101) <= moveAccuracy;
    }

    IEnumerator ShowStatusChanges(Pokemon pokemon)
    {
        while (pokemon.StatusChanges.Count > 0)
        {
            var message = pokemon.StatusChanges.Dequeue();
            yield return dialogBox.TypeDialog(message);
        }
    }

    void CheckForBattleOver(BattleUnit faintedUnit)
    {
        if (faintedUnit.IsPlayerUnit)
        {
            var nextPokemon = playerParty.GetHealthyPokemon();
            if (nextPokemon != null)
            {
                OpenPartyScreen();
            }
            else
            {
                BattleOver(false);
            }
        }
        else
        {
            BattleOver(true);
        }
    }

    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("¡Un golpe crítico!");
        }

        if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog("¡Es supereficaz!");
        }
        else if (damageDetails.TypeEffectiveness < 1f) 
        {
            yield return dialogBox.TypeDialog("No es muy eficaz...");
        }
    }

    public void HandleUpdate()
    {
        if (state == BattleState.ActionSelection)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.MoveSelection)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PartyScreen)
        {
            HandlePartyScreenSelection();
        }
    }
    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentAction;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentAction += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentAction -= 2;
        }

        currentAction = Math.Clamp(currentAction, 0, 3);

        dialogBox.ActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction== 0)
            {
                //fight
                MoveSelection();
            }
            else if (currentAction== 1)
            {
                //bag
            }
            else if (currentAction == 2)
            {
                //party
                prevState = state;
                OpenPartyScreen();
                malesymbolnopertenece.SetActive(false);
                malesymbolnopertenece2.SetActive(false);
                femalesymbolnopertenece.SetActive(false);
                femalesymbolnopertenece2.SetActive(false);

                playerUnit.Hud.DisableStatusIco(true);
                enemyUnit.Hud.DisableStatusIco(true);
            }
            else if (currentAction == 3)
            {
                //run
            }
        }


    }

    void HandleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < playerUnit.Pokemon.Moves.Count - 1)
            {
                ++currentMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 0)
            {
                --currentMove;
            }
        }
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var move = playerUnit.Pokemon.Moves[currentMove];
            if (move.PP <= 0)
            {

            }
            else
            {
                dialogBox.enableMoveSelector(false);
                dialogBox.enableDialogText(true);
                StartCoroutine(RunTurns(BattleAction.Move));
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.enableMoveSelector(false);
            dialogBox.enableDialogText(true);
            
            ActionSelection();
        }
    }

    void HandlePartyScreenSelection()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --currentMember;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentMember += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentMember -= 2;
        }

        currentMember = Math.Clamp(currentMember, 0, playerParty.Pokemons.Count - 1);

        partyScreen.UpdateMemberSelection(currentMember);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var selectedMember = playerParty.Pokemons[currentMember];
            if (selectedMember.HP <= 0)
            {
                partyScreen.SetMessageText("¡A " + selectedMember.Base.name + " " + "no le quedan fuerzas para luchar!");
                return;
            }
            if (selectedMember== playerUnit.Pokemon)
            {
                partyScreen.SetMessageText(playerUnit.Pokemon.Base.name + " ya está luchando.");
                return;
            }

            partyScreen.gameObject.SetActive(false);

            if (playerUnit.Pokemon.Base.IsMale)
            {
                malesymbolnopertenece.SetActive(true);
            }
            else if (!playerUnit.Pokemon.Base.IsMale)
            {
                femalesymbolnopertenece.SetActive(true);
            }

            if (enemyUnit.Pokemon.Base.IsMale)
            {
                malesymbolnopertenece2.SetActive(true);
            }
            else
            {
                femalesymbolnopertenece2.SetActive(true);
            }

            alltypeiconopertenece.SetActive(true);
            alltypeiconopertenece2.SetActive(true);

            playerUnit.Hud.DisableStatusIco(false);
            enemyUnit.Hud.DisableStatusIco(false);

            if (prevState == BattleState.ActionSelection)
            {
                prevState = null;
                StartCoroutine(RunTurns(BattleAction.SwitchPokemon));
            }
            else
            {
                state = BattleState.Busy;
                StartCoroutine(SwitchPokemon(selectedMember));
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            partyScreen.gameObject.SetActive(false);

            if (playerUnit.Pokemon.Base.IsMale)
            {
                malesymbolnopertenece.SetActive(true);
            }
            else if (!playerUnit.Pokemon.Base.IsMale)
            {
                femalesymbolnopertenece.SetActive(true);
            }

            if (enemyUnit.Pokemon.Base.IsMale)
            {
                malesymbolnopertenece2.SetActive(true);
            }
            else
            {
                femalesymbolnopertenece2.SetActive(true);
            }

            alltypeiconopertenece.SetActive(true);
            alltypeiconopertenece2.SetActive(true);

            playerUnit.Hud.DisableStatusIco(false);
            enemyUnit.Hud.DisableStatusIco(false);

            ActionSelection();
        }
    }

    IEnumerator SwitchPokemon(Pokemon newPokemon)
    {
        if (playerUnit.Pokemon.HP > 0) 
        { 
            yield return dialogBox.TypeDialog($"¡Vuelve {playerUnit.Pokemon.Base.name}! \nTe vendrá bien un cambio");
            playerUnit.playFaintAnimation();
            yield return new WaitForSeconds(2f);
        }

        playerUnit.Setup(newPokemon);
        dialogBox.SetMoveNames(newPokemon.Moves);
        yield return StartCoroutine(dialogBox.TypeDialog($"¡Adelante, {newPokemon.Base.name}! \n¡Confío en ti!"));

        state = BattleState.RunningTurn;
    }

}
