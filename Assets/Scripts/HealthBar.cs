using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RectTransform bar;
    private Image barImage;
    private PlayerController playerController;
    private Color originalColor; // Сохраняем исходный цвет
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        bar = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();
        originalColor = barImage.color; // Сохраняем исходный цвет
        UpdateColor(); // Обновляем цвет при старте
        SetSize(Health.totalHealth);
    }

    public void Damage(float damage)
    {
        Health.totalHealth -= damage;
        if (Health.totalHealth < 0f)
        {
            Health.totalHealth = 0f;
        }

        if (Health.totalHealth <= 0f)
        {
            playerController.Restart();
        }

        SetSize(Health.totalHealth);
        UpdateColor(); // Обновляем цвет после изменения здоровья
    }

    private void UpdateColor()
    {
        if (Health.totalHealth < 0.3f)
        {
            barImage.color = Color.red;
        }
        else
        {
            barImage.color = originalColor; // Возвращаем исходный цвет
        }
    }

    public void SetSize(float size)
    {
        bar.localScale = new Vector3(size, 1f);
    }
}
