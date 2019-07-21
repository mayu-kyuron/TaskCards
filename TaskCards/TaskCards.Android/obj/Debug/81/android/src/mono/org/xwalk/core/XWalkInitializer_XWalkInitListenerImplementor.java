package mono.org.xwalk.core;


public class XWalkInitializer_XWalkInitListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.xwalk.core.XWalkInitializer.XWalkInitListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onXWalkInitCancelled:()V:GetOnXWalkInitCancelledHandler:Org.Xwalk.Core.XWalkInitializer/IXWalkInitListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"n_onXWalkInitCompleted:()V:GetOnXWalkInitCompletedHandler:Org.Xwalk.Core.XWalkInitializer/IXWalkInitListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"n_onXWalkInitFailed:()V:GetOnXWalkInitFailedHandler:Org.Xwalk.Core.XWalkInitializer/IXWalkInitListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"n_onXWalkInitStarted:()V:GetOnXWalkInitStartedHandler:Org.Xwalk.Core.XWalkInitializer/IXWalkInitListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Xwalk.Core.XWalkInitializer+IXWalkInitListenerImplementor, Xamarin.Droid.CrossWalkLite", XWalkInitializer_XWalkInitListenerImplementor.class, __md_methods);
	}


	public XWalkInitializer_XWalkInitListenerImplementor ()
	{
		super ();
		if (getClass () == XWalkInitializer_XWalkInitListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Xwalk.Core.XWalkInitializer+IXWalkInitListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onXWalkInitCancelled ()
	{
		n_onXWalkInitCancelled ();
	}

	private native void n_onXWalkInitCancelled ();


	public void onXWalkInitCompleted ()
	{
		n_onXWalkInitCompleted ();
	}

	private native void n_onXWalkInitCompleted ();


	public void onXWalkInitFailed ()
	{
		n_onXWalkInitFailed ();
	}

	private native void n_onXWalkInitFailed ();


	public void onXWalkInitStarted ()
	{
		n_onXWalkInitStarted ();
	}

	private native void n_onXWalkInitStarted ();

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
