using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FightingUIController
{
    public enum Attacker { unit1 = 0, unit2 = 1 };
    const int numOfAlphaSteps = 50;
    const int textSpeed = 1;

    private Unit unit1, unit2;
    private GameObject sword1, sword2;
    private Vector3[] textPos = new Vector3[2];

    static readonly Vector3 transformVec = new Vector3(0.0f, 0.7f, -2.0f);
    static readonly Vector3 textMove = new Vector3(0.0f, 0.5f, 0.0f);

    public FightingUIController(Unit u1, Unit u2)
    {
        unit1 = u1;
        unit2 = u2;

        sword1 = GameObject.Instantiate(GameData.FightingSwordSprite);
        sword1.transform.position = unit1.transform.position + transformVec;
        textPos[0] = sword1.transform.position + textMove;

        sword2 = GameObject.Instantiate(GameData.FightingSwordSprite);
        sword2.transform.position = unit2.transform.position + transformVec;
        textPos[1] = sword2.transform.position + textMove;
    }

    public void PlayerAttack(Attacker Attacker, float damage)
    {
        GameObject.Instantiate(GameData.TextRenderer).GetComponent<FightTextController>()
            .Init(damage.ToString(), textPos[(int)Attacker], numOfAlphaSteps, textSpeed);
        damage.ToString();
    }

    public void FightingEnd(bool u1Killed, bool u2Killed)
    {
        sword1.SetActive(false);
        sword2.SetActive(false);
        GameObject.Destroy(sword1);
        GameObject.Destroy(sword2);
    }

}
