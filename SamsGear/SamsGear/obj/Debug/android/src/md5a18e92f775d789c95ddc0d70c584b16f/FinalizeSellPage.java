package md5a18e92f775d789c95ddc0d70c584b16f;


public class FinalizeSellPage
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onRestart:()V:GetOnRestartHandler\n" +
			"";
		mono.android.Runtime.register ("SamsGear.FinalizeSellPage, SamsGear, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FinalizeSellPage.class, __md_methods);
	}


	public FinalizeSellPage () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FinalizeSellPage.class)
			mono.android.TypeManager.Activate ("SamsGear.FinalizeSellPage, SamsGear, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onRestart ()
	{
		n_onRestart ();
	}

	private native void n_onRestart ();

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
