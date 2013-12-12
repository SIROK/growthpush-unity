using UnityEngine;
using System.Collections;
using System;


public class GrowthPushAndroid
{
	public enum Environment 
	{
		production, 
		development
	};
	
	private static GrowthPushAndroid instance = null;	
	
	public static GrowthPushAndroid getInstance() 
	{
		if(instance == null)
			instance = new GrowthPushAndroid();
		return instance;
	}
#if UNITY_ANDROID && !UNITY_EDITOR	
	private AndroidJavaObject growthPush = null;	
#endif
	
	private GrowthPushAndroid()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		using(AndroidJavaClass gpclass = new AndroidJavaClass( "com.growthpush.GrowthPush" ))
		{
			growthPush = gpclass.CallStatic<AndroidJavaObject>("getInstance");
		}
#endif
	}

	public GrowthPushAndroid initialize(int applicationId, string secret) 
	{
		return initialize(applicationId, secret, Environment.production, false);
	}

	public GrowthPushAndroid initialize(int applicationId, string secret, Environment environment) 
	{
		return initialize(applicationId, secret, environment, false);
	}

	public GrowthPushAndroid initialize(int applicationId, string secret, Environment environment, bool debug) 
	{		
#if UNITY_ANDROID && !UNITY_EDITOR
		if( growthPush != null )
		{
			AndroidJavaClass enviClassJava = new AndroidJavaClass("com.growthpush.model.Environment");
			AndroidJavaObject enviObjJava = enviClassJava.GetStatic<AndroidJavaObject>(environment == Environment.production ? "production" : "development");
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        	AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			growthPush.Call<AndroidJavaObject>("initialize", activity, applicationId, secret, enviObjJava, debug);
		}
		else
		{
			Debug.LogError( "growthPush is not created.");
		}
#endif
		return this;
	}
	
	public GrowthPushAndroid register(string senderId) 
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if( growthPush != null )
		{
			growthPush.Call<AndroidJavaObject>("register", senderId);
		}
		else
		{
			Debug.LogError( "growthPush is not created.");
		}
#endif
		return this;
	}
	
	public void trackEvent(string name) 
	{
		trackEvent(name, "");
	}
	
	public void trackEvent(string name, string val) 
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if( growthPush != null )
		{
			growthPush.Call("trackEvent", name, val);
		}
		else
		{
			Debug.LogError( "growthPush is not created.");
		}
#endif
	}
	
	public void setTag(string name) 
	{
		setTag(name, "");
	}
		
	public void setTag(string name, string val) 
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if( growthPush != null )
		{
			growthPush.Call("setTag", name, val);
		}
		else
		{
			Debug.LogError( "growthPush is not created.");
		}
#endif
	}
		
	public void setDeviceTags() 
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		if( growthPush != null )
		{
			growthPush.Call("setDeviceTags");
		}
		else
		{
			Debug.LogError( "growthPush is not created.");
		}
#endif
	}
	
	public void callTrackGrowthPushMessage()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		using(AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
    		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			if(activity != null)
				activity.CallStatic("callTrackGrowthPushMessage");
		}
#endif
	}
}




