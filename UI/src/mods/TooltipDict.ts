//TODO: Implement localization
import {ENTITY_INDEX_LIMIT} from "../bindings";

export const PersistentCountTooltip = "Current number of persistent entities in your city. Maximum is 2,147,483,647, based on the Unity ECS entity index."
export const ObjectHeaderTooltip = "Types of Entities in the game."
export const CountHeaderTooltip = "Number of those types of Entities in your city."
export const PercentOfTotalTooltip = "Percentage of those types of Entities relative to the total count in your city."
//TODO: Validate if its WaterNodeEdge/Node or WaterPipeEdge/Node
export const EntityTypeTooltip = [
    "Number of Buildings in your city.",
    "Number of Citizens in your city.",
    "Number of Vehicles in your city.",
    "Number of Plants in your city.",
    "Number of Network Segments (such as Roads and Railways) in your city.",
    "Number of Netlanes in your city. Includes (but not limited to) Netlane fences, as well as Sublanes in roads.",
    "Number of \"Areas\" (such as Districts and Surfaces) in your city.",
    "Number of Households in your city.",
    "Number of Companies in your city. Some companies may not have a property (called \"Ghost Companies\")",
    "Number of Flow Nodes (ElectricityFlowNode, WaterPipeNode) in your city. Exists in Electricity and Water networks.",
    "Number of Flow Segments (ElectricityFlowEdge, WaterPipeEdge) in your city. Exists in Electricity and Water networks.",
    "Number of loaded Prefabs (assets) in your city. Cannot be changed, except by removing Road Builder roads.",
    "Number of Creatures (Humans, Pets and Wild Animals) in your city.",
    "Number of Props in your city.",
    "Other types of Entites in your city."
]