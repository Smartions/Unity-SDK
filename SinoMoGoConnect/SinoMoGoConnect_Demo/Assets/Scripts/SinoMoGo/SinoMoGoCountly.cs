using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SinoMoGoCountly
{
	private static AndroidJavaObject _plugin;

	static SinoMoGoCountly()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.smartions.sinomogo.countly.plugin.SinoMoGoCountlyPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}
	
	
	public static void recordEvent(string eventName){
		_plugin.Call("postEvent",eventName);
	}
	
	
	public static void recordEvent(string eventName,double sum){
		_plugin.Call("postEventForSum",eventName,sum);
	}
	
	public static void recordEvent(string eventName,string[] keys,string[] values){
		
		
		postEvent(eventName,keys,values,0);
	}
	
	public static void recordEvent(string eventName,string[] keys,string[] values,double sum){
		
		
		postEvent(eventName,keys,values,sum);
	}
	
	
	
	private static void postEvent(string eventName,string[] keys,string[] values,double sum){
		if(Application.platform != RuntimePlatform.Android)
			return;
		if(keys == null || values == null){
			return;
		}
		
		Dictionary<string,string> eventDictionary = new Dictionary<string, string>();
		if(keys.Length != values.Length){
			Debug.Log("SinoMoGoCountly event "+ eventName + " has mismatch number of keys " + keys.Length + " & values" + values.Length);
			return;
		}
		for(int i=0; i < keys.Length; i++){
			eventDictionary.Add(keys[i],values[i]);
		}
		
		if(sum == 0){
			_plugin.Call("postEventSegmentation",eventName,eventDictionary.toSinoMoGoJson());
		}else{
			_plugin.Call("postEventSegmentationForSum",eventName,eventDictionary.toSinoMoGoJson(),sum);	
		}
		
		
	}
	

	
}


