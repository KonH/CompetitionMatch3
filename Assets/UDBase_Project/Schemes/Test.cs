#if Scheme_Test
using UDBase.Common;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.SaveSystem;

public class ProjectScheme : Scheme {

	public ProjectScheme() {
		AddController(new Log(), new UnityLog());
		AddController(new Events(), new EventController());
		AddController(new Scene(), new DirectSceneLoader());
		
		var save = new FsJsonDataSave().
			AddNode<GameSettings>("settings").
			AddNode<GameStats>("stats");
		AddController(new Save(), save);
	}
}
#endif
