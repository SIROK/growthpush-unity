package com.growthpush;

import android.content.Context;
import android.content.Intent;

public class UnityBroadcastReceiver extends BroadcastReceiver{
	@Override
	public void onReceive(Context context, Intent intent) {
		super.onReceive(context, intent);
		if(GrowthPush.getInstance().getClient() == null)
		{
			String str = UnityActivity.parsePushGrowthPushMessage(intent);
			UnityActivity.saveGrowthPushMessage(str);
		}
	}
}
