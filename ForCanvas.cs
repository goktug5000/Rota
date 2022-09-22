using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForCanvas : MonoBehaviour
{

    public GameObject panel, envPanel, LifeSkillPanel,CombatSkillPanel,ekEnvPanel,blade,inWarImage,aciklamaPanel;
    private bool panelB,envPanelB,LifeSkillPanelB,CombatSkillPanelB,ekEnvB;
    public bool atCombatB;
    public float atCombatCD;


    private void Start()
    {
        panelB = envPanelB = LifeSkillPanelB = CombatSkillPanelB = ekEnvB = false;
        panel.SetActive(false);
        envPanel.SetActive(false);
        LifeSkillPanel.SetActive(false);
        CombatSkillPanel.SetActive(false);
        ekEnvPanel.SetActive(false);
        inWarImage.SetActive(false);
        aciklamaPanel.SetActive(false);
    }
    public void envBut()
    {
        envPanelB = !envPanelB;
        envPanel.SetActive(envPanelB);
    }
    public void LifeSkillBut()
    {
        LifeSkillPanelB = !LifeSkillPanelB;
        CombatSkillPanelB = false; 

        LifeSkillPanel.SetActive(LifeSkillPanelB);
        CombatSkillPanel.SetActive(CombatSkillPanelB);
    }
    public void CombatSkillBut()
    {
        LifeSkillPanelB = false;
        CombatSkillPanelB = !CombatSkillPanelB;

        LifeSkillPanel.SetActive(LifeSkillPanelB);
        CombatSkillPanel.SetActive(CombatSkillPanelB);
    }
    public void EkEnvBut()
    {
        ekEnvB = !ekEnvB;
        ekEnvPanel.SetActive(ekEnvB);
    }
    void allFalse()
    {
        panelB = false;
        panel.SetActive(panelB);


        envPanelB = false;
        LifeSkillPanelB = false;
        CombatSkillPanelB = false;
        ekEnvB = false;

        envPanel.SetActive(envPanelB);
        LifeSkillPanel.SetActive(LifeSkillPanelB);
        CombatSkillPanel.SetActive(CombatSkillPanelB);
        ekEnvPanel.SetActive(ekEnvB);

        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            panelB = !panelB;
            aciklamaPanel.SetActive(false);
            panel.SetActive(panelB);
        }



        if (atCombatCD <= 0)
        {
            atCombatB = false;
            blade.GetComponent<Blade>().atCombatB = false;
            inWarImage.SetActive(false);
        }
        else
        {
            atCombatCD -= Time.deltaTime;
            atCombatB = true;
            blade.GetComponent<Blade>().atCombatB = true;
            inWarImage.SetActive(true);
            allFalse();
        }

    }


    public void intoToWar(float secForWar)
    {
        if(atCombatCD<=secForWar)
            atCombatCD = secForWar;
    }
}
