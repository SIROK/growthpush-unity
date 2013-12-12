using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

public class GrowthPush
{
	public enum Environment
	{
		unknown = 0,
		development,
		production
	};
	
	public enum Option
	{
		None = GrowthPushIOS.EGPOption.EGPOptionNone,
		TrackLaunch = GrowthPushIOS.EGPOption.EGPOptionTrackLaunch,
		TagDevice = GrowthPushIOS.EGPOption.EGPOptionTagDevice,
		TagOS = GrowthPushIOS.EGPOption.EGPOptionTagOS,
		TagLanguage = GrowthPushIOS.EGPOption.EGPOptionTagLanguage,
		TagTimeZone = GrowthPushIOS.EGPOption.EGPOptionTagTimeZone,
		TagVersion = GrowthPushIOS.EGPOption.EGPOptionTagVersion,
		TagBuild = GrowthPushIOS.EGPOption.EGPOptionTagBuild,
		TrackAll = GrowthPushIOS.EGPOption.EGPOptionTrackAll,
		TagAll = GrowthPushIOS.EGPOption.EGPOptionTagAll,
		All = GrowthPushIOS.EGPOption.EGPOptionAll,
	};
	
	public static void initialize(int applicationId, string secret)
	{
		initialize(applicationId, secret, Environment.production);
	}
	
	public static void initialize(int applicationId, string secret, Environment environment)
	{
		initialize(applicationId, secret, environment, Option.All);
	}
	
	public static void initialize(int applicationId, string secret, Environment environment, Option option)
	{
		initialize(applicationId, secret, environment, Option.All, false);
	}
	
	public static void initialize(int applicationId, string secret, Environment environment, Option option, bool debug)
	{
#if UNITY_ANDROID
		GrowthPushAndroid.Environment environmentAndroid = GrowthPushAndroid.Environment.development;
		if(environment == Environment.production)
			environmentAndroid = GrowthPushAndroid.Environment.production;		
		GrowthPushAndroid.getInstance().initialize(applicationId, secret, environmentAndroid, debug); 
#elif UNITY_IPHONE
		GrowthPushIOS.setApplicationId(applicationId, secret, (GrowthPushIOS.GPEnvironment)environment, debug);
#endif
	}
	
	public static void register()
	{
		register("");
	}
	
	public static void register(string senderId)
	{
#if UNITY_ANDROID
		GrowthPushAndroid.getInstance().register(senderId);
#elif UNITY_IPHONE
		GrowthPushIOS.requestDeviceToken(deviceToken => {
			if (deviceToken != null && deviceToken.Length != 0) 
			{
				GrowthPushIOS.setDeviceToken(deviceToken);
			}
		});
#endif
	}
	
	public static void trackEvent(string name)
	{
		trackEvent(name, "");
	}
	
	public static void trackEvent(string name, string val)
	{
#if UNITY_ANDROID
		GrowthPushAndroid.getInstance().trackEvent(name, val);
#elif UNITY_IPHONE
		GrowthPushIOS.trackEvent(name, val);
#endif
	}

	public static void setTag(string name)
	{
		setTag(name, "");
	}
	
	public static void setTag(string name, string val)
	{
#if UNITY_ANDROID
		GrowthPushAndroid.getInstance().setTag(name, val);
#elif UNITY_IPHONE
		GrowthPushIOS.setTag(name, val);
#endif
	}
	
	public static void requestDeviceToken()
	{
#if UNITY_IPHONE
		GrowthPushIOS.requestDeviceToken(null);
#endif
	}
	
  	public static void setDeviceToken(string deviceToken)
	{
#if UNITY_IPHONE
		GrowthPushIOS.setDeviceToken(deviceToken);
#endif
	}
	
  	public static void setDeviceTags()
	{
#if UNITY_ANDROID
		GrowthPushAndroid.getInstance().setDeviceTags();
#elif UNITY_IPHONE
		GrowthPushIOS.setDeviceTags();
#endif
	}
	
  	public static void clearBadge()
	{
#if UNITY_IPHONE
		GrowthPushIOS.clearBadge();
#endif
	}
	
	public static void launchWithNotification(Action<Dictionary<string, object>> callback)
	{
		GrowthPushReceive receive = GrowthPushReceive.CreateGO();
		if(receive != null)
			receive.launchWithNotificationCallback = callback;
#if UNITY_IPHONE
		GrowthPushIOS.callTrackGrowthPushMessage();
#elif UNITY_ANDROID
		GrowthPushAndroid.getInstance().callTrackGrowthPushMessage();
#endif
	}
}
