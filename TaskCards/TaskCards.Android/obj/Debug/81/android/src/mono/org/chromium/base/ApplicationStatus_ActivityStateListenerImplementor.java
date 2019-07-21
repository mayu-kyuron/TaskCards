package mono.org.chromium.base;


public class ApplicationStatus_ActivityStateListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.base.ApplicationStatus.ActivityStateListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onActivityStateChange:(Landroid/app/Activity;I)V:GetOnActivityStateChange_Landroid_app_Activity_IHandler:Org.Chromium.Base.ApplicationStatus/IActivityStateListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.Base.ApplicationStatus+IActivityStateListenerImplementor, Xamarin.Droid.CrossWalkLite", ApplicationStatus_ActivityStateListenerImplementor.class, __md_methods);
	}


	public ApplicationStatus_ActivityStateListenerImplementor ()
	{
		super ();
		if (getClass () == ApplicationStatus_ActivityStateListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.Base.ApplicationStatus+IActivityStateListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onActivityStateChange (android.app.Activity p0, int p1)
	{
		n_onActivityStateChange (p0, p1);
	}

	private native void n_onActivityStateChange (android.app.Activity p0, int p1);

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
