#if Scheme_Test
using UnityEngine;
using System.Collections;
using UDBase.Common;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;

public class ProjectScheme : Scheme {

	public ProjectScheme() {
		AddController(new Log(), new UnityLog());
		AddController(new Events(), new EventController());
	}
}
#endif
