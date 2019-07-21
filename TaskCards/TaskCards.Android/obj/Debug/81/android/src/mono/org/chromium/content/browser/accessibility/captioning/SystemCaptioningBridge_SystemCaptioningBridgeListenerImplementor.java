package mono.org.chromium.content.browser.accessibility.captioning;


public class SystemCaptioningBridge_SystemCaptioningBridgeListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.content.browser.accessibility.captioning.SystemCaptioningBridge.SystemCaptioningBridgeListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onSystemCaptioningChanged:(Lorg/chromium/content/browser/accessibility/captioning/TextTrackSettings;)V:GetOnSystemCaptioningChanged_Lorg_chromium_content_browser_accessibility_captioning_TextTrackSettings_Handler:Org.Chromium.Content.Browser.Accessibility.Captioning.ISystemCaptioningBridgeSystemCaptioningBridgeListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.Content.Browser.Accessibility.Captioning.ISystemCaptioningBridgeSystemCaptioningBridgeListenerImplementor, Xamarin.Droid.CrossWalkLite", SystemCaptioningBridge_SystemCaptioningBridgeListenerImplementor.class, __md_methods);
	}


	public SystemCaptioningBridge_SystemCaptioningBridgeListenerImplementor ()
	{
		super ();
		if (getClass () == SystemCaptioningBridge_SystemCaptioningBridgeListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.Content.Browser.Accessibility.Captioning.ISystemCaptioningBridgeSystemCaptioningBridgeListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onSystemCaptioningChanged (org.chromium.content.browser.accessibility.captioning.TextTrackSettings p0)
	{
		n_onSystemCaptioningChanged (p0);
	}

	private native void n_onSystemCaptioningChanged (org.chromium.content.browser.accessibility.captioning.TextTrackSettings p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
