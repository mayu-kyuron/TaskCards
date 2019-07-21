package mono.org.xwalk.core;


public class XWalkUpdater_XWalkBackgroundUpdateListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.xwalk.core.XWalkUpdater.XWalkBackgroundUpdateListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onXWalkUpdateCancelled:()V:GetOnXWalkUpdateCancelledHandler:Org.Xwalk.Core.XWalkUpdater/IXWalkBackgroundUpdateListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"n_onXWalkUpdateCompleted:()V:GetOnXWalkUpdateCompletedHandler:Org.Xwalk.Core.XWalkUpdater/IXWalkBackgroundUpdateListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"n_onXWalkUpdateFailed:()V:GetOnXWalkUpdateFailedHandler:Org.Xwalk.Core.XWalkUpdater/IXWalkBackgroundUpdateListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"n_onXWalkUpdateProgress:(I)V:GetOnXWalkUpdateProgress_IHandler:Org.Xwalk.Core.XWalkUpdater/IXWalkBackgroundUpdateListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"n_onXWalkUpdateStarted:()V:GetOnXWalkUpdateStartedHandler:Org.Xwalk.Core.XWalkUpdater/IXWalkBackgroundUpdateListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Xwalk.Core.XWalkUpdater+IXWalkBackgroundUpdateListenerImplementor, Xamarin.Droid.CrossWalkLite", XWalkUpdater_XWalkBackgroundUpdateListenerImplementor.class, __md_methods);
	}


	public XWalkUpdater_XWalkBackgroundUpdateListenerImplementor ()
	{
		super ();
		if (getClass () == XWalkUpdater_XWalkBackgroundUpdateListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Xwalk.Core.XWalkUpdater+IXWalkBackgroundUpdateListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onXWalkUpdateCancelled ()
	{
		n_onXWalkUpdateCancelled ();
	}

	private native void n_onXWalkUpdateCancelled ();


	public void onXWalkUpdateCompleted ()
	{
		n_onXWalkUpdateCompleted ();
	}

	private native void n_onXWalkUpdateCompleted ();


	public void onXWalkUpdateFailed ()
	{
		n_onXWalkUpdateFailed ();
	}

	private native void n_onXWalkUpdateFailed ();


	public void onXWalkUpdateProgress (int p0)
	{
		n_onXWalkUpdateProgress (p0);
	}

	private native void n_onXWalkUpdateProgress (int p0);


	public void onXWalkUpdateStarted ()
	{
		n_onXWalkUpdateStarted ();
	}

	private native void n_onXWalkUpdateStarted ();

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
