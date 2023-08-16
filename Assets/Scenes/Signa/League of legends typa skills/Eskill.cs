using UnityEngine;

public class Eskill : MonoBehaviour
{
    public GameObject skillLayoutVisualization; // Assign the visualization sprite/UI element in the Inspector
    public GameObject skillEffectPrefab; // Prefab of the skill's effect (damage area, particles, etc.)

    private bool holdingSkill = false;
    private bool skillReady = true; // Indicates if the skill is ready to be used
    public float skillCooldown = 5.0f; // Cooldown time in seconds

    private float timeSinceLastSkillUse = Mathf.Infinity; // Initialize with a very large value to allow immediate use

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && skillReady)
        {
            // Display the skill layout visualization
            holdingSkill = true;
            skillLayoutVisualization.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            // Hide the skill layout visualization
            holdingSkill = false;
            skillLayoutVisualization.SetActive(false);
        }

        if (holdingSkill)
        {
            // Update the skill layout visualization position to follow the mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            skillLayoutVisualization.transform.position = mousePosition;

            if (Input.GetMouseButtonDown(0) && skillReady)
            {
                // Player clicked while holding the skill button and skill is ready
                // Instantiate the skill effect at the mouse position
                Instantiate(skillEffectPrefab, mousePosition, Quaternion.identity);

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
}




