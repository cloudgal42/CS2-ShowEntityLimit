import {bindValue} from "cs2/api";
import mod from "mod.json"
export const ENTITY_INDEX_LIMIT = 2147483647
export const EntityCountArray = bindValue<number[]>(mod.id, "EntityCount", []);
// TODO: Implement closePanel
// export const closePanel = () => {
//    
// }    