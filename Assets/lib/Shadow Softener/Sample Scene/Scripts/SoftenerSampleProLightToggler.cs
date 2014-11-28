using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SoftenerSampleProLightToggler : MonoBehaviour
{
	public bool ProOnly;
	
	void Update ()
	{
		bool hasProLicense;
#if UNITY_3_5
		hasProLicense = true;
#else
		hasProLicense = Application.HasProLicense();
#endif
		if((hasProLicense && !ProOnly) || (!hasProLicense && ProOnly))
			light.enabled = false;
		else
			light.enabled = true;
	}
}
