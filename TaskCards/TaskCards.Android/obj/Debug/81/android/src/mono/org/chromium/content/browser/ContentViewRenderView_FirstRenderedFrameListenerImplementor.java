package mono.org.chromium.content.browser;


public class ContentViewRenderView_FirstRenderedFrameListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.content.browser.ContentViewRenderView.FirstRenderedFrameListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onFirstFrameReceived:()V:GetOnFirstFrameReceivedHandler:Org.Chromium.Content.Browser.ContentViewRenderView/IFirstRenderedFrameListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.Content.Browser.ContentViewRenderView+IFirstRenderedFrameListenerImplementor, Xamarin.Droid.CrossWalkLite", ContentViewRenderView_FirstRenderedFrameListenerImplementor.class, __md_methods);
	}


	public ContentViewRenderView_FirstRenderedFrameListenerImplementor ()
	{
		super ();
		if (getClass () == ContentViewRenderView_FirstRenderedFrameListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.Content.Browser.ContentViewRenderView+IFirstRenderedFrameListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onFirstFrameReceived ()
	{
		n_onFirstFrameReceived ();
	}

	private native void n_onFirstFrameReceived ();

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
