using UnityEngine;
using System.Collections;

public class ListData : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
		
		for(int i=1;i<=40;i++){
			GameObject list = (GameObject)Instantiate(Resources.Load("Item"));
			//list.GetComponentInChildren<UILabel>().text="Name: Product"+i+"\n"+"Price: "+i+".00";
			list.GetComponentsInChildren<UILabel>()[0].text="Name: Product"+i+"\n"+"Price: "+i+".00";
			list.GetComponentsInChildren<UILabel>()[2].text=i.ToString();
			
			list.name = "Item"+i;
			
			list.transform.parent = GameObject.Find("Grid").transform;
			
			GameObject item = GameObject.Find(list.name);
			item.transform.localPosition = new Vector3(0,0,0);
			item.transform.localScale = new Vector3(1,1,1);
			
		}
		
		
		//GetComponent<UIGrid>().repositionNow = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
