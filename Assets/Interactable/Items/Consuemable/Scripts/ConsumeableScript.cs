using UnityEngine;

public class ConsumeableScript : MonoBehaviour
{
    public ItemScriptableObject item;
    public PlayerParameters playerParameters;

    private void Awake()
    {
        playerParameters = GetComponentInParent<PlayerParameters>();
    }
   

    public void Modifiers()
    {
        if (item == null)
        {
            Debug.LogWarning($"{name}: ItemScriptableObject не назначен!");
            return;
        }

        if (playerParameters == null)
        {
            Debug.LogWarning($"{name}: PlayerParameters не найден!");
            return;
        }
        else
        {
            playerParameters._health += item.RestoreHealth;
            Debug.Log($"Здоровье увеличено на {item.RestoreHealth}. Текущее: {playerParameters._health}");  
        }
        
    }
}
