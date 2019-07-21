package mono.org.chromium.ui;


public class ColorSuggestionListAdapter_OnColorSuggestionClickListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.ui.ColorSuggestionListAdapter.OnColorSuggestionClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onColorSuggestionClick:(Lorg/chromium/ui/ColorSuggestion;)V:GetOnColorSuggestionClick_Lorg_chromium_ui_ColorSuggestion_Handler:Org.Chromium.UI.ColorSuggestionListAdapter/IOnColorSuggestionClickListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.UI.ColorSuggestionListAdapter+IOnColorSuggestionClickListenerImplementor, Xamarin.Droid.CrossWalkLite", ColorSuggestionListAdapter_OnColorSuggestionClickListenerImplementor.class, __md_methods);
	}


	public ColorSuggestionListAdapter_OnColorSuggestionClickListenerImplementor ()
	{
		super ();
		if (getClass () == ColorSuggestionListAdapter_OnColorSuggestionClickListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.UI.ColorSuggestionListAdapter+IOnColorSuggestionClickListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onColorSuggestionClick (org.chromium.ui.ColorSuggestion p0)
	{
		n_onColorSuggestionClick (p0);
	}

	private native void n_onColorSuggestionClick (org.chromium.ui.ColorSuggestion p0);

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
