using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
	public static int num_items = 11;
	public enum ItemType {
		None,
		Medicine, //patient
		CommonKit,
		ExpensiveKit,
		DeluxeKit,
		Ventilator, //rom
		RoomFailure,
		EquipmentFailure,
		Cylinder, //room
		AddRoom,
		Aesthetics
	}

	public static int GetCost(ItemType itemType){
		switch(itemType){
			default: return 0;
			case ItemType.AddRoom: return 100;
			case ItemType.Medicine: return 100;
			case ItemType.CommonKit: return 100;
			case ItemType.ExpensiveKit: return 500;
			case ItemType.DeluxeKit: return 1000;
			case ItemType.Ventilator: return 100;
			case ItemType.Cylinder: return 100;
			case ItemType.RoomFailure: return 200;
			case ItemType.EquipmentFailure: return 100;
			case ItemType.Aesthetics: return 100;
		}
	}

	public static Sprite GetSprite(ItemType itemType){
		switch(itemType){
			default: return Resources.Load<Sprite>("item");
			case ItemType.AddRoom: return Resources.Load<Sprite>("item");
			case ItemType.Medicine: return Resources.Load<Sprite>("medicine");
			case ItemType.CommonKit: return Resources.Load<Sprite>("testkit");
			case ItemType.ExpensiveKit: return Resources.Load<Sprite>("testkit");
			case ItemType.DeluxeKit: return Resources.Load<Sprite>("testkit");
			case ItemType.RoomFailure: return Resources.Load<Sprite>("room_failure");
			case ItemType.EquipmentFailure: return Resources.Load<Sprite>("component_failure");
			case ItemType.Cylinder: return Resources.Load<Sprite>("oxymask");
			case ItemType.Ventilator: return Resources.Load<Sprite>("ventilator");
			case ItemType.Aesthetics: return Resources.Load<Sprite>("ventilator");;
		}
	}

	public static string SetInfo(ItemType itemType)
	{
		switch(itemType)
		{
			default: return "Info";
			case ItemType.AddRoom: return "addroom";
			case ItemType.Medicine: return "medicine";
			case ItemType.CommonKit: return "CommonKit";
			case ItemType.ExpensiveKit: return "ExpensiveKit";
			case ItemType.DeluxeKit: return "DeluxeKit";
			case ItemType.Ventilator: return "Ventilator";
			case ItemType.Cylinder: return "Cylinder";
			case ItemType.RoomFailure: return "RoomFailure";
			case ItemType.EquipmentFailure: return "EquipmentFailure";
			case ItemType.Aesthetics: return "Aesthetics";
		}
	}
}