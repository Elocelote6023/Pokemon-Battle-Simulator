using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberUI : MonoBehaviour
{
    [SerializeField] Text NameText;
    [SerializeField] Text lvlText;
    [SerializeField] Text hpBarNumber;
    [SerializeField] float hpBarSpeed;
    [SerializeField] GameObject health;
    [SerializeField] GameObject maleSymbol;
    [SerializeField] GameObject femaleSymbol;


    [SerializeField] Color highlitedColor;

    Pokemon _pokemon;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        NameText.text = pokemon.Base.Nombre;
        lvlText.text = "Lvl " + pokemon.Level;
        SetHP((float)pokemon.HP / pokemon.VidaFinal);

        hpBarNumber.text = (float)pokemon.HP + "/" + pokemon.VidaFinal;

        if (pokemon.Base.IsMale)
        {
            maleSymbol.SetActive(true);
            femaleSymbol.SetActive(false);
        }
        else
        {
            maleSymbol.SetActive(false);
            femaleSymbol.SetActive(true);
        }
    }

    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3((float)_pokemon.HP / _pokemon.VidaFinal, 1f, 1f);
        hpBarNumber.text = (float)_pokemon.HP + "/" + _pokemon.VidaFinal;
    }

    public void SetSelected(bool selected)
    {
        if(selected)
        {
            NameText.color = highlitedColor;
        }
        else
        {
            NameText.color = Color.black;
        }
    }
}
