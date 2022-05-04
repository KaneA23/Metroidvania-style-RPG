using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BernardAnimationSystem : MonoBehaviour
{
    public BernardAttacking BA;
    public EnemyHealth EH;

    public Animator animator;

    private SpriteRenderer spriteRenderer;

    public GameObject bernardWallBody;

    private PolygonCollider2D bernardTailCollider;
    private BoxCollider2D bernardBodyCollider;

    private int currentAnimFrame;
    private int collNum = 0;

    public string currentAnimName;
    public string currentAnimNameNew;

    public string[] bernardAnimations = { "Bernard_Idle", "Bernard_Damaged_Run", "Bernard_Run", "Bernard_Wall_Idle", "Bernard_Jump", "Bernard_Walk" };
    [SerializeField] private string currentAnimation = null;

    public enum bernardAnimationStates
    {
        BERNARD_IDLE,       
        BERNARD_DAMAGED_RUN,   
        BERNARD_RUN,  
        BERNARD_WALL_IDLE,
        BERNARD_JUMP,
        BERNARD_WALK
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bernardBodyCollider = GetComponent<BoxCollider2D>();
        bernardTailCollider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        switch (currentAnimName)
        {
            case "LizardIdle":

                ChangeAnimation(bernardAnimationStates.BERNARD_IDLE);

                break;
            

            case "LizardDamagedRun":

                ChangeAnimation(bernardAnimationStates.BERNARD_DAMAGED_RUN);

                if(!(EH.m_CurrentHP <= 0))
                {
                    bernardBodyCollider.enabled = true;
                    bernardTailCollider.enabled = true;
                }
                
                bernardWallBody.SetActive(false);              

                break;
            case "LizardRun":

                ChangeAnimation(bernardAnimationStates.BERNARD_RUN);              

                break;

            case "LizardWallIdle":

                ChangeAnimation(bernardAnimationStates.BERNARD_WALL_IDLE);

                break;

            case "LizardJump":

                ChangeAnimation(bernardAnimationStates.BERNARD_JUMP);

                bernardBodyCollider.enabled = false;
                bernardTailCollider.enabled = false;

                bernardWallBody.SetActive(true);

                break;

            case "LizardWalk":

                ChangeAnimation(bernardAnimationStates.BERNARD_WALK);

                break;
        }

    }

    public void ChangeAnimation(bernardAnimationStates animState)
    {
        if (currentAnimation == bernardAnimations[(int)animState])
        {
            return;
        }

        animator.Play(bernardAnimations[(int)animState]);

        currentAnimation = bernardAnimations[(int)animState];
    }
}
