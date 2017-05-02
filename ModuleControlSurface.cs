using System;
using UnityEngine;

namespace ClawKSP
{
	[KSPAddon(KSPAddon.Startup.MainMenu, false)]
	public class MCSHook : MonoBehaviour
	{
		public void Start()
		{
			Controller.HookModule("ModuleControlSurface", "MCS");
		}
	}

	public class MCS : PartModule
	{
		private ModuleControlSurface ControlSurfaceModule;
		private float ctrlSurfaceRange;
		private float vacuumRange = 1.0f;

		private void Enable()
		{
			Debug.Log(moduleName + "enabled");

			if (FlightGlobals.getStaticPressure(part.transform.position) < 0.001f)
			{
				vacuumRange = 0.01f;
			}

			ctrlSurfaceRange = ControlSurfaceModule.ctrlSurfaceRange;
			ControlSurfaceModule.ctrlSurfaceRange = ctrlSurfaceRange * vacuumRange;
		}

		public override void OnStart(StartState state)
		{
			Debug.Log(moduleName + ".Start():");

			base.OnStart(state);

			ControlSurfaceModule = part.FindModuleImplementing<ModuleControlSurface>();

			if (null == ControlSurfaceModule)
			{
				Debug.LogWarning("Did not find Control Surface Module.");
				return;
			}

			Enable();

		}

		public void FixedUpdate()
		{
			ControlSurfaceModule = part.FindModuleImplementing<ModuleControlSurface>();

			if (null == ControlSurfaceModule)
			{
				return;
			}

			if (true)
			{
				if (FlightGlobals.getStaticPressure(part.transform.position) < 0.001f)
				{
					if (vacuumRange > 0.01f)
					{
						vacuumRange -= 0.05f;
					}
					else
					{
						vacuumRange = 0.01f;
					}
				}
				else
				{
					if (vacuumRange < 1.0f)
					{
						vacuumRange += 0.05f;
					}
					else
					{
						vacuumRange = 1.0f;
					}
				}
				ControlSurfaceModule.ctrlSurfaceRange = ctrlSurfaceRange * vacuumRange * Mathf.Sign(ControlSurfaceModule.ctrlSurfaceRange);
			}
		}
	}
}