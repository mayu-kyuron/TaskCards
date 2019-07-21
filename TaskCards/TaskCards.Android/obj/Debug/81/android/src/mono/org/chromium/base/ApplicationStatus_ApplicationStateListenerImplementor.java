package mono.org.chromium.base;


public class ApplicationStatus_ApplicationStateListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.base.ApplicationStatus.ApplicationStateListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onApplicationStateChange:(I)V:GetOnApplicationStateChange_IHandler:Org.Chromium.Base.ApplicationStatus/IApplicationStateListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.Base.ApplicationStatus+IApplicationStateListenerImplementor, Xamarin.Droid.CrossWalkLite", ApplicationStatus_ApplicationStateListenerImplementor.class, __md_methods);
	}


	public ApplicationStatus_ApplicationStateListenerImplementor ()
	{
		super ();
		if (getClass () == ApplicationStatus_ApplicationStateListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.Base.ApplicationStatus+IApplicationStateListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onApplicationStateChange (int p0)
	{
		n_onApplicationStateChange (p0);
	}

	private native void n_onApplicationStateChange (int p0);

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
