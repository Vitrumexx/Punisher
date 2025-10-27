using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParameterBars : MonoBehaviour
{
    [SerializeField] private PlayerParameters playerParam;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject HealthSlider_obj;

    [SerializeField] private Slider staminaBar;
    [SerializeField] private GameObject StaminaSlider_obj;

    [SerializeField] private TextMeshProUGUI AmmoText;

    // Update is called once per frame
    void FixedUpdate()
    {
        int _bullets = playerParam._bullets;
        float _maxStamina = playerParam._maxStamina;
        float _stamina = playerParam._stamina;
        float _maxHealth = playerParam._maxHealth;
        float _health = playerParam._health;
        HealthBar(_health, _maxHealth);
        StaminaBar(_stamina, _maxStamina);
        AmmoCounter(_bullets);
    }

    public void HealthBar(float _health, float _maxHealth)
    {
        float norm = _health / _maxHealth;
        healthBar.value = norm;
    }

    public void StaminaBar(float _stamina, float _maxStamina)
    {
        float norm = _stamina / _maxStamina;
        staminaBar.value = norm;

        if (_stamina == _maxStamina)
            StaminaSlider_obj.SetActive(false);
        else
            StaminaSlider_obj.SetActive(true);
    }
    public void AmmoCounter(int _bullets)
    {
        AmmoText.text = _bullets.ToString();
    }
}
