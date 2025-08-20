using Unity.Entities;
using Unity.Collections; 

using Game;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Citizens;
using Game.Companies;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
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
        private EntityQuery m_HouseholdQuery;
        private EntityQuery m_CompanyQuery;
        private EntityQuery m_FlowNodeQuery;
        private EntityQuery m_FlowEdgeQuery;
        private EntityQuery m_PrefabQuery;
        
        //TODO: Change m_TotalEntityCount to count all entities, incld temp ones
        private int m_TotalEntityCount;
        private int m_BuildingCount;
        private int m_VehicleCount;
        private int m_CitizenCount;
        private int m_PlantCount;
        private int m_NetEdgeCount;
        private int m_OtherEntityCount;
        private int m_AreaCount;
        private int m_NetLaneCount;
        private int m_HouseholdCount;
        private int m_CompanyCount;
        private int m_FlowNodeCount;
        private int m_FlowEdgeCount;
        private int m_PrefabCount;

        public NativeArray<int> m_TotalEntityResults;
        public NativeArray<int> m_EntityTypeResults;
        
        //TODO: Make this update interval adjustable in settings
        public const int kSystemUpdateInterval = 256;
        
        protected override void OnCreate()
        {  
            base.OnCreate();
            //NOTE: Change the length whenever a new count is implemented
            //TODO: Add NetObject (low priority), Creatures, Prefabs
            //TODO: Long-term: Group queries to several types (Game.Objects.Object, Networks, Netlanes, Misc)i s
            m_TotalEntityResults = new NativeArray<int>(3, Allocator.Persistent);
            m_EntityTypeResults = new NativeArray<int>(13, Allocator.Persistent);
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
            m_HouseholdQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Household>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_CompanyQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<CompanyData>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_FlowNodeQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ConnectedFlowEdge>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_FlowEdgeQuery = GetEntityQuery(new EntityQueryDesc
            {
                Any = new ComponentType[]
                {
                    ComponentType.ReadOnly<ElectricityFlowEdge>(),
                    ComponentType.ReadOnly<WaterPipeEdge>()
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_PrefabQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<PrefabData>() 
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
            m_HouseholdCount = m_HouseholdQuery.CalculateEntityCount();
            m_CompanyCount = m_CompanyQuery.CalculateEntityCount();
            m_FlowNodeCount = m_FlowNodeQuery.CalculateEntityCount();
            m_FlowEdgeCount = m_FlowEdgeQuery.CalculateEntityCount();
            m_PrefabCount = m_PrefabQuery.CalculateEntityCount();

            m_TotalEntityResults[(int)TotalResults.PersistentCount] = m_TotalEntityCount;
            
            m_EntityTypeResults[(int)TypeResults.BuildingCount] = m_BuildingCount;
            m_EntityTypeResults[(int)TypeResults.CitizenCount] = m_CitizenCount;
            m_EntityTypeResults[(int)TypeResults.VehicleCount] = m_VehicleCount;
            m_EntityTypeResults[(int)TypeResults.PlantCount] = m_PlantCount;
            m_EntityTypeResults[(int)TypeResults.NetEdgeCount] = m_NetEdgeCount;
            m_EntityTypeResults[(int)TypeResults.NetLaneCount] = m_NetLaneCount;
            m_EntityTypeResults[(int)TypeResults.AreaCount] = m_AreaCount;
            m_EntityTypeResults[(int)TypeResults.HouseholdCount] = m_HouseholdCount;
            m_EntityTypeResults[(int)TypeResults.CompanyCount] = m_CompanyCount;
            m_EntityTypeResults[(int)TypeResults.FlowNodeCount] = m_FlowNodeCount;
            m_EntityTypeResults[(int)TypeResults.FlowEdgeCount] = m_FlowEdgeCount;
            m_EntityTypeResults[(int)TypeResults.PrefabCount] = m_PrefabCount;

            m_OtherEntityCount = m_TotalEntityCount;
            for (int i = 0; i < m_EntityTypeResults.Length - 1; i++)
            {
                m_OtherEntityCount -= m_EntityTypeResults[i];
            }
            
            m_EntityTypeResults[(int)TypeResults.OtherEntityCount] = m_OtherEntityCount;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return kSystemUpdateInterval;
        }

        protected override void OnDestroy()
        {
            m_TotalEntityResults.Dispose();
            m_EntityTypeResults.Dispose();
            base.OnDestroy();
        }
    }

    public enum TotalResults
    {
        TotalCount,
        PersistentCount,
        TempCount
    }
    
    public enum TypeResults
    {
        BuildingCount,
        CitizenCount,
        VehicleCount,
        PlantCount,
        NetEdgeCount,
        NetLaneCount,
        AreaCount,
        HouseholdCount,
        CompanyCount,
        FlowNodeCount,
        FlowEdgeCount,
        PrefabCount,
        OtherEntityCount
    }
}