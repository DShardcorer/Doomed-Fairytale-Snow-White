using UnityEngine;

namespace Ink.Demos.Basic_Demo.Scripts
{
	public class QuitGameOnKeypress : MonoBehaviour {
	
		public KeyCode key = KeyCode.Escape;
	
		void Update () {
			if(UnityEngine.Input.GetKeyDown(key)) Application.Quit();
		}
	}
}