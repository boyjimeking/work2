﻿using UnityEngine;
using System.Collections;
using engine;
public class PlayerData : BasePlayerData {
    public static PlayerData instance = new PlayerData();


    ////////used for demo only//////////
    private PetData[] petData;
    public void init() {
        moveSpeed = 4;
        int templateId = 1;
        charTemplate = App.template.getTemp<CharTemplate>(templateId);
        charDataTemplate = App.template.getTemp<CharDataTemplate>(templateId);
        Team team = new Team();
        team.teamNo = Naming.TeamPlayer;
        this.team = team;
        hp = maxhp = charDataTemplate.HP;


        petData = new PetData[2];
        petData[0] = createPetData(5000);
        petData[1] = createPetData(5001);
    }
    public PetData getPetData(int index) {
        if (index >= petData.Length) return null;
        return petData[index];
    }
   
    private PetData createPetData(int templateId) {
        CharTemplate template = App.template.getTemp<CharTemplate>(templateId);
        PetData data = new PetData();
        data.primaryWeaponType = WeaponType.Melee;
        data.moveSpeed = PlayerData.instance.moveSpeed * 0.9f;
        data.attackSpeed = 0.5f;
        data.moveSpeed = template.moveSpeed;
        data.moveRanage = template.moveRange;
        data.weaponRange = template.weaponRange;

        data.charTemplate = App.template.getTemp<CharTemplate>(templateId);
        data.charDataTemplate = App.template.getTemp<CharDataTemplate>(templateId);
        data.hp = data.maxhp = data.charDataTemplate.HP;
        data.team = this.team.clone();
        return data;
    }
}