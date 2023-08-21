using UnityEngine;

[CreateAssetMenu (menuName = "Boss Init/Brain Of Cthulhu")]
public class BOCInit : BossInit
{
    [SerializeField]
    private GameObject floaterPrefab;

    // List of floaters
    private GameObject[] floaters = new GameObject[12];

    override public void BossInitialize()
    {
        // Spawning the floaters
        for (int i = 0; i < 12; i++)
        {
            // Spawn Randomly around brain of cthulhu
            float angle = Random.Range(0f, 360f);
            float x = 5f * Mathf.Cos(angle);
            float y = 5f * Mathf.Sin(angle);
            Vector2 spawnPos = new Vector2(x, y);

            // Spawn around the brain
            GameObject floater = Instantiate(floaterPrefab, new Vector2(transform.position.x, transform.position.y)
                + spawnPos, Quaternion.identity);
            floater.GetComponent<FloaterFSM>().brainTransform = transform;

            // Append to list
            floaters[i] = floater;
        }
    }
}
