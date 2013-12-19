using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;

public class GrowthPush
{
	public enum Environment
	{
		Unknown = 0,
		Development,
		Production
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
	
	public static void Initialize(int applicationId, string secret)
	{
		Initialize(applicationId, secret, Environment.Production);
	}
	
	public static void Initialize(int applicationId, string secret, Environment environment)
	{
		Initialize(applicationId, secret, environment, Option.All);
	}
	
	public static void Initialize(int applicationId, string secret, Environment environment, Option option)
	{
		Initialize(applicationId, secret, environment, Option.All, false);
	}
	
	public static void Initialize(int applicationId, string secret, Environment environment, Option option, bool debug)
	{
#if UNITY_ANDROID
		GrowthPushAndroid.Environment environmentAndroid = GrowthPushAndroid.Environment.Development;
		if(environment == Environment.Production)
			environmentAndroid = GrowthPushAndroid.Environment.Production;		
		GrowthPushAndroid.GetInstance().Initialize(applicationId, secret, environmentAndroid, debug); 
#elif UNITY_IPHONE
		GrowthPushIOS.SetApplicationId(applicationId, secret, (GrowthPushIOS.GPEnvironment)environment, debug);
#endif
	}
	
	public static void Register ()
	{
		Register("");
	}
	
	public static void Register(string senderId)
	{
#if UNITY_ANDROID
		GrowthPushAndroid.GetInstance().Register(senderId);
#elif UNITY_IPHONE
		GrowthPushIOS.RequestDeviceToken(deviceToken => {
			if (deviceToken != null && deviceToken.Length != 0) 
			{
				GrowthPushIOS.SetDeviceToken(deviceToken);
			}
		});
#endif
	}
	
	public static void TrackEvent(string name)
	{
		TrackEvent(name, "");
	}
	
	public static void TrackEvent(string name, string val)
	{
#if UNITY_ANDROID
		GrowthPushAndroid.GetInstance().TrackEvent(name, val);
#elif UNITY_IPHONE
		GrowthPushIOS.TrackEvent(name, val);
#endif
	}

	public static void SetTag(string name)
	{
		SetTag(name, "");
	}
	
	public static void SetTag(string name, string val)
	{
#if UNITY_ANDROID
		GrowthPushAndroid.GetInstance().SetTag(name, val);
#elif UNITY_IPHONE
		GrowthPushIOS.SetTag(name, val);
#endif
	}
	
	public static void RequestDeviceToken()
	{
#if UNITY_IPHONE
		GrowthPushIOS.RequestDeviceToken(null);
#endif
	}
	
  	public static void SetDeviceToken(string deviceToken)
	{
#if UNITY_IPHONE
		GrowthPushIOS.SetDeviceToken(deviceToken);
#endif
	}
	
  	public static void SetDeviceTags()
	{
#if UNITY_ANDROID
		GrowthPushAndroid.GetInstance().SetDeviceTags();
#elif UNITY_IPHONE
		GrowthPushIOS.SetDeviceTags();
#endif
	}
	
  	public static void ClearBadge()
	{
#if UNITY_IPHONE
		GrowthPushIOS.ClearBadge();
#endif
	}
	
	public static void LaunchWithNotification(Action<Dictionary<string, object>> callback)
	{
		GrowthPushReceive receive = GrowthPushReceive.CreateGO();
		if(receive != null)
			receive.LaunchWithNotificationCallback = callback;
#if UNITY_IPHONE
		GrowthPushIOS.callTrackGrowthPushMessage();
#elif UNITY_ANDROID
		GrowthPushAndroid.GetInstance().callTrackGrowthPushMessage();
#endif
	}
}
