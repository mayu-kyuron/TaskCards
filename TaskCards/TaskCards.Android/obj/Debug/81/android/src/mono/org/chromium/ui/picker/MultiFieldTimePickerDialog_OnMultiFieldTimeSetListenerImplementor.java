package mono.org.chromium.ui.picker;


public class MultiFieldTimePickerDialog_OnMultiFieldTimeSetListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.ui.picker.MultiFieldTimePickerDialog.OnMultiFieldTimeSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTimeSet:(IIII)V:GetOnTimeSet_IIIIHandler:Org.Chromium.UI.Picker.MultiFieldTimePickerDialog/IOnMultiFieldTimeSetListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.UI.Picker.MultiFieldTimePickerDialog+IOnMultiFieldTimeSetListenerImplementor, Xamarin.Droid.CrossWalkLite", MultiFieldTimePickerDialog_OnMultiFieldTimeSetListenerImplementor.class, __md_methods);
	}


	public MultiFieldTimePickerDialog_OnMultiFieldTimeSetListenerImplementor ()
	{
		super ();
		if (getClass () == MultiFieldTimePickerDialog_OnMultiFieldTimeSetListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.UI.Picker.MultiFieldTimePickerDialog+IOnMultiFieldTimeSetListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onTimeSet (int p0, int p1, int p2, int p3)
	{
		n_onTimeSet (p0, p1, p2, p3);
	}

	private native void n_onTimeSet (int p0, int p1, int p2, int p3);

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
