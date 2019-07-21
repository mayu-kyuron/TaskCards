package mono.org.chromium.ui;


public class OnColorChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.ui.OnColorChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onColorChanged:(I)V:GetOnColorChanged_IHandler:Org.Chromium.UI.IOnColorChangedListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.UI.IOnColorChangedListenerImplementor, Xamarin.Droid.CrossWalkLite", OnColorChangedListenerImplementor.class, __md_methods);
	}


	public OnColorChangedListenerImplementor ()
	{
		super ();
		if (getClass () == OnColorChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.UI.IOnColorChangedListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onColorChanged (int p0)
	{
		n_onColorChanged (p0);
	}

	private native void n_onColorChanged (int p0);

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
