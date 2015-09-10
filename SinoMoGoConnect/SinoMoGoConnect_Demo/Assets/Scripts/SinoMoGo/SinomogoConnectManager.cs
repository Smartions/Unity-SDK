using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SinomogoConnectManager : MonoBehaviour
{
	public static SinomogoConnectManager instance;
	
	void Start(){
		instance = this;
		
		DontDestroyOnLoad(gameObject);
		
	}

	
	/**
	 * SHARE_FLAG 分享结果("true" if the sharing was successful or "false" if it failed.)
	 * SHARE_TYPE 分享方式(the sharing platforms) 
	 */
	void ShareResult(string result){
		IDictionary item = (IDictionary)SinoMoGoJson.SinoMoGoDeserialize(result);
		bool resultflag = Convert.ToBoolean(item["SHARE_FLAG"]);
		string sharetype = item["SHARE_TYPE"]!=null ? Convert.ToString(item["SHARE_TYPE"]) : null;//判断分享方式是否为空
		Debug.Log("-->resultflag:"+ resultflag+",sharetype:"+sharetype);
		
		
		//true:Sharing Successful
		//false:Sharing Failed
		GUIStateManager.instance.showResult(sharetype,resultflag);
		
		
	}

}

