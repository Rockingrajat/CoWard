using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
	public enum ItemType {
		None,
		Medicine, //patient
		CommonKit,
		ExpensiveKit,
		DeluxeKit,
		RoomUpgrade,
		Ventilator, //rom
		RoomFailure,
		EquipmentFailure,
		Cylinder, //room
		Bed
	}

	public static int GetCost(ItemType itemType){
		switch(itemType){
			default: return 0;
			case ItemType.Bed: return 100;
			case ItemType.Medicine: return 100;
			case ItemType.CommonKit: return 100;
			case ItemType.ExpensiveKit: return 500;
			case ItemType.DeluxeKit: return 1000;
			case ItemType.RoomUpgrade: return 100;
			case ItemType.Ventilator: return 100;
			case ItemType.Cylinder: return 100;
			case ItemType.RoomFailure: return 200;
			case ItemType.EquipmentFailure: return 100;
		}
	}

	public static Sprite GetSprite(ItemType itemType){
		switch(itemType){
			default: return Resources.Load<Sprite>("item");
			case ItemType.Bed: return Resources.Load<Sprite>("item");
			case ItemType.RoomUpgrade: return Resources.Load<Sprite>("item");
			case ItemType.Medicine: return Resources.Load<Sprite>("medicine");
			case ItemType.CommonKit: return Resources.Load<Sprite>("testkit");
			case ItemType.ExpensiveKit: return Resources.Load<Sprite>("testkit");
			case ItemType.DeluxeKit: return Resources.Load<Sprite>("testkit");
			case ItemType.RoomFailure: return Resources.Load<Sprite>("room_failure");
			case ItemType.EquipmentFailure: return Resources.Load<Sprite>("component_failure");
			case ItemType.Cylinder: return Resources.Load<Sprite>("oxymask");
			case ItemType.Ventilator: return Resources.Load<Sprite>("ventilator");
		}
	}
}