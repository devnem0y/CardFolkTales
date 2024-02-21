using UnityEngine;

public class Hand : MonoBehaviour
{
    public int CardCount => transform.childCount;
    public bool HasCard => transform.childCount > 0;

    /// <summary>
    /// Возвращает первую карту
    /// </summary>
    public Card GetFirstCard()
    {
        return HasCard ? transform.GetChild(0).GetComponent<Card>() : null;
    }
    
    /// <summary>
    /// Возвращает вторую карту
    /// </summary>
    public Card GetSecondCard()
    {
        return HasCard && CardCount > 1 ? transform.GetChild(1).GetComponent<Card>() : null;
    }
    
    /// <summary>
    /// Возвращает случайную карту
    /// </summary>
    public Card GetRandomCard()
    {
        if (!HasCard) return null;
        
        if (CardCount < 2) return transform.GetChild(0).GetComponent<Card>();

        var rnd = Random.Range(0, 11);
        return rnd <= 5 ? transform.GetChild(0).GetComponent<Card>() : transform.GetChild(1).GetComponent<Card>();
    }
    
    /// <summary>
    /// Возвращает карту юнита данного типа
    /// </summary>
    /// <param name="unitType">Тип юнита</param>
    public Card GetCardWithType(UnitType unitType)
    {
        if (!HasCard) return null;

        for (var i = 0; i < CardCount; i++)
        {
            var card = transform.GetChild(i).GetComponent<Card>();
            if (card.Get<Unit>().Type == unitType) return card;
        }

        return null;
    }
    
    /// <summary>
    /// Возвращает карту бонуса данного типа
    /// </summary>
    /// <param name="bonusType">Тип бонуса</param>
    public Card GetCardWithType(BonusType bonusType)
    {
        if (!HasCard) return null;

        for (var i = 0; i < CardCount; i++)
        {
            var card = transform.GetChild(i).GetComponent<Card>();
            if (card.Get<Bonus>().Type == bonusType) return card;
        }

        return null;
    }

    public void CoverUp()
    {
        for (var i = 0; i < CardCount; i++)
        {
            var card = transform.GetChild(i).GetComponent<Card>();
            card.CoverUp(true);
            card.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
}