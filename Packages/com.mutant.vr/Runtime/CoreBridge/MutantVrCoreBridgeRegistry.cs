using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mutant.VR.CoreBridge
{
	public static class MutantVrCoreBridgeRegistry
	{
		private static readonly List<MutantVrCoreBridgeInstaller> s_installers =
			new List<MutantVrCoreBridgeInstaller>();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			s_installers.Clear();
		}

		public static void Register(MutantVrCoreBridgeInstaller installer)
		{
			if (installer == null)
			{
				throw new ArgumentNullException(nameof(installer));
			}

			if (!s_installers.Contains(installer))
			{
				s_installers.Add(installer);
			}
		}

		public static void Unregister(MutantVrCoreBridgeInstaller installer)
		{
			if (installer == null)
			{
				return;
			}

			s_installers.Remove(installer);
		}

		public static bool TryGetPrimary(out MutantVrCoreBridgeInstaller installer)
		{
			for (int i = 0; i < s_installers.Count; i++)
			{
				var current = s_installers[i];
				if (current == null)
				{
					continue;
				}

				if (!current.isActiveAndEnabled)
				{
					continue;
				}

				installer = current;
				return true;
			}

			installer = null;
			return false;
		}

		public static IReadOnlyList<MutantVrCoreBridgeInstaller> GetAll()
		{
			return s_installers.AsReadOnly();
		}
	}
}
