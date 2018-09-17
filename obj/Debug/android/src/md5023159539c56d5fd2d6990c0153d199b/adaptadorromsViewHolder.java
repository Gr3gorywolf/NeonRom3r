package md5023159539c56d5fd2d6990c0153d199b;


public class adaptadorromsViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("neonrommer.adaptadorromsViewHolder, NeonRom3r", adaptadorromsViewHolder.class, __md_methods);
	}


	public adaptadorromsViewHolder ()
	{
		super ();
		if (getClass () == adaptadorromsViewHolder.class)
			mono.android.TypeManager.Activate ("neonrommer.adaptadorromsViewHolder, NeonRom3r", "", this, new java.lang.Object[] {  });
	}

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
