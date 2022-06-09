using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BernardStates : MonoBehaviour
{
	private AISetUp AISU;
	public BernardAttacking BA;
	public BernardIdle BI;
	public BernardAnimationSystem BAS;
	public EnemyHealth EH;

	private GameObject m_Player;
	public GameObject bernardWallBody;

	private PolygonCollider2D bernardTailCollider;
    private CapsuleCollider2D bernardBodyCollider;

    private float m_DistanceToPlayer;
	public float m_AttackDistance;
	public float m_StartFightDistance;
	public float m_Health;
	private float m_MaxHealth;
	private float m_HealthPercentage;
	[SerializeField] private int m_HealthPercentageRounded;

	private int state;

	[SerializeField] private bool m_Attacking;
	private bool startFight = false;

	public enum states
	{
		Idle,
		Attacking,
	}

	public string[] IdleStateTypes = { "Idle_1", "Idle_2", "Idle_3" };
	public string[] AttackStateTypes = { "Attacking_1", "Attacking_2", "Attacking_3" };

	private void Start()
	{
		AISU = GameObject.Find("AI_Setup").GetComponent<AISetUp>();

		m_Player = AISU.m_ActivePlayer;

		m_MaxHealth = GetComponent<EnemyHealth>().m_MaxHP;
		m_Health = m_MaxHealth;

        bernardBodyCollider = GetComponent<CapsuleCollider2D>();
        bernardTailCollider = GetComponent<PolygonCollider2D>();
    }

	private void FixedUpdate()
	{
		//if (FindObjectOfType<BernardIntroCutscene>().isCutscene)
		//{
		//	return;
		//}

		m_DistanceToPlayer = Vector2.Distance(gameObject.transform.position, m_Player.transform.position);
		m_Health = GetComponent<EnemyHealth>().m_CurrentHP;

		if (m_DistanceToPlayer < m_StartFightDistance)
		{
			state = (int)states.Attacking;
		}

		m_HealthPercentage = (m_MaxHealth * 33.33f) / 100;
		m_HealthPercentageRounded = (int)Math.Round(m_HealthPercentage, 0);

		switch (state)
		{
			case 0:
				m_Attacking = false;

				if (BA.onWall)
				{
					BAS.currentAnimName = "LizardWallIdle";
				}
				else
				{
					BAS.currentAnimName = "LizardIdle";
				}

				BI.Invoke(IdleStateTypes[0], 0f);

				break;
			case 1:
				m_Attacking = true;

				if (m_Health > 0 && m_Health < m_HealthPercentageRounded)
				{
					BAS.currentAnimName = "LizardDamagedRun";

                    if (!(EH.m_CurrentHP <= 0))
                    {
                        bernardBodyCollider.enabled = true;
                        bernardTailCollider.enabled = true;
                    }

                    bernardWallBody.SetActive(false);

                    BA.Invoke(AttackStateTypes[2], 0f);
				}
				else if (m_Health >= m_HealthPercentageRounded && m_Health < m_HealthPercentageRounded * 2)
				{
					if (BA.jumpedUp == true)
					{
						if (!BA.onWall)
						{
							//BAS.currentAnimName = "LizardJump";

                            //bernardBodyCollider.enabled = false;
                            //bernardTailCollider.enabled = false;

                            //bernardWallBody.SetActive(true);
                        }
						else
						{
							BAS.currentAnimName = "LizardWallIdle";
						}

					}
					BA.Invoke(AttackStateTypes[1], 0f);
				}
				else if (m_Health >= m_HealthPercentageRounded * 2 && m_Health <= m_MaxHealth)
				{
					BAS.currentAnimName = "LizardRun";
					
					BA.Invoke(AttackStateTypes[0], 0f);
				}
				else if (m_Health <= 0)
                {
					BAS.currentAnimName = "LizardDying";

					BA.Invoke("Die", 0);
                }

				break;
		}
	}
}
