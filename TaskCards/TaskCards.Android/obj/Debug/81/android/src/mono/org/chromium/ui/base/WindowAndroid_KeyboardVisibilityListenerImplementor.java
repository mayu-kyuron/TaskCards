package mono.org.chromium.ui.base;


public class WindowAndroid_KeyboardVisibilityListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.ui.base.WindowAndroid.KeyboardVisibilityListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_keyboardVisibilityChanged:(Z)V:GetKeyboardVisibilityChanged_ZHandler:Org.Chromium.UI.Base.WindowAndroid/IKeyboardVisibilityListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.UI.Base.WindowAndroid+IKeyboardVisibilityListenerImplementor, Xamarin.Droid.CrossWalkLite", WindowAndroid_KeyboardVisibilityListenerImplementor.class, __md_methods);
	}


	public WindowAndroid_KeyboardVisibilityListenerImplementor ()
	{
		super ();
		if (getClass () == WindowAndroid_KeyboardVisibilityListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.UI.Base.WindowAndroid+IKeyboardVisibilityListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void keyboardVisibilityChanged (boolean p0)
	{
		n_keyboardVisibilityChanged (p0);
	}

	private native void n_keyboardVisibilityChanged (boolean p0);

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
