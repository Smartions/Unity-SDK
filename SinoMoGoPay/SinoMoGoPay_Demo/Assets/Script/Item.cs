using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	

	void OnClick(){
		string productID = "com.smartions.sinomogo.test.product" + GetComponentsInChildren<UILabel>()[2].text;
		string inputCount = GameObject.FindGameObjectWithTag("Input").GetComponent<UIInput>().value;
		int count = 1;
		if(!"".Equals(inputCount)){
			count = int.Parse(inputCount);
		}
		
		SinoMoGoPay.purchase(productID,count);
		

	}
	
}
