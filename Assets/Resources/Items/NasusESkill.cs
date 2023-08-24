using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/NasusESkill")]
public class NasusESkill : Item
{
    public GameObject skillEffectPrefab; // Prefab of the skill's effect (damage area, particles, etc.)

    public GameObject skillLayoutVisualization; // The instantiated skill layout visualization

    private GameObject skilldemon;
    private bool holdingSkill = false;
    private bool skillReady = true; // Indicates if the skill is ready to be used
    public float skillCooldown = 5.0f; // Cooldown time in seconds

    private float timeSinceLastSkillUse = Mathf.Infinity; // Initialize with a very large value to allow immediate use
    private Mortality _playerMortality;

    private void OnEnable()
    {
        // Update the skill layout visualization position to follow the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;


        skilldemon = Instantiate(skillLayoutVisualization, mousePosition, Quaternion.identity);

        skilldemon.SetActive(false);
        _playerMortality = PlayerManager.Instance.MortalityScript;
    }
    public override void WhileHolding()
    {
        if (holdingSkill)
        {
            skilldemon.SetActive(true);
            // Update the skill layout visualization position to follow the mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            skilldemon.transform.position = mousePosition;

            if (Input.GetMouseButtonDown(0) && skillReady)
            {
                // Player clicked while holding the skill button and skill is ready
                // Instantiate the skill effect at the mouse position
                Instantiate(skillEffectPrefab, mousePosition, Quaternion.identity);

                _playerMortality.ActiveEnergy -= 120;
                // Set skill on cooldown
                skillReady = false;
                timeSinceLastSkillUse = Time.time;
            }
        }

        // Check if cooldown has finished
        if (!skillReady && Time.time - timeSinceLastSkillUse >= skillCooldown)
        {
            skillReady = true; // Skill is ready again
        }
    }

    public override void Use()
    {
        holdingSkill = !holdingSkill;
        skilldemon.SetActive(holdingSkill);

        //// Check if the skill is ready and the player is holding the skill
        //if (skillReady)
        //{
        //    // Update the skill layout visualization position to follow the mouse
        //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    mousePosition.z = 0;

        //    if (!holdingSkill)
        //    {
        //        // Instantiate the skill layout visualization if it doesn't exist
        //        skillLayoutVisualization = Instantiate(skillEffectPrefab, mousePosition, Quaternion.identity);
        //        holdingSkill = true;
        //    }

        //    skillLayoutVisualization.transform.position = mousePosition;

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        // Player clicked while holding the skill button and skill is ready
        //        // Instantiate the skill effect at the mouse position
        //        Instantiate(skillEffectPrefab, mousePosition, Quaternion.identity);

        //        // Set skill on cooldown
        //        skillReady = false;
        //        timeSinceLastSkillUse = Time.time;
        //    }
        //}
        //else
        //{
        //    // Destroy the skill layout visualization if it exists
        //    if (holdingSkill)
        //    {
        //        Destroy(skillLayoutVisualization);
        //        holdingSkill = false;
        //    }
        //}

        //// Check if cooldown has finished
        //if (!skillReady && Time.time - timeSinceLastSkillUse >= skillCooldown)
        //{
        //    skillReady = true; // Skill is ready again
        //}
    }
}