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
    OtherEntityCount
}

export const MainWindow = ({onClose}:DraggablePanelProps) => {
    const countArray = useValue(EntityCountArray)
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
    //TODO: 1) Make this more scalable and follow React best practices
    //TODO: 2) Change the count by Entity type to a JSX table
    return (
        <Panel
            draggable={true}
            onClose={onClose}
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
                <div className={styles.innerContainer}>
                    <div className={styles.colWide}>
                        <div className={styles.rowHeader}>Object</div>
                        {/*Data*/}
                        <div className={styles.data}>Buildings</div>
                        <div className={styles.data}>Citizens</div>
                        <div className={styles.data}>Vehicles</div>
                        <div className={styles.data}>Plants</div>
                        <div className={styles.data}>Net Edge</div>
                        <div className={styles.data}>Other</div>
                    </div>
                    <div className={styles.col}>
                        <div className={styles.rowHeader}>Count</div>
                        {/*Data*/}
                        <div className={styles.dataNum}>{countArray[EntityCountType.BuildingCount]}</div>
                        <div className={styles.dataNum}>{countArray[EntityCountType.CitizenCount]}</div>
                        <div className={styles.dataNum}>{countArray[EntityCountType.VehicleCount]}</div>
                        <div className={styles.dataNum}>{countArray[EntityCountType.PlantCount]}</div>
                        <div className={styles.dataNum}>{countArray[EntityCountType.NetEdgeCount]}</div>
                        <div className={styles.dataNum}>{countArray[EntityCountType.OtherEntityCount]}</div>
                    </div>
                    <div className={styles.col}>
                        <div className={styles.rowHeader}>% of Total</div>
                        {/*Data*/}
                        <div className={styles.dataNum}>42%</div>
                        <div className={styles.dataNum}>42%</div>
                        <div className={styles.dataNum}>42%</div>
                        <div className={styles.dataNum}>42%</div>
                        <div className={styles.dataNum}>42%</div>
                        <div className={styles.dataNum}>42%</div>
                    </div>
                </div>
            </div>
        </Panel>
    );
}