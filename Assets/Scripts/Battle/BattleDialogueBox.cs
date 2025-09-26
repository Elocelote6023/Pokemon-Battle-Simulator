using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleDialogueBox : MonoBehaviour
{
    [SerializeField] Text dialogText;
    [SerializeField] Color highlightedColor;

    [SerializeField] int letrasPorSegundo;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionText;
    [SerializeField] List<Text> moveText;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;
    [SerializeField] Text damageText;
    [SerializeField] Text precisionText;
    [SerializeField] GameObject IconoFisico;
    [SerializeField] GameObject IconoEspecial;
    [SerializeField] GameObject IconoEstado;






    public void SetDialogue(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/letrasPorSegundo);
        }
        yield return new WaitForSeconds(1f);
    }

    public void enableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    public void enableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void enableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    public void enableIconoFisico(bool enabled)
    {
        IconoFisico.SetActive(true);
        IconoEspecial.SetActive(false);
        IconoEstado.SetActive(false);
    }
    public void enableIconoEspecial(bool enabled)
    {
        IconoFisico.SetActive(false);
        IconoEspecial.SetActive(true);
        IconoEstado.SetActive(false);
    }
    public void enableIconoEstado(bool enabled)
    {
        IconoFisico.SetActive(false);
        IconoEspecial.SetActive(false);
        IconoEstado.SetActive(true);
    }

    public void ActionSelection(int selectedaction)
    {
        for (int i=0; i<actionText.Count; ++i)
        {
            if (i == selectedaction)
            {
                actionText[i].color= highlightedColor;
            }
            else
            {
                actionText[i].color = Color.black;
            }
        }
    }

    public void UpdateMoveSelection(int selectedmove, Move move)
    {
        for (int i=0; i<moveText.Count; ++i)
        {
            if (i == selectedmove)
            {
                moveText[i].color = highlightedColor;
            }
            else
            {
                moveText[i].color = Color.black;
            }
        }
        ppText.text = $"PP {move.PP}/{move.Base.PP}";
        typeText.text = move.Base.Type.ToString();
        damageText.text = $"Daño {move.Damage}";
        precisionText.text = $"{move.Precision}%";

        if (move.PP <= 0)
        {
            ppText.color = Color.red;
        }
        else 
        { 
            ppText.color = Color.black;
        }

        if (move.Feen == 1)
        {
            enableIconoFisico(true);
        }
        else if (move.Feen == 2)
        {
            enableIconoEspecial(true);
        }
        else if (move.Feen == 3)
        {
            enableIconoEstado(true);
        }
    }

    public void SetMoveNames(List<Move> moves)
    {
        for (int i=0; i<moveText.Count; ++i)
        {
            if (i<moves.Count)
            {
                moveText[i].text = moves[i].Base.name;
            }
            else
            {
                moveText[i].text = "-";
            }
        }
    }
}
