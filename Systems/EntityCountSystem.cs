using Unity.Entities;
using Unity.Collections; 

using Game;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Citizens;
using Game.Net;
using Game.Objects;
using Game.Tools;
using Game.Vehicles;

namespace ShowEntityLimit.Systems
{
    public partial class EntityCountSystem : GameSystemBase
    {
        private EntityQuery m_Query;
        private EntityQuery m_BuildingQuery;
        private EntityQuery m_VehicleQuery;
        private EntityQuery m_CitizenQuery;
        private EntityQuery m_PlantQuery;
        private EntityQuery m_NetEdgeQuery;
        private EntityQuery m_AreaQuery;
        private EntityQuery m_NetLaneQuery;
        private EntityQuery m_PropertyRenterQuery;
        
        private int m_TotalEntityCount;
        private int m_BuildingCount;
        private int m_VehicleCount;
        private int m_CitizenCount;
        private int m_PlantCount;
        private int m_NetEdgeCount;
        private int m_OtherEntityCount;
        private int m_AreaCount;
        private int m_NetLaneCount;
        private int m_PropertyRenterCount;

        public NativeArray<int> m_Results;
        
        //TODO: Make this update interval adjustable in settings
        public const int kSystemUpdateInterval = 256;
        
        protected override void OnCreate()
        {  
            base.OnCreate();
            //NOTE: Change the length whenever a new count is implemented
            //TODO: Add NetObject (low priority), Game.Companies.CompanyData, Game.Citizens.Household
            //Can be generalized as a PropertyRenter (separation between 2 can be done via a job)
            m_Results = new NativeArray<int>(10, Allocator.Persistent);
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
            m_NetLaneQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Lane>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_AreaQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Area>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_PropertyRenterQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<PropertyRenter>() 
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
            m_NetLaneCount = m_NetLaneQuery.CalculateEntityCount();
            m_AreaCount = m_AreaQuery.CalculateEntityCount();
            m_PropertyRenterCount = m_PropertyRenterQuery.CalculateEntityCount();
            
            m_OtherEntityCount = m_TotalEntityCount - m_BuildingCount - m_VehicleCount - m_CitizenCount - m_PlantCount - m_NetEdgeCount - m_NetLaneCount - m_AreaCount;

            m_Results[(int)EntityCountType.TotalEntityCount] = m_TotalEntityCount;
            m_Results[(int)EntityCountType.BuildingCount] = m_BuildingCount;
            m_Results[(int)EntityCountType.CitizenCount] = m_CitizenCount;
            m_Results[(int)EntityCountType.VehicleCount] = m_VehicleCount;
            m_Results[(int)EntityCountType.PlantCount] = m_PlantCount;
            m_Results[(int)EntityCountType.NetEdgeCount] = m_NetEdgeCount;
            m_Results[(int)EntityCountType.NetLaneCount] = m_NetLaneCount;
            m_Results[(int)EntityCountType.AreaCount] = m_AreaCount;
            m_Results[(int)EntityCountType.PropertyRenterCount] = m_PropertyRenterCount;
            m_Results[(int)EntityCountType.OtherEntityCount] = m_OtherEntityCount;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return kSystemUpdateInterval;
        }

        protected override void OnDestroy()
        {
            m_Results.Dispose();
            base.OnDestroy();
        }
    }

    public enum EntityCountType
    {
        TotalEntityCount,
        BuildingCount,
        CitizenCount,
        VehicleCount,
        PlantCount,
        NetEdgeCount,
        NetLaneCount,
        AreaCount,
        PropertyRenterCount,
        OtherEntityCount
    }
}