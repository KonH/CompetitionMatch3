using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UDBase.EditorTools {
	public static class SchemesMenuItems {
		[MenuItem("UDBase/Schemes/Default")]
		static void SwitchToScheme_Default() {
			SchemesTool.SwitchScheme("Default");
		}		[MenuItem("UDBase/Schemes/Test")]
		static void SwitchToScheme_Test() {
			SchemesTool.SwitchScheme("Test");
		}
	}
}
