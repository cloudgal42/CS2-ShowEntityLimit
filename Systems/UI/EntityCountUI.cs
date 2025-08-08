using Colossal.UI.Binding;
using Game;
using ShowEntityLimit.Extensions;

namespace ShowEntityLimit.Systems.UI
{
    public partial class EntityCountUI : ExtendedUISystemBase
    {
        private EntityCountSystem m_EntityCountSystem;
        private GetterValueBinding<int[]> m_EntityCountBinding;

        protected override void OnCreate()
        {
            base.OnCreate();
           
            m_EntityCountBinding = CreateBinding("EntityCount", () =>
            {   
                m_EntityCountSystem = base.World.GetOrCreateSystemManaged<EntityCountSystem>();
                return m_EntityCountSystem.m_Results.ToArray();
            });
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return EntityCountSystem.kSystemUpdateInterval;
        }

        protected override void OnUpdate()
        {
            m_EntityCountBinding.Update();
        }
    }
}