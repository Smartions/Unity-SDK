using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SinomogoPayManager : MonoBehaviour
{
	public static SinomogoPayManager instance;

	void Start(){
		instance = this;
		
		DontDestroyOnLoad(gameObject);

	}

	
	/**
	 *  RESULT_FLAG 支付结果 (Result flag which indicates if the transaction is successful)
	 *  PAY_WAY 支付方式 (Which payment option the end user selected to use)
	 *  PRODUCT_ID 产品ID (Product ID of the product purchased)
	 */
	void PayResult(string result){

		
		IDictionary item =  (IDictionary)SinoMoGoJson.SinoMoGoDeserialize(result);
		bool resultflag = Convert.ToBoolean(item["RESULT_FLAG"]);
		string productid = Convert.ToString(item["PRODUCT_ID"]);
		
		string payway = item["PAY_WAY"]!=null ? Convert.ToString(item["PAY_WAY"]) : null;//判断支付方式是否为空
			
		
		Debug.Log("-->resultflg:"+resultflag+",payway:"+payway+",id:"+productid);
		
		//true:Transaction Successfully
		//false:Transaction failure
		GUIStateManager.instance.showResult(payway,resultflag,productid);
		
	}

	
}

