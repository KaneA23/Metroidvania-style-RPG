using UnityEngine;

/// <summary>
/// Controls whether a UI bar is visible. UI only available when there is an ability that uses it.
/// Created by: Kane Adams
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Referenced Scripts")]
    public BasePlayerClass BPC;

    [Header("User Interface Objects")]
    public GameObject healthBar;
    public GameObject staminaBar;
    public GameObject manaBar;

    // Update is called once per frame
    void Update()
    {
        if (BPC.hasRun)
        {
            staminaBar.SetActive(true);
        }
        else
        {
            staminaBar.SetActive(false);
        }

        if (BPC.hasLightAtk)
		{
            healthBar.SetActive(true);
		}
		else
		{
            healthBar.SetActive(false);
        }

        if (BPC.hasDash)
        {
            manaBar.SetActive(true);
        }
        else
        {
            manaBar.SetActive(false);
        }
    }
}
