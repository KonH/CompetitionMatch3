#if Scheme_Test
using UDBase.Common;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SceneSystem;

public class ProjectScheme : Scheme {

	public ProjectScheme() {
		AddController(new Log(), new UnityLog());
		AddController(new Events(), new EventController());
		AddController(new Scene(), new DirectSceneLoader());
	}
}
#endif
