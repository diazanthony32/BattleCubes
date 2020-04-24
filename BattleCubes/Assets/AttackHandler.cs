﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttackHandler : MonoBehaviour
{
	[SerializeField] GameManager gameManager;
	public string[] attackArray = null;
	public int attackCost = -1;

    // Start is called before the first frame update
    void Start()
    {
    	//gameManager = GameObject.FindGameObjectWithTag("gameManager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StoreAttack(){
    	if(attackArray != null && attackCost != -1){
    		gameManager.AddAction(attackArray, attackCost);
    	}
    	ResetChooseAttackHandlers();
    }

    void ResetChooseAttackHandlers(){
    	//if (PhotonNetwork.LocalPlayer.IsMasterClient) {
	    	for(int i = 0; i < transform.childCount ; i++){
	    		ChooseAttackHandler chooseAttackHandler = transform.GetChild(i).GetComponent<ChooseAttackHandler>();
	    		chooseAttackHandler.ResetHighlights();
	    		chooseAttackHandler.isSelected = false;
				chooseAttackHandler.GetComponent<TweenController>().CancelPulseHighlight();

	    		//chooseAttackHandler.isSelected = false;
	    	}
    	//}
    }

    public void DoAttack(string player, string[] array){

    	//TurnOnTargetting();

    	// if(player == "Host"){
    	// 	UnitInformation unitInfo = GetAttackUnit(player, array[1]);
    	// 	GameObject attackedPlane = GetTargettedPlane(player, array[2], array[3]);

    	// 	if(attackedPlane.transform.childCount > 0){
    	// 		print("You hit a Unit: " + attackedPlane.transform.GetChild(0).name);
    	// 	}
    	// 	else{
    	// 		print("You missed...");
    	// 	}
    	// }
    	// else if(player == "Client"){
    	// 	UnitInformation unitInfo = GetAttackUnit(player, array[1]);
    	// 	GameObject attackedPlane = GetTargettedPlane(player, array[2], array[3]);

    	// 	if(attackedPlane.transform.childCount > 0){
    	// 		print("You hit a Unit: " + attackedPlane.transform.GetChild(0).name);
    	// 	}
    	// 	else{
    	// 		print("You missed...");
    	// 	}
    	// } 

    	UnitInformation unitInfo = GetAttackUnit(player, array[1]);

    	if(unitInfo.targetSystem == "SingleAttack"){
    		GameObject attackedPlane = GetTargettedPlane(player, array[2], array[3]);

    		if(unitInfo.AttackParticle != null){
    			GameObject particle = Instantiate(unitInfo.AttackParticle);
    			particle.transform.position = attackedPlane.transform.position;
				particle.transform.rotation = attackedPlane.transform.rotation;
    		}

    		if(attackedPlane.transform.childCount > 0){
				print("You hit a Unit: " + attackedPlane.transform.GetChild(0).name);
				UnitInformation hitUnitInfo = attackedPlane.transform.GetChild(0).GetComponent<UnitInformation>();
				hitUnitInfo.TakeDamage(unitInfo.attackDmg);
			}
			else{
				print("You missed...");
			}
    	}
    	else if(unitInfo.targetSystem == "TankAttack"){
    		GameObject[] attackedPlanes = GetTankTargettedPlaneArray(player, array[2], array[3]);
    		for(int i = 0; i < 4; i++){
    			if(unitInfo.AttackParticle != null){
	    			GameObject particle = Instantiate(unitInfo.AttackParticle);
	    			particle.transform.position = attackedPlanes[i].transform.position;
					particle.transform.rotation = attackedPlanes[i].transform.rotation;
	    		}

    			if(attackedPlanes[i].transform.childCount > 0){
					print("You hit a Unit: " + attackedPlanes[i].transform.GetChild(0).name);
					UnitInformation hitUnitInfo = attackedPlanes[i].transform.GetChild(0).GetComponent<UnitInformation>();
					hitUnitInfo.TakeDamage(unitInfo.attackDmg);
				}
				else{
					print("You missed...");
				}
    		}
    	}

    	//TurnOffTargetting();
		// attackArray = new string[]{"attack", unitInformation.attackName, hitPlane.transform.parent.name, hitPlane.transform.name};
		//gameManager.enemyCubePosition.transform.GetChild(1).Find(unitInformation.targetSystem).gameObject.SetActive(true);
    	
    }

    public UnitInformation GetAttackUnit(string player, string attackName){
    	
    	if(player == "Host"){

    		// UnitInformation unitInfo;
    		for(int i = 0; i <= 3; i++){

    			UnitInformation unitInfo = null;

    			if(i == 0){
    				unitInfo = Resources.Load<GameObject>(gameManager.cubeInfo[0] + "/" + gameManager.cubeInfo[1] + "/Units/Tower/Prefab" ).GetComponent<UnitInformation>();
    			}
    			else{
    				unitInfo = Resources.Load<GameObject>(gameManager.cubeInfo[0] + "/" + gameManager.cubeInfo[1] + "/Units/"+ i + "/Prefab").GetComponent<UnitInformation>();
    			}

    			if(unitInfo != null && unitInfo.attackName == attackName){
    				return unitInfo;
    			}
    		}
    	}

    	if(player == "Client"){
    		//UnitInformation unitInfo;
    		print("Enemy Cube Info: " + gameManager.enemyCubeInfo[0] + " " + gameManager.enemyCubeInfo[1]);

    		for(int i = 0; i <= 3; i++){
    			
    			UnitInformation unitInfo = null;

    			if(i == 0){
    				unitInfo = Resources.Load<GameObject>(gameManager.enemyCubeInfo[0] + "/" + gameManager.enemyCubeInfo[1] + "/Units/Tower/Prefab" ).GetComponent<UnitInformation>();
    			}
    			else{
    				unitInfo = Resources.Load<GameObject>(gameManager.enemyCubeInfo[0] + "/" + gameManager.enemyCubeInfo[1] + "/Units/"+ i + "/Prefab").GetComponent<UnitInformation>();
    			}

    			if(unitInfo != null && unitInfo.attackName == attackName){
    				return unitInfo;
    			}
    		}
    	}
    	return null;
    }

    GameObject GetTargettedPlane(string player, string planeParent, string plane){

    // 	void HighlightPlane(string parentName, string i){
    // 	GameObject plane = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(parentName).Find(i).gameObject;
    // 	plane.GetComponent<MeshRenderer>().material.color = Color.yellow;
    // 	oldTargets.Add(plane);
    // }
    	GameObject targetPlane = null;
    	if(player == "Host"){
    		targetPlane = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(planeParent).Find(plane).gameObject;
    	}
    	else if(player == "Client"){
    		targetPlane = gameManager.playerCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(planeParent).Find(plane).gameObject;
    	}

    	RaycastHit[] hits;
 		hits = Physics.RaycastAll(targetPlane.transform.position, -targetPlane.transform.up, 0.25f);

 		for (int i = 0; i < hits.Length; i++)
 		{
			//stored info from the enemy's unitPlane underneath
 			RaycastHit hit = hits[i];
 			var hitPlane = hit.transform.gameObject;

			//print(hitPlane.name);
			//this is where we filter out the weird self hits
 			if(hitPlane.tag == "unitSquare")
 			{
 				return hitPlane.gameObject;
 			}

 		}

    	return null;
    }

    GameObject[] GetTankTargettedPlaneArray(string player, string planeParent, string plane){

    // 	void HighlightPlane(string parentName, string i){
    // 	GameObject plane = gameManager.enemyCubePosition.transform.GetChild(1).Find("HighlightAttacks").Find(parentName).Find(i).gameObject;
    // 	plane.GetComponent<MeshRenderer>().material.color = Color.yellow;
    // 	oldTargets.Add(plane);
    // }
    	GameObject targetPlane1 = null;
    	GameObject targetPlane2 = null;
    	GameObject targetPlane3 = null;
    	GameObject targetPlane4 = null;

    	if(plane == "1"){
    		targetPlane1 = GetTargettedPlane(player, planeParent, "1");
    		targetPlane2 = GetTargettedPlane(player, planeParent, "2");
    		targetPlane3 = GetTargettedPlane(player, planeParent, "4");
    		targetPlane4 = GetTargettedPlane(player, planeParent, "5");

    	}
    	else if(plane == "3"){
    		targetPlane1 = GetTargettedPlane(player, planeParent, "2");
    		targetPlane2 = GetTargettedPlane(player, planeParent, "3");
    		targetPlane3 = GetTargettedPlane(player, planeParent, "5");
    		targetPlane4 = GetTargettedPlane(player, planeParent, "6");
    	}
    	else if(plane == "7"){
    		targetPlane1 = GetTargettedPlane(player, planeParent, "4");
    		targetPlane2 = GetTargettedPlane(player, planeParent, "5");
    		targetPlane3 = GetTargettedPlane(player, planeParent, "7");
    		targetPlane4 = GetTargettedPlane(player, planeParent, "8");
    	}
    	else if(plane == "9"){
    		targetPlane1 = GetTargettedPlane(player, planeParent, "5");
    		targetPlane2 = GetTargettedPlane(player, planeParent, "6");
    		targetPlane3 = GetTargettedPlane(player, planeParent, "8");
    		targetPlane4 = GetTargettedPlane(player, planeParent, "9");
    	}

    	GameObject[] targetPlanes = new GameObject[]{targetPlane1, targetPlane2, targetPlane3, targetPlane4};

    	return targetPlanes;
    }



  //   public void TurnOnTargetting(){
		// //print("Turning on targets");

  //   	for(int j = 0; j < gameManager.enemyCubePosition.transform.GetChild(1).childCount-1; j++){
		// 	gameManager.enemyCubePosition.transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
		// 	// targetsystems.SetActive(false);
		// 	//print("turning enemy targets on");
		// }

		// for(int i = 0; i < gameManager.playerCubePosition.transform.GetChild(1).childCount-1; i++){
		// 	gameManager.playerCubePosition.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
		// 	// targetsystems.SetActive(false);
		// 	//print("turning player targets on");

		// }

		// //gameManager.enemyCubePosition.transform.GetChild(1).Find(unitInformation.targetSystem).gameObject.SetActive(true);

  //   }
  //   public void TurnOffTargetting(){
		// //print("Turning off targets");


  //   	for(int j = 0; j < gameManager.enemyCubePosition.transform.GetChild(1).childCount-1; j++){
		// 	gameManager.enemyCubePosition.transform.GetChild(1).GetChild(j).gameObject.SetActive(false);
		// 	//print("turning enemy targets off");
		// 	// targetsystems.SetActive(false);
		// }

		// for(int i = 0; i < gameManager.playerCubePosition.transform.GetChild(1).childCount-1; i++){
		// 	gameManager.playerCubePosition.transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
		// 	// targetsystems.SetActive(false);
		// 	//print("turning player targets off");

		// }

		// //gameManager.enemyCubePosition.transform.GetChild(1).Find(unitInformation.targetSystem).gameObject.SetActive(true);

  //   }

    public bool CheckAttackAvalibility(){
    	return false;
    }
}