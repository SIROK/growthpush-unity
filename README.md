> ## :warning: Deprecation Notice

> Growth Push SDK for Unity will be depricated becasue all SDK are integrated with Growthbeat SDK. Please use its SDK instead:
> * [Growthbeat SDK for Unity](https://github.com/SIROK/growthbeat-unity)

GrowthPush SDK for Unity
==================

![GrowthPush](https://growthpush.com/) is push notification and analysis platform for smart devices.

## Easy usage

```cs
GrowthPush.Initialize(APPLICATION_ID, "APPLICATION_SECRET", GrowthPush.Environment.Development, true, "SENDER_ID");
```

That's all. GrowthPush instance will get APNS device token, send it to server. You can get the app ID and secret on web site of GrowthPush. 

You can get furthermore information on [GrowthPush documetations](https://growthpush.com/documents).

## Installation

1. Import GrowthPush.unitypackage.
1. Rename PostprocessBuildPlayerSampleForGrowthPush to PostprocessBuildPlayer, or add the following code if PostprocessBuildePlayer exists.

  ```bash
  ./Assets/Editor/PostprocessBuildPlayerForGrowthPush $@
  ```
  
1. PostprocessBuildPlayerSampleForGrowthPush need Ruby, RubyGems and xcodeproj gem.

  ```bash
  sudo gem install xcodeproj
  ```

1. Rename AndroidManifestSampleForGrowthPush.xml to AndroidManifest.xml, or add the following code if AndroidManifest.xml exists.

  ```xml
  <uses-permission android:name="android.permission.INTERNET" />
  <uses-permission android:name="android.permission.GET_ACCOUNTS" />
  <uses-permission android:name="com.google.android.c2dm.permission.RECEIVE" />
  <uses-permission android:name="android.permission.VIBRATE" />
  <uses-permission android:name="android.permission.WAKE_LOCK" />
  
  <permission android:name="YOUR_APPLICATION_PACKAGE_NAME.permission.C2D_MESSAGE" android:protectionLevel="signature" />
  <uses-permission android:name="YOUR_APPLICATION_PACKAGE_NAME.permission.C2D_MESSAGE" />
  ```

  ```xml
  <activity
      android:name="com.growthpush.view.AlertActivity"
      android:configChanges="orientation|keyboardHidden"
      android:launchMode="singleInstance"
      android:theme="@android:style/Theme.Translucent" />
  
  <receiver
      android:name="com.growthpush.BroadcastReceiver"
      android:permission="com.google.android.c2dm.permission.SEND" >
      <intent-filter>
          <action android:name="com.google.android.c2dm.intent.RECEIVE" />
          <action android:name="com.google.android.c2dm.intent.REGISTRATION" />
          <category android:name="YOUR_APPLICATION_PACKAGE_NAME" />
      </intent-filter>
  </receiver>
  ```

1. Add the following code to register device token and tracking events.

  ```cs
  GrowthPush.Initialize(APPLICATION_ID, "APPLICATION_SECRET", GrowthPush.Environment.Development, true, "SENDER_ID");
  GrowthPush.TrackEvent("Launch");
  GrowthPush.SetDeviceTags();
  GrowthPush.ClearBadge();
  ```

## License

Licensed under the Apache License.
