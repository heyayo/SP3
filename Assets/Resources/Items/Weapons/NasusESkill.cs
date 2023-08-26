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

    public override void WhileHolding()
    {
        _playerMortality = PlayerManager.Instance.MortalityScript;

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
                if (_playerMortality.ActiveEnergy >= 120)
                {
                    _playerMortality.ActiveEnergy -= 120;
                    // Instantiate the skill effect at the mouse position
                    Instantiate(skillEffectPrefab, mousePosition, Quaternion.identity);

                    // Set skill on cooldown
                    skillReady = false;
                    timeSinceLastSkillUse = Time.time;
                }
            }
        }

        // Check if cooldown has finished
        if (!skillReady && Time.time - timeSinceLastSkillUse >= skillCooldown)
        {
            skillReady = true; // Skill is ready again
        }

        if (!holdingSkill)
            Destroy(skilldemon);
    }

    public override void Use()
    {
        holdingSkill = !holdingSkill;
        if (holdingSkill)
            skilldemon = Instantiate(skillLayoutVisualization, Input.mousePosition, Quaternion.identity);
    }
}