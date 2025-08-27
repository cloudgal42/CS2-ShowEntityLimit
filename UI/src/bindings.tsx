import {bindLocalValue, bindValue} from "cs2/api";
import mod from "mod.json"
export const ENTITY_INDEX_LIMIT = 2147483647;

export const PanelVisabilityBinding = bindLocalValue(false);
export const TotalEntityCount = bindValue<number[]>(mod.id, "TotalEntityCount", []);
export const EntityTypeCount = bindValue<number[]>(mod.id, "EntityTypeCount", []);
// TODO: Implement ClosePanel
// export const ClosePanel = () => {
//    
// }    