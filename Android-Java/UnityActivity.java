package com.growthpush;

import android.os.Bundle;
import android.util.Log;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;

import com.growthpush.handler.DefaultReceiveHandler;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

@SuppressLint("NewApi")
public class UnityActivity extends UnityPlayerActivity {
	
	private static String growthPushMessage = null;
    
	private boolean started = false;
    
	public static void saveGrowthPushMessage(String msg)
	{
		growthPushMessage = msg;
	}
	
	public static void callTrackGrowthPushMessage()
	{
		if(growthPushMessage != null)
		{
			UnityPlayer.UnitySendMessage("GrowthPushReceiveAndroid", "LaunchWithNotification", growthPushMessage);
			growthPushMessage = null;
		}
	}
	
	
	public static String parsePushGrowthPushMessage(Intent intent)
	{
		String str = null;
		Bundle bundle = null;
		if(intent != null)
			bundle = intent.getExtras();
		
		if(bundle != null)
		{
			str = "";
			for (String key : bundle.keySet()) {
				String value = bundle.get(key).toString();
		        Log.d("unity", key + " => " + value);				        
		        if(key.equals("showDialog") || key.equals("collapse_key") || key.equals("from"))
		        	continue;
				str += String.format("&%s=%s", key, value);
			}
            
		}
		
		return str;
	}
	
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		Log.d("unity", "onCreate");
		
		DefaultReceiveHandler receiveHandler = new DefaultReceiveHandler() {
			@Override
			public void onReceive(Context context, Intent intent) {
				Log.d("unity", "onReceive");
				super.onReceive(context, intent);
			}
		};
		receiveHandler.setCallback(new DefaultReceiveHandler.Callback() {

			@Override
			public void onOpen(Context context, Intent intent) {

				Log.d("unity", "onOpen");
				super.onOpen(context, intent);
				String str = UnityActivity.parsePushGrowthPushMessage(intent);
				if(str != null)
				{
					UnityPlayer.UnitySendMessage("GrowthPushReceiveAndroid", "LaunchWithNotification", str);
				}
                
			}

		});
		GrowthPush.getInstance().setReceiveHandler(receiveHandler);

	}

	@Override
	protected void onStart ()
	{
		Log.d("unity", "onStart");
		super.onStart();
		GrowthPush.getInstance().trackEvent("Launch");
		started = true;
	}
	
	@Override
	protected void onResume()
	{
		Log.d("unity", "onResume");
		super.onResume();
		if(!started)
			GrowthPush.getInstance().trackEvent("Launch");
		started = false;
	}
}

