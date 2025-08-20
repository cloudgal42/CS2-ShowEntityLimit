using Unity.Entities;
using Unity.Collections; 

using Game;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Citizens;
using Game.Companies;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using Marker = Game.Objects.Marker;

namespace ShowEntityLimit.Systems
{
    public partial class EntityCountSystem : GameSystemBase
    {   
        //TODO: Reduce # of EntityQuery by offloading 1 query to process in an IJobChunk
        private EntityQuery m_Query;
        private NativeArray<EntityQuery> m_QueryArray;
        
        //TODO: Change m_TotalEntityCount to count all entities, incld temp ones
        private int m_TotalEntityCount;
        private int m_OtherEntityCount;

        public NativeArray<int> m_TotalEntityResults;
        public NativeArray<int> m_EntityTypeResults;
        
        //TODO: Make this update interval adjustable in settings
        public const int kSystemUpdateInterval = 256;
        
        protected override void OnCreate()
        {  
            base.OnCreate();
            //NOTE: Change the length whenever a new count is implemented
            //TODO: Long-term: Group queries to several types (Game.Objects.Object, Networks, Netlanes, Misc)i s
            m_QueryArray = new NativeArray<EntityQuery>(15, Allocator.Persistent);
            
            m_TotalEntityResults = new NativeArray<int>(3, Allocator.Persistent);
            m_EntityTypeResults = new NativeArray<int>(15, Allocator.Persistent);
            m_Query = GetEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_QueryArray[(int)Type.Building] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.Citizen] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.Vehicle] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.Plant] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.NetEdge] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.NetLane] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.Area] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.Household] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.Company] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.FlowNode] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.FlowEdge] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.Prefab] = GetEntityQuery(new EntityQueryDesc
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
            m_QueryArray[(int)Type.Creature] = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Creature>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>()
                }
            });
            m_QueryArray[(int)Type.Prop] = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<Object>(),
                    ComponentType.ReadOnly<Static>() 
                },
                None = new ComponentType[]
                {
                    ComponentType.Exclude<Deleted>(),
                    ComponentType.Exclude<Temp>(),
                    ComponentType.Exclude<Marker>(),
                    ComponentType.Exclude<Building>(),
                    ComponentType.Exclude<Plant>(),
                    ComponentType.Exclude<Creature>(),
                    ComponentType.Exclude<Vehicle>(),
                }
            });
        }

        protected override void OnUpdate()
        {
            m_TotalEntityCount = m_Query.CalculateEntityCount();
            m_OtherEntityCount = m_TotalEntityCount;
            
            for (int i = 0; i < m_EntityTypeResults.Length - 1; i++)
            {   
                m_EntityTypeResults[i] = m_QueryArray[i].CalculateEntityCount();
                m_OtherEntityCount -= m_EntityTypeResults[i];
            }
            
            m_TotalEntityResults[(int)TotalResults.PersistentCount] = m_TotalEntityCount;
            m_EntityTypeResults[(int)Type.OtherEntity] = m_OtherEntityCount;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return kSystemUpdateInterval;
        }

        protected override void OnDestroy()
        {
            m_QueryArray.Dispose();
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
    
    public enum Type
    {
        Building,
        Citizen,
        Vehicle,
        Plant,
        NetEdge,
        NetLane,
        Area,
        Household,
        Company,
        FlowNode,
        FlowEdge,
        Prefab,
        Creature,
        Prop,
        OtherEntity
    }
}