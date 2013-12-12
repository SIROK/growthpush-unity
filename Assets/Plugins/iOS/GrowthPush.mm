//
//  GrowthPush.mm
//
//
//  Created by Cuong Do on 11/5/13.
//
//

#import <UIKit/UIKit.h>
#import <GrowthPush/GrowthPush.h>
#import "App42PushHandlerInternal.h"

NSString* NSStringFromCharString(const char* in_str)
{
    NSString* str = [NSString stringWithCString:in_str encoding:NSUTF8StringEncoding];
    return str;
}

extern "C" void _easyGrowthPush_setApplicationId(int applicationID, const char* secret, bool debug)
{
    [EasyGrowthPush setApplicationId:applicationID
                              secret:NSStringFromCharString(secret)
                         environment:kGrowthPushEnvironment
                               debug:debug];
}

extern "C" void _easyGrowthPush_setApplicationId_option(int applicationID, const char* secret, bool debug, int option)
{
    [EasyGrowthPush setApplicationId:applicationID
                              secret:NSStringFromCharString(secret)
                         environment:kGrowthPushEnvironment
                               debug:debug
                              option:option];
}

extern "C" void _growthPush_setApplicationId(int applicationID, const char* secret, int environment, bool debug, int option)
{
    [GrowthPush setApplicationId:applicationID
                          secret:NSStringFromCharString(secret)
                     environment:environment
                           debug:debug];
}

extern "C" void _growthPush_requestDeviceToken()
{
    [GrowthPush requestDeviceToken];
}

extern "C" void _growthPush_setDeviceToken(const char* deviceToken)
{
    NSString *str = NSStringFromCharString(deviceToken);
    str = [str lowercaseString];
    NSMutableData *data= [NSMutableData new];
    unsigned char whole_byte;
    char byte_chars[3] = {'\0','\0','\0'};
    int i = 0;
    int length = str.length;
    while (i < length-1) {
        char c = [str characterAtIndex:i++];
        if (c < '0' || (c > '9' && c < 'a') || c > 'f')
            continue;
        byte_chars[0] = c;
        byte_chars[1] = [str characterAtIndex:i++];
        whole_byte = strtol(byte_chars, NULL, 16);
        [data appendBytes:&whole_byte length:1];
        
    }

    [GrowthPush setDeviceToken:data];
}


extern "C" void _growthPush_trackEvent(const char* name)
{
    [GrowthPush trackEvent:NSStringFromCharString(name)];
}

extern "C" void _growthPush_trackEvent_value(const char* name, const char* value)
{
    [GrowthPush trackEvent:NSStringFromCharString(name)
                     value:NSStringFromCharString(value)];
}

extern "C" void _growthPush_setTag(const char* name)
{
    [GrowthPush setTag:NSStringFromCharString(name)];
}

extern "C" void _growthPush_setTag_value(const char* name, const char* value)
{
    [GrowthPush setTag:NSStringFromCharString(name)
                 value:NSStringFromCharString(value)];
}

extern "C" void _growthPush_setDeviceTags()
{
    [GrowthPush setDeviceTags];
}

extern "C" void _growthPush_clearBadge()
{
    [GrowthPush clearBadge];
}