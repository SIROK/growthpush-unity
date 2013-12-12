//
//  App42PushHandlerInternal.m
//  PushSDK
//
//  Created by Rajeev Ranjan on 06/08/13.
//  Copyright (c) 2013 ShepHertz Technologies Pvt Ltd. All rights reserved.
//

#import "App42PushHandlerInternal.h"
#import <objc/runtime.h>
#import <GrowthPush/GrowthPush.h>


char * growthPushMessage = 0;
void saveGrowthPushMessage(const char * msg)
{
	free(growthPushMessage);
    growthPushMessage = 0;
	int len = strlen(msg);
	growthPushMessage = malloc(len+1);
	strcpy(growthPushMessage, msg);
}

void callTrackGrowthPushMessage()
{
    if(growthPushMessage != 0)
    {
        UnitySendMessage("GrowthPushReceiveIOS", "launchWithNotification", growthPushMessage );
        free(growthPushMessage);
        growthPushMessage = 0;
    }
}

@implementation UIApplication(App42PushHandlerInternal)

+(void)load
{
    method_exchangeImplementations(class_getInstanceMethod(self, @selector(setDelegate:)), class_getInstanceMethod(self, @selector(setApp42Delegate:)));
}

void app42RunTimeDidBecomActive(id self)
{
	if ([self respondsToSelector:@selector(app42didBecomeActive:)])
    {
		[self app42didBecomeActive:self];
	}
    [GrowthPush trackEvent:@"Launch"];
}

BOOL app42RunTimeDidFinishLaunching(id self, SEL _cmd, id application, id launchOptions)
{
	BOOL result = YES;
	
	if ([self respondsToSelector:@selector(application:app42didFinishLaunchingWithOptions:)])
    {
		result = (BOOL) [self application:application app42didFinishLaunchingWithOptions:launchOptions];
	}
    else
    {
		[self applicationDidFinishLaunching:application];
		result = YES;
	}
    
	NSDictionary *remoteNotificationDictionary = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];
    if (remoteNotificationDictionary != nil) {
        NSMutableDictionary *payload = [NSMutableDictionary dictionaryWithDictionary:remoteNotificationDictionary];
        if(payload != nil)
            [payload removeObjectForKey:@"aps"];
        
        NSError *error;
        NSData *jsonData = [NSJSONSerialization dataWithJSONObject:payload
                                                           options:NSJSONWritingPrettyPrinted
                                                             error:&error];
        NSString *jsonString = nil;
        if(jsonData)
            jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        
        if (jsonString)
            saveGrowthPushMessage([jsonString UTF8String]);
        else
            [GrowthPush trackEvent:@"Launch"];
    }
    else
    {
        [GrowthPush trackEvent:@"Launch"];
    }
	return result;
}


void app42RunTimeDidRegisterForRemoteNotificationsWithDeviceToken(id self, SEL _cmd, id application, id devToken)
{
	if ([self respondsToSelector:@selector(application:app42didRegisterForRemoteNotificationsWithDeviceToken:)])
    {
		[self application:application app42didRegisterForRemoteNotificationsWithDeviceToken:devToken];
	}
    UnitySendMessage("GrowthPushReceiveIOS", "onDidRegisterForRemoteNotificationsWithDeviceToken", [[devToken description] UTF8String]);

}

void app42RunTimeDidFailToRegisterForRemoteNotificationsWithError(id self, SEL _cmd, id application, id error)
{
	if ([self respondsToSelector:@selector(application:app42didFailToRegisterForRemoteNotificationsWithError:)])
    {
		[self application:application app42didFailToRegisterForRemoteNotificationsWithError:error];
	}
	NSString *errorString = [error description];
    const char * str = [errorString UTF8String];
    UnitySendMessage("GrowthPushReceiveIOS", "onDidFailToRegisterForRemoteNotificationsWithError", str);
}

void app42RunTimeDidReceiveRemoteNotification(id self, SEL _cmd, id application, id userInfo)
{
	if ([self respondsToSelector:@selector(application:app42didReceiveRemoteNotification:)])
    {
		[self application:application app42didReceiveRemoteNotification:userInfo];
	}
    
    NSMutableDictionary *payload = [NSMutableDictionary dictionaryWithDictionary:userInfo];
    if(payload != nil)
        [payload removeObjectForKey:@"aps"];
    
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:payload
                                                       options:NSJSONWritingPrettyPrinted
                                                         error:&error];
    NSString *jsonString = nil;
    if(jsonData)
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    
    if (jsonString)
        UnitySendMessage("GrowthPushReceiveIOS", "launchWithNotification", [jsonString UTF8String] );
}



static void exchangeMethodImplementations(Class class, SEL oldMethod, SEL newMethod, IMP impl, const char * signature)
{
	Method method = nil;
    //Check whether method exists in the class
	method = class_getInstanceMethod(class, oldMethod);
	
	if (method)
    {
		//if method exists add a new method 
		class_addMethod(class, newMethod, impl, signature);
        //and then exchange with original method implementation
		method_exchangeImplementations(class_getInstanceMethod(class, oldMethod), class_getInstanceMethod(class, newMethod));
	}
    else
    {
		//if method does not exist, simply add as orignal method
		class_addMethod(class, oldMethod, impl, signature);
	}
}

- (void) setApp42Delegate:(id<UIApplicationDelegate>)delegate
{
    
	static Class delegateClass = nil;
	
	if(delegateClass == [delegate class])
	{
		[self setApp42Delegate:delegate];
		return;
	}
	
	delegateClass = [delegate class];
    
    exchangeMethodImplementations(delegateClass, @selector(applicationDidBecomeActive:),
                                  @selector(app42didBecomeActive:), (IMP)app42RunTimeDidBecomActive, "v@:::");
    
	exchangeMethodImplementations(delegateClass, @selector(application:didFinishLaunchingWithOptions:),
                                  @selector(application:app42didFinishLaunchingWithOptions:), (IMP)app42RunTimeDidFinishLaunching, "v@:::");
    exchangeMethodImplementations(delegateClass, @selector(application:didRegisterForRemoteNotificationsWithDeviceToken:),
		   @selector(application:app42didRegisterForRemoteNotificationsWithDeviceToken:), (IMP)app42RunTimeDidRegisterForRemoteNotificationsWithDeviceToken, "v@:::");
    
	exchangeMethodImplementations(delegateClass, @selector(application:didFailToRegisterForRemoteNotificationsWithError:),
		   @selector(application:app42didFailToRegisterForRemoteNotificationsWithError:), (IMP)app42RunTimeDidFailToRegisterForRemoteNotificationsWithError, "v@:::");
    
	exchangeMethodImplementations(delegateClass, @selector(application:didReceiveRemoteNotification:),
		   @selector(application:app42didReceiveRemoteNotification:), (IMP)app42RunTimeDidReceiveRemoteNotification, "v@:::");
    
	[self setApp42Delegate:delegate];
}


@end
