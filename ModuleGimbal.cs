using UnityEngine;

namespace ClawKSP
{
	[KSPAddon(KSPAddon.Startup.MainMenu, false)]
	public class MGHook : MonoBehaviour
	{
		public void Start()
		{
			Controller.HookModule("ModuleGimbal", "MG");
		}
	}

	public class MG : PartModule
	{
		public bool on = true;
		ModuleGimbal gimbal;
		float gimbalRange;
		float oneOrZero = 1.0f;

		public override void OnStart(StartState state)
		{
			base.OnStart(state);
			gimbal = part.FindModuleImplementing<ModuleGimbal>();
			gimbalRange = gimbal.gimbalRange;
			gimbal.gimbalRange = gimbalRange * oneOrZero;
		}

		public void FixedUpdate()
		{
			if (on)
			{
				oneOrZero = 1.0f;
			}
			else
			{
				oneOrZero = 0.0f;
			}
			gimbal.gimbalRange = gimbalRange * oneOrZero;
		}
	}
}