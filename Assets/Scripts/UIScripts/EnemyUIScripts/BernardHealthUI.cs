using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BernardHealthUI : MonoBehaviour
{
    public EnemyHealth EH;

    [Header("First Health Bar")]
    public Image healthFrontFillBar;
    public GameObject healthBarEmpty;
    public Image healthBackHealthBar;

    public float healthlerpTimer;
    private float healthLerpSpeed;

    [Header("Second Health Bar")]
    public Image healthFrontFillBarSecondary;
    public GameObject healthBarEmptySecondary;
    public Image healthBackHealthBarSecondary;

    private float healthLerpTimerSecond;
    private float healthLerpSpeedSecond;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //healthBarEmpty.GetComponent<RectTransform>().sizeDelta = new Vector2(EH.m_MaxHP, 32);

        float fillF = Mathf.Round(healthFrontFillBar.fillAmount * 100) * 0.01f;
        float fillB = Mathf.Round(healthBackHealthBar.fillAmount * 100) * 0.01f;

        float fillFSecond = Mathf.Round(healthFrontFillBarSecondary.fillAmount * 100) * 0.01f;
        float fillBSecond = Mathf.Round(healthBackHealthBarSecondary.fillAmount * 100) * 0.01f;

        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        float fillF = healthFrontFillBar.fillAmount;
        float fillB = healthBackHealthBar.fillAmount;

        float fillFSecond = healthFrontFillBarSecondary.fillAmount;
        float fillBSecond = healthBackHealthBarSecondary.fillAmount;

        float healthFraction = EH.m_CurrentHP/ (float)EH.m_MaxHP;


        // Decreases healthbar UI when player takes damage
        if (fillB > healthFraction)
        {
            healthLerpSpeed = 15f;

            healthFrontFillBar.fillAmount = healthFraction;

            healthlerpTimer += Time.deltaTime;
            float percentComplete = healthlerpTimer / healthLerpSpeed;
            percentComplete *= percentComplete;

            healthBackHealthBar.fillAmount = Mathf.Lerp(fillB, healthFraction, percentComplete);

        }

        // Increases health UI when boss regens health
        if (fillF < healthFraction)
        {
            healthLerpSpeed = 1f;
            healthBackHealthBar.fillAmount = healthFraction;
            healthlerpTimer += Time.deltaTime;

            float percentComplete = healthlerpTimer / healthLerpSpeed;
            percentComplete *= percentComplete;

            healthFrontFillBar.fillAmount = Mathf.Lerp(fillF, healthBackHealthBar.fillAmount, percentComplete);
        }

        if (fillBSecond > healthFraction)
        {
            healthLerpSpeed = 15f;

            healthFrontFillBarSecondary.fillAmount = healthFraction;

            healthlerpTimer += Time.deltaTime;
            float percentComplete = healthlerpTimer / healthLerpSpeed;
            percentComplete *= percentComplete;

            healthBackHealthBarSecondary.fillAmount = Mathf.Lerp(fillBSecond, healthFraction, percentComplete);

        }

        if (fillFSecond < healthFraction)
        {
            healthLerpSpeed = 1f;
            healthBackHealthBarSecondary.fillAmount = healthFraction;
            healthlerpTimer += Time.deltaTime;

            float percentComplete = healthlerpTimer / healthLerpSpeed;
            percentComplete *= percentComplete;

            healthFrontFillBarSecondary.fillAmount = Mathf.Lerp(fillFSecond, healthBackHealthBarSecondary.fillAmount, percentComplete);
        }
    }
}
