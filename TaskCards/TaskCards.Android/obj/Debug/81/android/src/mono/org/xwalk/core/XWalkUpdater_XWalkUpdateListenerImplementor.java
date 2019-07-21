package mono.org.xwalk.core;


public class XWalkUpdater_XWalkUpdateListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.xwalk.core.XWalkUpdater.XWalkUpdateListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onXWalkUpdateCancelled:()V:GetOnXWalkUpdateCancelledHandler:Org.Xwalk.Core.XWalkUpdater/IXWalkUpdateListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Xwalk.Core.XWalkUpdater+IXWalkUpdateListenerImplementor, Xamarin.Droid.CrossWalkLite", XWalkUpdater_XWalkUpdateListenerImplementor.class, __md_methods);
	}


	public XWalkUpdater_XWalkUpdateListenerImplementor ()
	{
		super ();
		if (getClass () == XWalkUpdater_XWalkUpdateListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Xwalk.Core.XWalkUpdater+IXWalkUpdateListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onXWalkUpdateCancelled ()
	{
		n_onXWalkUpdateCancelled ();
	}

	private native void n_onXWalkUpdateCancelled ();

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
