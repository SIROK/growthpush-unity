//
//  GrowthPush.mm
//
//
//  Created by Cuong Do on 11/5/13.
//
//

#import <UIKit/UIKit.h>
#import <GrowthPush/GrowthPush.h>

NSString* NSStringFromCharString(const char* charString) {
	if(charString == NULL)
		return nil;
    return [NSString stringWithCString:charString encoding:NSUTF8StringEncoding];
}

extern "C" void growthPushSetApplicationId(int applicationID, const char* secret, int environment, bool debug) {
    [EasyGrowthPush setApplicationId:applicationID secret:NSStringFromCharString(secret) environment:(GPEnvironment)environment debug:debug option:EGPOptionNone];
}

extern "C" void growthPushTrackEvent(const char* name, const char* value) {
    [EasyGrowthPush trackEvent:NSStringFromCharString(name) value:NSStringFromCharString(value)];
}

extern "C" void growthPushSetTag(const char* name, const char* value) {
    [EasyGrowthPush setTag:NSStringFromCharString(name) value:NSStringFromCharString(value)];
}

extern "C" void growthPushSetDeviceTags() {
    [EasyGrowthPush setDeviceTags];
}

extern "C" void growthPushClearBadge() {
    [EasyGrowthPush clearBadge];
}
