using System.ComponentModel;
using Colossal.Logging;
using Game;
using Game.Buildings;
using Game.Common;
using Game.Citizens;
using Game.Net;
using Game.Objects;
using Game.Tools;
using Game.Vehicles;
using Unity.Entities;
using Debug = UnityEngine.Debug;

namespace ShowEntityLimit.Systems
{
    public partial class EntityCountCalculationSystem : GameSystemBase
    {
        private EntityQuery m_Query;
        private EntityQuery m_BuildingQuery;
        private EntityQuery m_VehicleQuery;
        private EntityQuery m_CitizenQuery;
        private EntityQuery m_PlantQuery;
        private EntityQuery m_NetEdgeQuery;
        
        private int m_TotalEntityCount;
        private int m_BuildingCount;
        private int m_VehicleCount;
        private int m_CitizenCount;
        private int m_PlantCount;
        private int m_NetEdgeCount;
        private int m_OtherEntityCount;
        
        //TODO: Make this update interval adjustable in settings
        private const int kSystemUpdateInterval = 1024;
        
        protected override void OnCreate()
        {
            base.OnCreate(); 
            m_Query = GetEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_BuildingQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Building>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_CitizenQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Citizen>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_VehicleQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Vehicle>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_PlantQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Plant>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_NetEdgeQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Edge>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
        }

        protected override void OnUpdate()
        {
            m_TotalEntityCount = m_Query.CalculateEntityCount();
            m_BuildingCount = m_BuildingQuery.CalculateEntityCount();
            m_CitizenCount = m_CitizenQuery.CalculateEntityCount();
            m_VehicleCount = m_VehicleQuery.CalculateEntityCount();
            m_PlantCount = m_PlantQuery.CalculateEntityCount();
            m_NetEdgeCount = m_NetEdgeQuery.CalculateEntityCount();
            
            m_OtherEntityCount = m_TotalEntityCount - m_BuildingCount - m_VehicleCount - m_CitizenCount - m_PlantCount - m_NetEdgeCount;
            //TODO: Implement UI for this
            Mod.log.Info("Current Total Entities in Game: " + m_TotalEntityCount);
            Mod.log.Info("---Total Buildings: " + m_BuildingCount);
            Mod.log.Info("---Total Citizens: " + m_CitizenCount);
            Mod.log.Info("---Total Vehicles: " + m_VehicleCount);
            Mod.log.Info("---Total Plants: " + m_VehicleCount);
            Mod.log.Info("---Total Net Segments: " + m_VehicleCount);
            Mod.log.Info("---Other Entities: " + m_OtherEntityCount);
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return kSystemUpdateInterval;
        }
    }
}