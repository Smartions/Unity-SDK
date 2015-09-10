using UnityEngine;

public class SinoMoGoConnect
{
	private static AndroidJavaObject _plugin;

	static SinoMoGoConnect()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.smartions.sinomogo.connect.plugin.SinoMoGoConnectPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}

	
	/**
	 * Display all sharing options
	 */
	public static void shareAction(string content,byte[] data,byte[] wechatData,string shareType,string url){
		
		toShare(shareType,data,wechatData,new string[]{content,url});
		
	}
	
	private static void toShare(string type,byte[] data,byte[] wechatData,string[] param){
		if( Application.platform != RuntimePlatform.Android )
			return;
		_plugin.Call("share",type,data,wechatData,param);
	}
	

}


