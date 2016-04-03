﻿using UnityEngine;
using System.Collections;

public class EquipmentManager : Singleton<EquipmentManager> {
	[SerializeField] private EquipmentSlot hatSlot;
	[SerializeField] private EquipmentSlot shoesSlot;
	[SerializeField] private EquipmentSlot powerUpSlot1;
	[SerializeField] private EquipmentSlot powerUpSlot2;

	public EquipmentSlot GetHatSlot() {return hatSlot;}
	public EquipmentSlot GetShoesSlot() {return shoesSlot;}
	public EquipmentSlot GetPowerUp1Slot() {return powerUpSlot1;}
	public EquipmentSlot GetPowerUp2Slot() {return powerUpSlot2;}

	public bool deleteSavesOnDestroy = false;

	private void Awake() {
		MakePersistent();
	}

	private void OnDestroy() {
		if (deleteSavesOnDestroy) DeleteSaves();
	}

	private void DeleteSaves() {
		GetHatSlot().RemoveItem();
		GetShoesSlot().RemoveItem();
		GetPowerUp1Slot().RemoveItem();
		GetPowerUp2Slot().RemoveItem();
	}
}
