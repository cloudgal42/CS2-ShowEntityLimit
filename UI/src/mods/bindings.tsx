import {bindValue} from "cs2/api";
import mod from "mod.json"
export const ENTITY_INDEX_LIMIT = 2147483647

export const TotalEntityCount = bindValue<number[]>(mod.id, "TotalEntityCount", []);
export const EntityTypeCount = bindValue<number[]>(mod.id, "EntityTypeCount", []);
// TODO: Implement closePanel
// export const closePanel = () => {
//    
// }    