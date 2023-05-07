using Harmony;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace CraftingHotkey
{
	public class Implementation : MelonMod
	{
        [Obsolete]
        public override void OnApplicationStart()
		{
			Debug.Log($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();
		}
		public static void MaybeShowCraftingMenu()
		{
			if (InputManager.GetKeyDown(InputManager.m_CurrentContext, Settings.options.keyCode))
			{
				Panel_Crafting craftingPanel = InterfaceManager.GetPanel<Panel_Crafting>();
				if (craftingPanel == null || uConsole.IsOn()) return;
				if (craftingPanel.IsEnabled() && !craftingPanel.m_CraftingInProgress)
				{
					craftingPanel.OnBackButton();
					return;
				}
				else
				{
					Panel_FirstAid firstAid = InterfaceManager.GetPanel<Panel_FirstAid>();
					if (firstAid != null && firstAid.IsEnabled())
					{
						firstAid.OnCraftingNav();
						return;
					}
					Panel_Clothing clothing = InterfaceManager.GetPanel<Panel_Clothing>();
					if (clothing != null && clothing.IsEnabled())
					{
						clothing.OnCraftingNav();
						return;
					}
					Panel_Inventory inventory = InterfaceManager.GetPanel<Panel_Inventory>();
					if (inventory != null && inventory.IsEnabled())
					{
						inventory.OnCraftingNav();
						return;
					}
					Panel_Log journal = InterfaceManager.GetPanel<Panel_Log>();
					if (journal != null && journal.IsEnabled() && journal.m_ReadyForInput)
					{
						journal.OnCraftingNav();
						return;
					}
					Panel_Map map = InterfaceManager.GetPanel<Panel_Map>();
					if (map != null && (map.IsEnabled() || !InterfaceManager.IsOverlayActiveImmediate()))
					{
						map.OnCraftingNav();
						return;
					}
				}
			}
		}
	}

	[HarmonyLib.HarmonyPatch(typeof(GameManager), "Update")]
	internal class GameManager_Update
	{
		private static void Postfix()
		{
			Implementation.MaybeShowCraftingMenu();
		}
	}
}
