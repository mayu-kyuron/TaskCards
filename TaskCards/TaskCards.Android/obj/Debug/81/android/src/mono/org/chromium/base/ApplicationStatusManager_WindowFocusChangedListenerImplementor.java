package mono.org.chromium.base;


public class ApplicationStatusManager_WindowFocusChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.base.ApplicationStatusManager.WindowFocusChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onWindowFocusChanged:(Landroid/app/Activity;Z)V:GetOnWindowFocusChanged_Landroid_app_Activity_ZHandler:Org.Chromium.Base.ApplicationStatusManager/IWindowFocusChangedListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.Base.ApplicationStatusManager+IWindowFocusChangedListenerImplementor, Xamarin.Droid.CrossWalkLite", ApplicationStatusManager_WindowFocusChangedListenerImplementor.class, __md_methods);
	}


	public ApplicationStatusManager_WindowFocusChangedListenerImplementor ()
	{
		super ();
		if (getClass () == ApplicationStatusManager_WindowFocusChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.Base.ApplicationStatusManager+IWindowFocusChangedListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onWindowFocusChanged (android.app.Activity p0, boolean p1)
	{
		n_onWindowFocusChanged (p0, p1);
	}

	private native void n_onWindowFocusChanged (android.app.Activity p0, boolean p1);

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
