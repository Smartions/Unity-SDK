using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIStateManager : MonoBehaviour {
	
	public static GUIStateManager instance;
	
	public Texture2D shareData; //Sina Weibo, Tencent Weibo and RenRen Sharing

	public Texture2D wechatData;// WeChat Sharing
	
	public GameObject m_resultTrigger;
	public UILabel resultLabel;

	// Use this for initialization
	void Start () {
		instance = this;
		DontDestroyOnLoad(gameObject);

	}
	

	public byte[] encode(){
		return shareData.EncodeToPNG();
	}

	public byte[] wechatEncode(){
		return wechatData.EncodeToPNG();
	}
	
	public void allShare(){
		SinoMoGoConnect.shareAction("This is test 'SinoMoGoConnect Demo'.",encode(),wechatEncode(),SNSType.ALL, "http://www.sinomogo.com");
	}
	
	public void sinaShare(){
		SinoMoGoConnect.shareAction("This is test 'SinoMoGoConnect Demo'.",encode(),null,SNSType.SINA,null);
	}
	
	public void tencentShare(){
		SinoMoGoConnect.shareAction("This is test 'SinoMoGoConnect Demo'.",encode(),null,SNSType.TENCENT,null);
	}
	
	public void renrenShare(){
		SinoMoGoConnect.shareAction("This is test 'SinoMoGoConnect Demo'.",encode(),null,SNSType.RENREN,null);
	}
	
	public void friendShare(){
		SinoMoGoConnect.shareAction("This is test 'SinoMoGoConnect Demo'.",encode(),wechatEncode(),SNSType.FRIEND,"http://www.sinomogo.com");
	}
	
	public void groupShare(){
		SinoMoGoConnect.shareAction("This is test 'SinoMoGoConnect Demo'.",encode(),wechatEncode(),SNSType.GROUP,"http://www.sinomogo.com");
	}
	
	/**
	 * Countly Event,Event key
	 */
	public void postEventFirst(){
		SinoMoGoCountly.recordEvent("Event1");
	}
	
	/**
	 * Countly Event, Event key and sum
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
	
	public void showResult(string sharetype,bool flg){
		string message="";
		
		if(sharetype!=null){
			
			ShareType type = (ShareType)System.Enum.Parse(typeof(ShareType),sharetype,false);
			switch(type){
				case ShareType.sina:
					message="Sina";
					break;
				case ShareType.tencent:
					message="Tencent";
					break;
				case ShareType.renren:
					message="Renren";
					break;
				case ShareType.friend:
					message="Wechat friend";
					break;
				case ShareType.group:
					message="Wechat moment";
					break;
				default:
					break;
			}
			
		}
		
		
		message +=" Sharing " + ( flg ? "Successful" : "Failed" )+ ".";
		resultLabel.text = message;
		m_resultTrigger.SendMessage("OnClick");
		
	}
}
