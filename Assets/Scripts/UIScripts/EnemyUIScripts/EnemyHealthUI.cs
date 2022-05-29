using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
	public EnemyHealth EH;

	[Header("First Health Bar")]
	public Image healthFrontFillBar;
	public GameObject healthBarEmpty;
	public Image healthBackHealthBar;

	[SerializeField] private GameObject healthBar;

	private float healthlerpTimer;
	private float healthLerpSpeed;

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

		if (fillF == fillB && fillF == 0 && healthBar.activeInHierarchy)
		{
			healthBar.SetActive(false);
		}

		UpdateHealthUI();
	}

	public void UpdateHealthUI()
	{
		float fillF = healthFrontFillBar.fillAmount;
		float fillB = healthBackHealthBar.fillAmount;

		float healthFraction = EH.m_CurrentHP / (float)EH.m_MaxHP;

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
	}
}
