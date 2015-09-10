using UnityEngine;

public class SinoMoGoPay
{
	private static AndroidJavaObject _plugin;

	static SinoMoGoPay()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.smartions.sinomogo.pay.plugin.SinoMoGoPayPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}

	/**
	 * 设置背景音乐
	 * setting the background music
	 */
	public static void forBackgroundMusic(){
		if( Application.platform != RuntimePlatform.Android )
			return;
		_plugin.Call("forBackgroundMusic");
	}


	/**
	 * 是否显示第三方支付平台菜单 
	 */
	public static bool isShow(){
		if( Application.platform != RuntimePlatform.Android )
			return false;
		return _plugin.Call<bool>("isShow");
	}

	/**
	 * 菜单按钮触发事件 
	 */
	public static void callShow(){
		if( Application.platform != RuntimePlatform.Android )
			return;
		_plugin.Call("callShow");
	}


	/**
	 * 程序退出 
	 * exit application
	 */
	public static void exit(){
		if( Application.platform != RuntimePlatform.Android )
			return;
		_plugin.Call("exitApplication");
	}


	
	/**
	 * Show the page of SinoMoGo Pay 
	 */
	public static void purchase(string productId,int count){
		if( Application.platform != RuntimePlatform.Android )
			return;
		_plugin.Call("purchase",productId,count);
		
	}

}


