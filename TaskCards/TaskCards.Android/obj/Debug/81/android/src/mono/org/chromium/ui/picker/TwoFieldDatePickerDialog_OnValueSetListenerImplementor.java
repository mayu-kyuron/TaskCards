package mono.org.chromium.ui.picker;


public class TwoFieldDatePickerDialog_OnValueSetListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.ui.picker.TwoFieldDatePickerDialog.OnValueSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onValueSet:(II)V:GetOnValueSet_IIHandler:Org.Chromium.UI.Picker.TwoFieldDatePickerDialog/IOnValueSetListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.UI.Picker.TwoFieldDatePickerDialog+IOnValueSetListenerImplementor, Xamarin.Droid.CrossWalkLite", TwoFieldDatePickerDialog_OnValueSetListenerImplementor.class, __md_methods);
	}


	public TwoFieldDatePickerDialog_OnValueSetListenerImplementor ()
	{
		super ();
		if (getClass () == TwoFieldDatePickerDialog_OnValueSetListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.UI.Picker.TwoFieldDatePickerDialog+IOnValueSetListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onValueSet (int p0, int p1)
	{
		n_onValueSet (p0, p1);
	}

	private native void n_onValueSet (int p0, int p1);

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
