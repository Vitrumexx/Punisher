using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrapsToText : MonoBehaviour
{
    [SerializeField] private PlayerParameters playerParam;
    [SerializeField] private TextMeshProUGUI TMPText;

    void FixedUpdate()
    {
        TMPText.text = playerParam._supplies.ToString();
    }
}
