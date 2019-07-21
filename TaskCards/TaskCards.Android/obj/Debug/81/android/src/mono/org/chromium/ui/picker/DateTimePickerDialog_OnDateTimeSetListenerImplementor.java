package mono.org.chromium.ui.picker;


public class DateTimePickerDialog_OnDateTimeSetListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		org.chromium.ui.picker.DateTimePickerDialog.OnDateTimeSetListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDateTimeSet:(Landroid/widget/DatePicker;Landroid/widget/TimePicker;IIIII)V:GetOnDateTimeSet_Landroid_widget_DatePicker_Landroid_widget_TimePicker_IIIIIHandler:Org.Chromium.UI.Picker.DateTimePickerDialog/IOnDateTimeSetListenerInvoker, Xamarin.Droid.CrossWalkLite\n" +
			"";
		mono.android.Runtime.register ("Org.Chromium.UI.Picker.DateTimePickerDialog+IOnDateTimeSetListenerImplementor, Xamarin.Droid.CrossWalkLite", DateTimePickerDialog_OnDateTimeSetListenerImplementor.class, __md_methods);
	}


	public DateTimePickerDialog_OnDateTimeSetListenerImplementor ()
	{
		super ();
		if (getClass () == DateTimePickerDialog_OnDateTimeSetListenerImplementor.class)
			mono.android.TypeManager.Activate ("Org.Chromium.UI.Picker.DateTimePickerDialog+IOnDateTimeSetListenerImplementor, Xamarin.Droid.CrossWalkLite", "", this, new java.lang.Object[] {  });
	}


	public void onDateTimeSet (android.widget.DatePicker p0, android.widget.TimePicker p1, int p2, int p3, int p4, int p5, int p6)
	{
		n_onDateTimeSet (p0, p1, p2, p3, p4, p5, p6);
	}

	private native void n_onDateTimeSet (android.widget.DatePicker p0, android.widget.TimePicker p1, int p2, int p3, int p4, int p5, int p6);

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
