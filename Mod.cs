using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Colossal.IO.AssetDatabase;
using Game.Input;
using ShowEntityLimit.Systems;
using UnityEngine;

namespace ShowEntityLimit
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(ShowEntityLimit)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);

        private ModSettings m_ModSettings;
        public static ProxyAction m_ButtonAction;
        public static ProxyAction m_AxisAction;
        public static ProxyAction m_VectorAction;

        public const string kButtonActionName = "ButtonBinding";
        public const string kAxisActionName = "FloatBinding";
        public const string kVectorActionName = "Vector2Binding";

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");
            
            updateSystem.UpdateAfter<EntityCountCalculationSystem>(SystemUpdatePhase.GameSimulation);

            m_ModSettings = new ModSettings(this);
            m_ModSettings.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_ModSettings));

            m_ModSettings.RegisterKeyBindings();

            m_ButtonAction = m_ModSettings.GetAction(kButtonActionName);
            m_AxisAction = m_ModSettings.GetAction(kAxisActionName);
            m_VectorAction = m_ModSettings.GetAction(kVectorActionName);

            m_ButtonAction.shouldBeEnabled = true;
            m_AxisAction.shouldBeEnabled = true;
            m_VectorAction.shouldBeEnabled = true;

            m_ButtonAction.onInteraction += (_, phase) =>
                log.Info($"[{m_ButtonAction.name}] On{phase} {m_ButtonAction.ReadValue<float>()}");
            m_AxisAction.onInteraction += (_, phase) =>
                log.Info($"[{m_AxisAction.name}] On{phase} {m_AxisAction.ReadValue<float>()}");
            m_VectorAction.onInteraction += (_, phase) =>
                log.Info($"[{m_VectorAction.name}] On{phase} {m_VectorAction.ReadValue<Vector2>()}");

            AssetDatabase.global.LoadSettings(nameof(ShowEntityLimit), m_ModSettings, new ModSettings(this));
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_ModSettings != null)
            {
                m_ModSettings.UnregisterInOptionsUI();
                m_ModSettings = null;
            }
        }
    }
}