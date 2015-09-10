using UnityEngine;
using System.Collections;

public class GUIStateManager: MonoBehaviour {

	public static GUIStateManager instance;
	
	public GameObject m_resultInTrigger;
	
	public UILabel resultLabel;
	

	void Start(){
		instance = this;
		DontDestroyOnLoad(gameObject);
		
	}
	

	
	/**
	 * 显示支付结果信息
	 * show the info of payment result 
	 */
	public void showResult(string payway,bool flg,string productid){
		string message = "";
		
		if(payway!=null && !payway.Equals("")){
			
			PayWay way = (PayWay)System.Enum.Parse(typeof(PayWay),payway,false);
			switch(way){
				case PayWay.alipayclient:
					message = "AlipayClient";
					break;

				case PayWay.unionpay:
					message = "UnionPay";
					break;

				default:
					break;
			}
			
		}
		
		
		message +=" Payment "+ ( flg ? "Successful" : "Failed" ) +"." + "   PID:"+productid;

		resultLabel.text = message;
		m_resultInTrigger.SendMessage("OnClick");
		
	}



	
	/**
	 * Analytics Event,Event key
	 */
	public void postEventFirst(){
		SinoMoGoCountly.recordEvent("Event1");

	}
	
	/**
	 * Analytics Event, Event key and sum
	 */
	public void postEventSecond(){
		SinoMoGoCountly.recordEvent("Event2",0.1);
	}
	
	/**
	 * Event key with segmentation(s) 
	 */
	public void postEventThree(){
		SinoMoGoCountly.recordEvent("Event3",new string[]{"list1","list2","list3"},new string[]{"item1","item2","item3"});
	}
	
	/**
	 * Event key and sum with segmentation(s) 
	 */
	public void postEventFourth(){
		SinoMoGoCountly.recordEvent("Event4",new string[]{"list1","list2","list3"},new string[]{"item1","item2","item3"},10);
	}
	
}
