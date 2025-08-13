import React, {FC} from "react";
import {EntityCountArray, ENTITY_INDEX_LIMIT} from "./bindings";
import {DraggablePanelProps, Panel} from "cs2/ui";
import {
    closeButtonImageClass,
    panelStyle,
    defaultStyle,
    closeButtonClass
} from "../StyleBindings";
import styles from "./Mainwindow.module.scss"
import {useValue} from "cs2/api";

enum EntityCountType {
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

function calculateEntityPercentage(entityCount: number, totalEntityCount: number): string {
    let percentage = 100 * (entityCount / totalEntityCount);
    if (totalEntityCount === 0) {
        return "0%";
    }
    if (percentage < 1) {
        return "<1%";
    }
    else {
        return Math.round(percentage) + "%";
    }
}
interface EntityTypeCountProps {
    index: number;
}

export const MainWindow = () => {
    const countArray = useValue(EntityCountArray);
    const entityType = ["Total", "Building", "Citizen", "Vehicle", "Plants", "Net Segments", "Netlanes", "Areas", "Property Renters", "Other Entities"];
    const TotalEntityCountSection = () => {
        const totalCount = countArray[EntityCountType.TotalEntityCount];
        const totalCountData = totalCount + "/" + ENTITY_INDEX_LIMIT;
        return (
            <div className={styles.row}>
                <div className={styles.data}>Total Entities</div>
                <div className={styles.dataNum}>{totalCountData}</div>
            </div>
        );
    }
    
    const TableHeader = () => {
        return (
            <div className={styles.rowHeader}>
                <div className={styles.data}>Objects</div>
                <div className={styles.dataNum}>Count</div>
                <div className={styles.dataNum}>% of Total</div>
            </div>
        );
    }
    const EntityTypeCountSection = ({index}: EntityTypeCountProps) => {
        return (
            <div className={styles.row}>
                <div className={styles.data}>{entityType[index]}</div>
                <div className={styles.dataNum}>{countArray[index]}</div>
                <div className={styles.dataNum}>{calculateEntityPercentage(countArray[index], countArray[EntityCountType.TotalEntityCount])}</div>
            </div>
        );
    }
    //TODO: 1) Make each data entry be managed by a separated const for better readability
    return (
        <Panel
            draggable={true}
            className={styles.panel}
            header={
                <div className={panelStyle.titleBar}>
                    <div className={defaultStyle.title}>Show Entity Limit</div>
                    <button className={closeButtonClass}>
                        <div
                            className={closeButtonImageClass}
                            style={{
                                maskImage: "url(Media/Glyphs/Close.svg)",
                            }}>
                        </div>
                    </button>
                </div>
            }
        >   
            <div className={styles.container}>
                <TotalEntityCountSection></TotalEntityCountSection>
                <div>
                    <TableHeader />
                    <EntityTypeCountSection index={EntityCountType.BuildingCount} />
                    <EntityTypeCountSection index={EntityCountType.CitizenCount} />
                    <EntityTypeCountSection index={EntityCountType.PropertyRenterCount} />
                    <EntityTypeCountSection index={EntityCountType.VehicleCount} />
                    <EntityTypeCountSection index={EntityCountType.PlantCount} />
                    <EntityTypeCountSection index={EntityCountType.NetEdgeCount} />
                    <EntityTypeCountSection index={EntityCountType.NetLaneCount} />
                    <EntityTypeCountSection index={EntityCountType.AreaCount} />
                    <EntityTypeCountSection index={EntityCountType.OtherEntityCount} />
                </div>
            </div>
        </Panel>
    );
}