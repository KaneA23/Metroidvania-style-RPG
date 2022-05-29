using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BernardAnimationSystem : MonoBehaviour
{
    public BernardAttacking BA;
    public EnemyHealth EH;

    public Animator animator;

    private SpriteRenderer spriteRenderer;

    private int currentAnimFrame;
    private int collNum = 0;

    public string currentAnimName;
    public string currentAnimNameNew;

    public string[] bernardAnimations = { "Bernard_Idle", "Bernard_Damaged_Run", "Bernard_Run", "Bernard_Wall_Idle", "Bernard_Jump", "Bernard_Walk", "Bernard_Dying" };
    [SerializeField] private string currentAnimation = null;

    public enum bernardAnimationStates
    {
        BERNARD_IDLE,       
        BERNARD_DAMAGED_RUN,   
        BERNARD_RUN,  
        BERNARD_WALL_IDLE,
        BERNARD_JUMP,
        BERNARD_WALK,
        BERNARD_DYING
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

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

                break;
            case "LizardRun":

                ChangeAnimation(bernardAnimationStates.BERNARD_RUN);              

                break;

            case "LizardWallIdle":

                ChangeAnimation(bernardAnimationStates.BERNARD_WALL_IDLE);

                break;

            case "LizardJump":

                ChangeAnimation(bernardAnimationStates.BERNARD_JUMP);

                break;

            case "LizardWalk":

                ChangeAnimation(bernardAnimationStates.BERNARD_WALK);

                break;
            case "LizardDying":

                ChangeAnimation(bernardAnimationStates.BERNARD_DYING);

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
