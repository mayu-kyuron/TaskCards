package mono.org.chromium.ui;


public class VSyncMonitor_ListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.ui.VSyncMonitor.Listener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onVSync:(Lorg/chromium/ui/VSyncMonitor;J)V:GetOnVSync_Lorg_chromium_ui_VSyncMonitor_JHandler:Org.Chromium.UI.VSyncMonitor/IListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.UI.VSyncMonitor+IListenerImplementor, Xamarin.Droid.CrossWalkLite", VSyncMonitor_ListenerImplementor.class, __md_methods);
	}


	public VSyncMonitor_ListenerImplementor ()
	{
		super ();
		if (getClass () == VSyncMonitor_ListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.UI.VSyncMonitor+IListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onVSync (org.chromium.ui.VSyncMonitor p0, long p1)
	{
		n_onVSync (p0, p1);
	}

	private native void n_onVSync (org.chromium.ui.VSyncMonitor p0, long p1);

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
