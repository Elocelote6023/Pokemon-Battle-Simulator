using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text NameText;
    [SerializeField] Text lvlText;
    [SerializeField] Text hpBarNumber;
    [SerializeField] float hpBarSpeed;
    [SerializeField] GameObject health;

    [SerializeField] GameObject shinyIcon;

    [SerializeField] GameObject aceroIco;
    [SerializeField] GameObject aguaIco;
    [SerializeField] GameObject bichoIco;
    [SerializeField] GameObject dragonIco;
    [SerializeField] GameObject electricoIco;
    [SerializeField] GameObject fantasmaIco;
    [SerializeField] GameObject fuegoIco;
    [SerializeField] GameObject hadaIco;
    [SerializeField] GameObject hieloIco;
    [SerializeField] GameObject luchaIco;
    [SerializeField] GameObject normalIco;
    [SerializeField] GameObject plantaIco;
    [SerializeField] GameObject psiquicoIco;
    [SerializeField] GameObject rocaIco;
    [SerializeField] GameObject siniestroIco;
    [SerializeField] GameObject tierraIco;
    [SerializeField] GameObject venenoIco;
    [SerializeField] GameObject voladorIco;

    [SerializeField] GameObject congeladoIco;
    [SerializeField] GameObject dormidoIco;
    [SerializeField] GameObject envenenadoIco;
    [SerializeField] GameObject envenenado2Ico;
    [SerializeField] GameObject paralizadoIco;
    [SerializeField] GameObject quemadoIco;
    [SerializeField] GameObject allstatusIco;

    Pokemon _pokemon;

    Dictionary<ConditionID, int> statusNumber;

    public void SetData(Pokemon pokemon)
    {
        _pokemon = pokemon;

        NameText.text = pokemon.Base.Nombre;
        lvlText.text = "Lvl " + pokemon.Level;
        SetHP((float)pokemon.HP / pokemon.VidaFinal);

        hpBarNumber.text = (float)pokemon.HP + "/" + pokemon.VidaFinal;
        
        if (pokemon.Base.IsShiny)
        {
            shinyIcon.SetActive(true);
        }
        else
        {
            shinyIcon.SetActive(false);
        }

        DisableAllTypeIco();
        SetTypeIco(pokemon);

        statusNumber = new Dictionary<ConditionID, int>()
        {
            {ConditionID.frz, 1},
            {ConditionID.slp, 2},
            {ConditionID.psn, 3},
            {ConditionID.psn2, 4},
            {ConditionID.par, 5},
            {ConditionID.brn, 6}
        };


        SetStatusIco();
        _pokemon.OnStatusChanged += SetStatusIco;
    }

    public void SetStatusIco()
    {
        if (_pokemon.Status == null)
        {
            DisableStatusIco(false);

            congeladoIco.SetActive(false);
            dormidoIco.SetActive(false);
            envenenadoIco.SetActive(false);
            envenenado2Ico.SetActive(false);
            paralizadoIco.SetActive(false);
            quemadoIco.SetActive(false);
        }
        else if (_pokemon.Status.ID.ToString() == "frz") 
        {
            DisableStatusIco(false);

            congeladoIco.SetActive(true);
            dormidoIco.SetActive(false);
            envenenadoIco.SetActive(false);
            envenenado2Ico.SetActive(false);
            paralizadoIco.SetActive(false);
            quemadoIco.SetActive(false);
        }
        else if (_pokemon.Status.ID.ToString() == "slp")
        {
            DisableStatusIco(false);

            congeladoIco.SetActive(false);
            dormidoIco.SetActive(true);
            envenenadoIco.SetActive(false);
            envenenado2Ico.SetActive(false);
            paralizadoIco.SetActive(false);
            quemadoIco.SetActive(false);
        }
        else if (_pokemon.Status.ID.ToString() == "psn")
        {
            DisableStatusIco(false);

            congeladoIco.SetActive(false);
            dormidoIco.SetActive(false);
            envenenadoIco.SetActive(true);
            envenenado2Ico.SetActive(false);
            paralizadoIco.SetActive(false);
            quemadoIco.SetActive(false);
        }
        else if (_pokemon.Status.ID.ToString() == "psn2")
        {
            DisableStatusIco(false);

            congeladoIco.SetActive(false);
            dormidoIco.SetActive(false);
            envenenadoIco.SetActive(false);
            envenenado2Ico.SetActive(true);
            paralizadoIco.SetActive(false);
            quemadoIco.SetActive(false);
        }
        else if (_pokemon.Status.ID.ToString() == "par")
        {
            DisableStatusIco(false);

            congeladoIco.SetActive(false);
            dormidoIco.SetActive(false);
            envenenadoIco.SetActive(false);
            envenenado2Ico.SetActive(false);
            paralizadoIco.SetActive(true);
            quemadoIco.SetActive(false);
        }
        else if (_pokemon.Status.ID.ToString() == "brn")
        {
            DisableStatusIco(false);

            congeladoIco.SetActive(false);
            dormidoIco.SetActive(false);
            envenenadoIco.SetActive(false);
            envenenado2Ico.SetActive(false);
            paralizadoIco.SetActive(false);
            quemadoIco.SetActive(true);
        }

    }

    public void DisableStatusIco(bool true_)
    {
        if (true_)
        {
            allstatusIco.SetActive(false);
        }
        else if (!true_)
        {
            allstatusIco.SetActive(true);
        }
    }

    public void SetTypeIco(Pokemon pokemon)
    {
        if ((int)pokemon.Base.type1 == 0)
        {
            Debug.Log("el tipo principal del pkm es none");
        }
        else if ((int)pokemon.Base.type1 == (int)pokemon.Base.type2)
        {
            Debug.Log("el pkm no puede tener 2 veces el mismo tipo");
        }
        else if ((int)pokemon.Base.type1 == 1)
        {
            aceroIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 2)
        {
            aguaIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 3)
        {
            bichoIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 4)
        {
            dragonIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 5)
        {
            electricoIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 6)
        {
            fantasmaIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 7)
        {
            fuegoIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 8)
        {
            hadaIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 9)
        {
            hieloIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 10)
        {
            luchaIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 11)
        {
            normalIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 12)
        {
            plantaIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 13)
        {
            psiquicoIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 14)
        {
            rocaIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 15)
        {
            siniestroIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 16)
        {
            tierraIco.SetActive(true);
        }
        else if ((int)pokemon.Base.type1 == 17)
        {
            venenoIco.SetActive(true);
        }
        else if ((int)_pokemon.Base.type1 == 18)
        {
            voladorIco.SetActive(true);
        }

        if ((int)pokemon.Base.type2 == 0)
        {
            Debug.Log("el tipo secundario del pkm es none");
        }
        else if ((int)pokemon.Base.type2 == 1)
        {
            aceroIco.SetActive(true);
            aceroIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 2)
        {
            aguaIco.SetActive(true);
            aguaIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 3)
        {
            bichoIco.SetActive(true);
            bichoIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 4)
        {
            dragonIco.SetActive(true);
            dragonIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 5)
        {
            electricoIco.SetActive(true);
            electricoIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 6)
        {
            fantasmaIco.SetActive(true);
            fantasmaIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 7)
        {
            fuegoIco.SetActive(true);
            fuegoIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 8)
        {
            hadaIco.SetActive(true);
            hadaIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 9)
        {
            hieloIco.SetActive(true);
            hieloIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 10)
        {
            luchaIco.SetActive(true);
            luchaIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 11)
        {
            normalIco.SetActive(true);
            normalIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 12)
        {
            plantaIco.SetActive(true);
            plantaIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 13)
        {
            psiquicoIco.SetActive(true);
            psiquicoIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 14)
        {
            rocaIco.SetActive(true);
            rocaIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 15)
        {
            siniestroIco.SetActive(true);
            siniestroIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 16)
        {
            tierraIco.SetActive(true);
            tierraIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 17)
        {
            venenoIco.SetActive(true);
            venenoIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
        else if ((int)pokemon.Base.type2 == 18)
        {
            voladorIco.SetActive(true);
            voladorIco.transform.localPosition = new Vector3(-2.991028f, -2.749f, -2.5468211f);
        }
    }

    public void DisableAllTypeIco()
    {
        aceroIco.SetActive(false);
        aguaIco.SetActive(false);
        bichoIco.SetActive(false);
        dragonIco.SetActive(false);
        electricoIco.SetActive(false);
        fantasmaIco.SetActive(false);
        fuegoIco.SetActive(false);
        hadaIco.SetActive(false);
        hieloIco.SetActive(false);
        luchaIco.SetActive(false);
        normalIco.SetActive(false);
        plantaIco.SetActive(false);
        psiquicoIco.SetActive(false);
        rocaIco.SetActive(false);
        siniestroIco.SetActive(false);
        tierraIco.SetActive(false);
        venenoIco.SetActive(false);
        voladorIco.SetActive(false);
    }

    public IEnumerator UpdateHP()
    {
        
        if (_pokemon.HpChanged)
        {
            yield return SetHPSmoothly((float)_pokemon.HP / _pokemon.VidaFinal);
            _pokemon.HpChanged = false;
        }
    }

    public void SetHP(float hpNormalized)
    {
        health.transform.localScale = new Vector3((float)_pokemon.HP / _pokemon.VidaFinal, 1f, 1f);
        hpBarNumber.text = (float)_pokemon.HP + "/" + _pokemon.VidaFinal;
    }

    public IEnumerator SetHPSmoothly(float newHP)
    {
        float curhp = health.transform.localScale.x;
        float changeAmt = curhp - newHP;

        while (curhp - newHP > Mathf.Epsilon)
        {
            curhp -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(curhp, 1f, 1f);
            hpBarNumber.text = Mathf.RoundToInt(curhp * _pokemon.VidaFinal) + "/" + _pokemon.VidaFinal;
            yield return null;
        }
        health.transform.localScale = new Vector3(newHP, 1f, 1f);
    }

    

}
