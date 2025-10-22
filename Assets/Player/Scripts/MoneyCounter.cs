using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyCounter : MonoBehaviour
{
    private PlayerParameters playerParam;
    [SerializeField] private TMP_Text ShopText;
    void Start()
    {
        playerParam = GetComponent<PlayerParameters>();
    }

    void Update()
    {
        ChangeText();
    }

    void ChangeText()
    {
        ShopText.text = playerParam._money.ToString();
    }
}
