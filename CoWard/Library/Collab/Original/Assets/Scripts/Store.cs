using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Store : MonoBehaviour
{
	// public GameObject inventory = GameObject.Find("Inventory");
	[SerializeField] GameObject ShopItemTemplateObj;
	private Transform container;
	private Transform ShopItemTemplate;
	public bool shopOn = false;
	public static IDictionary<string, int> inventory = new Dictionary<string, int>();
	[SerializeField] GameObject mainCamera;
	float width = 430;
	float height = 140;
	IDictionary<string, Text> quantity = new Dictionary<string, Text>();
	public GameObject[] storeitems;
	[SerializeField]
	GameObject commonkitObject, expensivekitObject, deluxekitObject;
	Text commonComponent, expensiveComponent, deluxeComponent;
	int shoplen = 10;

/*	public static Dictionary<string, Item.ItemType> itemFromGameItem = new Dictionary<string, Item.ItemType>()
	{
		{ "Common Kit", Item.ItemType.CommonKit },
		{ "Expensive Kit", Item.ItemType.ExpensiveKit },
		{ "Deluxe Kit", Item.ItemType.DeluxeKit },
		{ "Medicine", Item.ItemType.Medicine },
		{ "Fix Room", Item.ItemType.RoomFailure},
		{"Fix Equipment",  Item.ItemType.EquipmentFailure},
		{"Room Upgrade-1",  Item.ItemType.Cylinder},
		{ "Room Upgrade-2", Item.ItemType.Ventilator}

	};*/

	public static Dictionary<string, string> itemNameFromGameItem = new Dictionary<string, string>()
	{
		{ "Common Kit", "CommonKit"},
		{ "Expensive Kit", "ExpensiveKit" },
		{ "Deluxe Kit", "DeluxeKit" },
		{ "Medicine", "Medicine" },
		{ "Fix Room", "RoomFailure"},
		{"Fix Equipment",  "EquipmentFailure"},
		{"Room Upgrade-1", "Aesthetics" },
		{ "Room Upgrade-2", "Cylinder"},
		{"Room Upgrade-3", "Ventilator" },
		{"Add Room",  "AddRoom"}

	};

	public void ShopButton()
    {
		if(Input.touchCount == 1)
        {
			if (shopOn)
			{
				gameObject.SetActive(false);
				shopOn = false;
			}
			else
			{
				gameObject.transform.SetAsLastSibling();
				gameObject.SetActive(true);
				shopOn = true;
			}

        }
    }

	public void HelpButton(){
		if(Input.touchCount == 1)
		{
			
		}
	}

	private void Awake(){
	}

	public void initializeInventory()
    {
		inventory["AddRoom"] = 0;
		inventory["Aesthetics"] = 0;
		inventory["Medicine"] = 0;
		inventory["CommonKit"] = 0;
		inventory["ExpensiveKit"] = 0;
		inventory["DeluxeKit"] = 0;
		inventory["RoomFailure"] = 0;
		inventory["EquipmentFailure"] = 0;
		inventory["Cylinder"] = 0;
		inventory["Ventilator"] = 0;
	}

	public void initializeStore()
    {
		initializeInventory();
		storeitems = new GameObject[shoplen];

		commonComponent = commonkitObject.GetComponent<Text>();
		deluxeComponent = deluxekitObject.GetComponent<Text>();
		expensiveComponent = expensivekitObject.GetComponent<Text>();
		commonComponent.text = inventory["CommonKit"].ToString();
		expensiveComponent.text = inventory["ExpensiveKit"].ToString();
		deluxeComponent.text = inventory["DeluxeKit"].ToString();
		
		container = transform.Find("container");
		ShopItemTemplate = ShopItemTemplateObj.transform;
		storeitems[0]= CreateItemButton(Item.ItemType.CommonKit, Item.GetSprite(Item.ItemType.CommonKit), "Common Kit", Item.GetCost(Item.ItemType.CommonKit), 0);
		storeitems[1]= CreateItemButton(Item.ItemType.ExpensiveKit, Item.GetSprite(Item.ItemType.ExpensiveKit), "Expensive Kit", Item.GetCost(Item.ItemType.ExpensiveKit), 1);
		storeitems[2]=CreateItemButton(Item.ItemType.DeluxeKit, Item.GetSprite(Item.ItemType.DeluxeKit), "Deluxe Kit", Item.GetCost(Item.ItemType.DeluxeKit), 2);
		storeitems[3]=CreateItemButton(Item.ItemType.Medicine, Item.GetSprite(Item.ItemType.Medicine), "Medicine", Item.GetCost(Item.ItemType.Medicine), 3);
		storeitems[4]=CreateItemButton(Item.ItemType.RoomFailure, Item.GetSprite(Item.ItemType.RoomFailure), "Fix Room", Item.GetCost(Item.ItemType.RoomFailure), 4);
		storeitems[5]=CreateItemButton(Item.ItemType.EquipmentFailure, Item.GetSprite(Item.ItemType.EquipmentFailure), "Fix Equipment", Item.GetCost(Item.ItemType.EquipmentFailure), 5);
		storeitems[6]=CreateItemButton(Item.ItemType.Cylinder, Item.GetSprite(Item.ItemType.Cylinder), "Room Upgrade-2", Item.GetCost(Item.ItemType.Cylinder), 6);
		storeitems[7]=CreateItemButton(Item.ItemType.Ventilator, Item.GetSprite(Item.ItemType.Ventilator), "Room Upgrade-3", Item.GetCost(Item.ItemType.Ventilator), 7);
		storeitems[8]=CreateItemButton(Item.ItemType.Aesthetics, Item.GetSprite(Item.ItemType.Aesthetics), "Room Upgrade-1", Item.GetCost(Item.ItemType.Aesthetics), 8);
		storeitems[9]=CreateItemButton(Item.ItemType.AddRoom, Item.GetSprite(Item.ItemType.AddRoom), "Add Room", Item.GetCost(Item.ItemType.AddRoom), 9);
		ShopItemTemplate.gameObject.SetActive(true);
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	private void Start(){
		
		// gameObject.SetActive(false);
	}
	public void NextButton(){
		
		Text store_button = gameObject.transform.Find("button").GetComponent<Text>();
		if(store_button.text=="Next"){
			store_button.text = "Prev";
			for(int i=0;i<shoplen;i++){
				if(i<6){
					storeitems[i].SetActive(false);
				}
				else{
					storeitems[i].SetActive(true);
				}
			}
		}
		else{
			store_button.text = "Next";
			for(int i=0;i<shoplen;i++){
				if(i<6){
					storeitems[i].SetActive(true);
				}
				else{
					storeitems[i].SetActive(false);
				}
			}
		}

	}
	private GameObject CreateItemButton(Item.ItemType itemType, Sprite sprite, string name, int cost, int p_ind){
		Transform shopItemTransform = Instantiate(ShopItemTemplate,container);
		RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
		int ind = p_ind%6;
		shopItemRectTransform.anchoredPosition = new Vector2(width * (ind % 2), height * (ind / 2)) ;
		shopItemTransform.Find("nameText").GetComponent<Text>().text = name;
		shopItemTransform.Find("Buy").Find("costText").GetComponent<Text>().text = cost.ToString();
		shopItemTransform.Find("Icon").GetComponent<Image>().sprite = sprite;
		quantity[stringFromItem(itemType)] = shopItemTransform.Find("Use").Find("quantity").GetComponent<Text>();
		shopItemTransform.Find("Buy").GetComponent<Button>().onClick.AddListener(() => BoughtItem(itemType));
		GameObject HelpPanel = shopItemTransform.Find("HelpPanel").gameObject;
		HelpPanel.SetActive(false);
		shopItemTransform.Find("HelpButton").GetComponent<Button>().onClick.AddListener(() => ShowHelp(itemType, HelpPanel));		
		Text helpText = HelpPanel.transform.Find("HelpText").GetComponent<Text>();
		helpText.text = Item.SetInfo(itemType);
		
		if((int)itemType <5 && (int)itemType > 0)
        {
			shopItemTransform.GetComponent<itemScript>().toBeAppliedOn = itemScript.ApplyOn.Patient;
        }
		else if((int)itemType >= 5)
        {
			shopItemTransform.GetComponent<itemScript>().toBeAppliedOn = itemScript.ApplyOn.Ward;
		}
		// if(ind>=6){
		// 	shopItemTransform.gameObject.SetActive(false);
		// }
		//Debug.Log(stringFromItem(itemType));
		Updateinventory(itemType, inventory[stringFromItem(itemType)]);
		if (p_ind / 6 > 0)
        {
			shopItemTransform.gameObject.SetActive(false);
		}
		return shopItemTransform.gameObject; 
		// shopItemTransform.Find("Use").GetComponent<Button>().onClick.AddListener(delegate{UseItem(name);});
	}

	public static string stringFromItem(Item.ItemType itemType)
    {
		string obj;
		switch (itemType)
		{
			default:
			case Item.ItemType.AddRoom: obj = "AddRoom"; break;
			case Item.ItemType.Aesthetics: obj = "Aesthetics"; break;
			case Item.ItemType.Medicine: obj = "Medicine"; break;
			case Item.ItemType.CommonKit: obj = "CommonKit"; break;
			case Item.ItemType.ExpensiveKit: obj = "ExpensiveKit"; break;
			case Item.ItemType.DeluxeKit: obj = "DeluxeKit"; break;
			case Item.ItemType.RoomFailure: obj = "RoomFailure"; break;
			case Item.ItemType.EquipmentFailure: obj = "EquipmentFailure"; break;
			case Item.ItemType.Cylinder: obj = "Cylinder"; break;
			case Item.ItemType.Ventilator: obj = "Ventilator"; break;
		}
		return obj;
	}

	Item.ItemType ItemFromString(string itemName)
	{
		Item.ItemType item;
		switch (itemName)
		{
			default:
			case "Aesthetics" : item = Item.ItemType.Aesthetics; break;
			case "AddRoom" : item = Item.ItemType.AddRoom; break;
			case "Medicine": item = Item.ItemType.Medicine; break;
			case "CommonKit": item = Item.ItemType.CommonKit; break;
			case "ExpensiveKit": item = Item.ItemType.ExpensiveKit; break;
			case "DeluxeKit": item = Item.ItemType.DeluxeKit; break;
			case "RoomFailure": item = Item.ItemType.RoomFailure; break;
			case "EquipmentFailure": item = Item.ItemType.EquipmentFailure; break;
			case "Cylinder": item = Item.ItemType.Cylinder; break;
			case "Ventilator": item = Item.ItemType.Ventilator; break;
		}
		return item;
	}

	void Updateinventory(Item.ItemType itemType, int val){

		string obj = stringFromItem(itemType);
		inventory[obj] += val;
		quantity[obj].text = (int.Parse(quantity[obj].text) + val).ToString();
	}

	void BoughtItem(Item.ItemType itemType){
		
		int cost = Item.GetCost(itemType);
		int score = mainCamera.GetComponent<Attributes>().GetScore();
		if(cost<=score){
			//Debug.Log("Bought Item: "+ itemType);
			StartCoroutine(mainCamera.GetComponent<Attributes>().UpdateScore(-cost,0)); 
			//TODO: Update inventory
			Updateinventory(itemType,1);
            switch (itemType)
            {
				case Item.ItemType.CommonKit:
					commonComponent.text = inventory["CommonKit"].ToString();
					break;
				case Item.ItemType.ExpensiveKit:
					expensiveComponent.text = inventory["ExpensiveKit"].ToString();
					break;
				case Item.ItemType.DeluxeKit:
					deluxeComponent.text = inventory["DeluxeKit"].ToString();
					break;
				case Item.ItemType.AddRoom:
					mainCamera.GetComponent<MainScript>().addRoom();
					break;
            }
		}
		else{
			//alert the player that he can't buy
			//Debug.Log("Can't purchase");
			string msg = "Don't have sufficient coins";
			StartCoroutine(mainCamera.GetComponent<MainScript>().notif(msg));
			return;
		}
	}

	void ShowHelp(Item.ItemType itemType, GameObject panel)
	{	
		panel.transform.SetAsLastSibling();
		panel.SetActive(!panel.activeSelf);
	}
	public int UseItem(string itemObj, GameObject appliedOnObject) {//1 is used, 0 is not used.
		
		if (inventory[itemObj] > 0)
        {
            switch (itemObj)
            {
				case "Aesthetics":
                    if (!appliedOnObject.CompareTag("level7"))
                    {
						if(!appliedOnObject.CompareTag("level1")){
							string msg = "Already applied Room Upgrade-1 to "+appliedOnObject.name;
							StartCoroutine(mainCamera.GetComponent<MainScript>().notif(msg));
							return 0;
						}
						appliedOnObject.tag = "level" + ((appliedOnObject.tag[5] - '0') + 1).ToString();
						appliedOnObject.GetComponent<Image>().color = Color.red;
						inventory[itemObj]-=1;
						quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
						return 1;
                    }
					else{
						string msg = appliedOnObject.name+" already upgraded to highest aesthetics";
						StartCoroutine(mainCamera.GetComponent<MainScript>().notif(msg));
						return 0;
					}
				case "Cylinder":
                    if (!appliedOnObject.CompareTag("level7"))
                    {	
						if(appliedOnObject.CompareTag("level1")){
							string msg = "Can't directly apply Room Upgrade-2";
							StartCoroutine(mainCamera.GetComponent<MainScript>().notif(msg));
							return 0;
						}
						if(!appliedOnObject.CompareTag("level2")){
							string msg = "Already applied Room Upgrade-2 to "+appliedOnObject.name;
							StartCoroutine(mainCamera.GetComponent<MainScript>().notif(msg));
							return 0;
						}
						appliedOnObject.tag = "level" + ((appliedOnObject.tag[5] - '0') + 2).ToString();
						appliedOnObject.GetComponent<Image>().color = Color.blue;
						inventory[itemObj]-=1;
						quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
						return 1;
                    }
					else{
						string msg = appliedOnObject.name+" already upgraded to highest aesthetics";
						StartCoroutine(mainCamera.GetComponent<MainScript>().notif(msg));
						return 0;
					}
					
				case "Ventilator":
                    if (!appliedOnObject.CompareTag("level7"))
                    {
						if(appliedOnObject.CompareTag("level1") || appliedOnObject.CompareTag("level2") ){
							string msg = "Can't directly apply Room Upgrade-3";
							StartCoroutine(mainCamera.GetComponent<MainScript>().notif(msg));
							return 0;
						}
						if(!appliedOnObject.CompareTag("level4")){
							string msg = "Already applied Room Upgrade-3 to "+appliedOnObject.name;
							StartCoroutine(mainCamera.GetComponent<MainScript>().notif(msg));
							return 0;
						}
						appliedOnObject.tag = "level" + ((appliedOnObject.tag[5] - '0') + 3).ToString();
						appliedOnObject.GetComponent<Image>().color = new Color(0,1,1,1);
						inventory[itemObj]-=1;
						quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
						return 1;
                    }
					else{
						string msg = appliedOnObject.name+" already upgraded to highest aesthetics";
						StartCoroutine(mainCamera.GetComponent<MainScript>().notif(msg));
						return 0;
					}
				
				case "Medicine":
					inventory[itemObj] -= 1;
					quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
					commonComponent.text = inventory[itemObj].ToString();
					appliedOnObject.GetComponent<Patient>().administer_medicine();
					return 1;
				case "CommonKit":
					inventory[itemObj] -= 1;
					quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
					commonComponent.text = inventory[itemObj].ToString();
					appliedOnObject.GetComponent<Patient>().startTesting(itemObj);
					return 1;
				case "ExpensiveKit":
					inventory[itemObj] -= 1;
					quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
					expensiveComponent.text = inventory[itemObj].ToString();
					appliedOnObject.GetComponent<Patient>().startTesting(itemObj);
					return 1;
				case "DeluxeKit":
					inventory[itemObj] -= 1;
					quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
					deluxeComponent.text = inventory[itemObj].ToString();
					appliedOnObject.GetComponent<Patient>().startTesting(itemObj);
					return 1;
				case "EquipmentFailure":

					if(appliedOnObject.transform.Find("Text").GetComponent<Text>().text!=""){
						inventory[itemObj] -= 1;
						quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
						Patient[] patients2 = appliedOnObject.transform.GetComponentsInChildren<Patient>();
						foreach (Patient pat in patients2)
						{
							pat.prob_sev_dec_base = 0.1f;
							pat.prob_sev_inc = 0.07f;
						}
						appliedOnObject.transform.Find("Text").GetComponent<Text>().text = "";
						return 1;
					}
					return 0;
				case "RoomFailure":
					if(appliedOnObject.transform.Find("Text").GetComponent<Text>().text!=""){
						inventory[itemObj] -= 1;
						quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();

						Patient[] patients = appliedOnObject.transform.GetComponentsInChildren<Patient>();
						foreach (Patient pat in patients)
						{
							pat.prob_sev_dec_base = 0.1f;
							pat.prob_sev_inc = 0.07f;
						}
						appliedOnObject.transform.Find("Text").GetComponent<Text>().text = "";
						return 1;
					}
					return 0;
				case "AddRoom":
					inventory[itemObj] -= 1;
					quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
					

					return 1;
				default:
					inventory[itemObj] -= 1;
					quantity[itemObj].text = (int.Parse(quantity[itemObj].text) - 1).ToString();
					return 1;
			}

        }
        else
        {
			return 0;
        }


		
		// Updateinventory(itemType,-1);
	}
	// 	GameObject myTextgameObject = GameObject.Find("Score"); 

	// 	Text ourComponent = myTextgameObject.GetComponent<Text>();
	// 	if(demand.cost<=ourComponent.text){
	// 		ourComponent.text = ourComponent.text - demand.cost;

	// 		if(Inventory.contains(demand.name))
	// 		inventory.inventory[demand.name]+=1;
	// 		else
	// 		inventory.inventory[demand.name] = 1;

	// 		Debug.Log("Score reduced to");
	// 	}
	// }

}