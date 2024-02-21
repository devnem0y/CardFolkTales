using System;
using UnityEngine;

public abstract class CommanderBase : MonoBehaviour
{
    public event Action<CardBase> OnCardUse; //TODO: Подписывается Controller, а инвокает Card
    
    public void InvokeCardUse(CardBase cardBase)
    {
        OnCardUse?.Invoke(cardBase);
    }
}