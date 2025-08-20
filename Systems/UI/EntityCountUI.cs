using Colossal.UI.Binding;
using Game;
using ShowEntityLimit.Extensions;

namespace ShowEntityLimit.Systems.UI
{
    public partial class EntityCountUI : ExtendedUISystemBase
    {
        private EntityCountSystem m_EntityCountSystem;
        private GetterValueBinding<int[]> m_EntityTypeBinding;
        private GetterValueBinding<int[]> m_TotalEntityBinding;

        protected override void OnCreate()
        {
            base.OnCreate();
            
            m_TotalEntityBinding = CreateBinding("TotalEntityCount", () =>
            {
                m_EntityCountSystem = base.World.GetOrCreateSystemManaged<EntityCountSystem>();
                return m_EntityCountSystem.m_TotalEntityResults.ToArray();
            });
            m_EntityTypeBinding = CreateBinding("EntityTypeCount", () =>
            {   
                m_EntityCountSystem = base.World.GetOrCreateSystemManaged<EntityCountSystem>();
                return m_EntityCountSystem.m_EntityTypeResults.ToArray();
            });
            
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return EntityCountSystem.kSystemUpdateInterval;
        }

        protected override void OnUpdate()
        {
            m_TotalEntityBinding.Update();
            m_EntityTypeBinding.Update();
        }
    }
}