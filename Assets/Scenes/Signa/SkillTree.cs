using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{


    private bool isButton1Clicked = false;
    private bool Unlocked1 = false;

    [SerializeField] GameObject inv;

    private InventoryManager invmanager;

    [SerializeField] Item kamehameha;

    [SerializeField] Item HomingSkill;

    [SerializeField] Item Spiritbomb;

    [SerializeField] Item Baseball;

    [SerializeField] Item Boomerang;

    [SerializeField] Item Dash;

    [SerializeField] Item Teleport;

    [SerializeField] Item Eskill;

    [SerializeField] Item Hellzone;


    private void Start()
    {
        invmanager = inv.GetComponent<InventoryManager>();

    }


    public void OnClickButtonHoming()
    {


        if (BossManager.Instance.bossList["Skeletron"].bossDefeated)
        {

            invmanager.Add(HomingSkill.Clone());
        }
    }

    public void OnClickButtonDash()
    {
        if (BossManager.Instance.bossList["EyeOfCthulhu"].bossDefeated)
        {
            invmanager.Add(Dash.Clone());

        }
    }

    public void OnClickButtonTP()
    {

        if (BossManager.Instance.bossList["EyeOfCthulhu"].bossDefeated)
        {
            invmanager.Add(Teleport.Clone());
        }
    }

    public void OnClickButtonESKILL()
    {


        if (BossManager.Instance.bossList["BrainOfCthulhu"].bossDefeated)
        {

            invmanager.Add(Eskill.Clone());
        }
    }

    public void OnClickButtonBOOMERANG()
    {


        if (BossManager.Instance.bossList["BrainOfCthulhu"].bossDefeated)
        {

            invmanager.Add(Boomerang.Clone());
        }
    }

    public void OnClickButtonHellzone()
    {


        if (BossManager.Instance.bossList["QueenBee"].bossDefeated)
        {

            invmanager.Add(Hellzone.Clone());
        }
    }

    public void OnClickButtonBASEBALL()
    {


        if (BossManager.Instance.bossList["QueenBee"].bossDefeated)
        {

            invmanager.Add(Baseball.Clone());
        }
    }

    public void OnClickButtonSPIRITBOMB()
    {


        if (BossManager.Instance.bossList["Plantera"].bossDefeated)
        {

            invmanager.Add(Spiritbomb.Clone());
        }
    }

    public void OnClickButtonKAMEHAMEHA()
    {


        if (BossManager.Instance.bossList["Plantera"].bossDefeated)
        {

            invmanager.Add(kamehameha.Clone());
        }
    }
}

//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;

//public class SkillTree : MonoBehaviour
//{

//    private void Start()
//    {
//        BossManager.Instance.bossList[“EyeOfCthulhu”].isDefeated;
//    }


//}