using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayerUnit;
    [SerializeField] BattleHud hud;

    public bool IsPlayerUnit
    {
        get
        {
            return isPlayerUnit;
        }
    }

    public BattleHud Hud
    {
        get
        {
            return hud;
        }
    }

    [SerializeField] public GameObject malesymbol;
    [SerializeField] public GameObject femalesymbol;


    public Pokemon Pokemon { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        if (isPlayerUnit)
        {
            image.sprite = Pokemon.Base.backSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.frontSprite;
        }

        if (Pokemon.Base.IsMale == true) 
        {
            femalesymbol.SetActive(false);
            malesymbol.SetActive(true);
        }
        else
        {
            femalesymbol.SetActive(true);
            malesymbol.SetActive(false);
        }

        hud.SetData(Pokemon);

        image.color = originalColor;
        PlayEnterAnimation();
        
    }

    public void DisableSymbolsMF()
    {
        femalesymbol.SetActive(false);
        malesymbol.SetActive(false);
    }

    public void PlayEnterAnimation ()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-370f, originalPos.y, originalPos.z);
        }
        else
        {
            image.transform.localPosition = new Vector3(+370f, originalPos.y, originalPos.z);
        }
        image.transform.DOLocalMoveX(originalPos.x, 1f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayerUnit) 
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 37f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + -37f, 0.25f));
        }
        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void playHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));

    }

    public void playFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 111f, 0.5f));
        sequence.Join(image.DOFade(0f,0.5f));
    }
}
