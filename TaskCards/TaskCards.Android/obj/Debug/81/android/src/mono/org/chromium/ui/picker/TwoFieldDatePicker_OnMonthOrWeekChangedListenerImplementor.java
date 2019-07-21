package mono.org.chromium.ui.picker;


public class TwoFieldDatePicker_OnMonthOrWeekChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.ui.picker.TwoFieldDatePicker.OnMonthOrWeekChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMonthOrWeekChanged:(Lorg/chromium/ui/picker/TwoFieldDatePicker;II)V:GetOnMonthOrWeekChanged_Lorg_chromium_ui_picker_TwoFieldDatePicker_IIHandler:Org.Chromium.UI.Picker.TwoFieldDatePicker/IOnMonthOrWeekChangedListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.UI.Picker.TwoFieldDatePicker+IOnMonthOrWeekChangedListenerImplementor, Xamarin.Droid.CrossWalkLite", TwoFieldDatePicker_OnMonthOrWeekChangedListenerImplementor.class, __md_methods);
	}


	public TwoFieldDatePicker_OnMonthOrWeekChangedListenerImplementor ()
	{
		super ();
		if (getClass () == TwoFieldDatePicker_OnMonthOrWeekChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.UI.Picker.TwoFieldDatePicker+IOnMonthOrWeekChangedListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onMonthOrWeekChanged (org.chromium.ui.picker.TwoFieldDatePicker p0, int p1, int p2)
	{
		n_onMonthOrWeekChanged (p0, p1, p2);
	}

	private native void n_onMonthOrWeekChanged (org.chromium.ui.picker.TwoFieldDatePicker p0, int p1, int p2);

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
