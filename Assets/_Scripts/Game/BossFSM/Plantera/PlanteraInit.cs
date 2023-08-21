using UnityEngine;

[CreateAssetMenu (menuName = "Boss Init/Plantera")]
public class PlanteraInit : BossInit
{
    [SerializeField]
    private GameObject hookPrefab;

    // Hooks' variables
    private GameObject[] hooks = new GameObject[8];

    override public void BossInitialize()
    {
        // Init hooks
        for (int i = 0; i < 8; i++)
        {
            // Spawn Randomly around Plantera
            float angle = Random.Range(0f, 360f);
            float x = 7f * Mathf.Cos(angle);
            float y = 7f * Mathf.Sin(angle);
            Vector2 spawnPos = new Vector2(x, y);

            // Spawn the hooks around plantera
            GameObject hook = Instantiate(hookPrefab, new Vector2(transform.position.x, transform.position.y)
                + spawnPos, Quaternion.identity);
            hook.GetComponent<PlanteraHookFSM>().planteraTransform = transform;

            // Append to list
            hooks[i] = hook;
        }
    }
}
