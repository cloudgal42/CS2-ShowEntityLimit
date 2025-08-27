import React from "react";
import {Panel, Tooltip} from "cs2/ui";
import {
    closeButtonClass, 
    closeButtonImageClass
} from "../StyleBindings";
import {
    ENTITY_INDEX_LIMIT, 
    EntityTypeCount, 
    TotalEntityCount
} from "../bindings";
import styles from "./Mainwindow.module.scss"
import {useValue} from "cs2/api";
import {LocalizedNumber, Unit} from "cs2/l10n";
import {
    CountHeaderTooltip, EntityTypeTooltip,
    ObjectHeaderTooltip,
    PercentOfTotalTooltip,
    PersistentCountTooltip
} from "./TooltipDict";

enum TotalResults {
    TotalCount,
    PersistentCount,
    TempCount
}
interface EntityType {
    id: number;
    type: string;
    count: number;
    percentOfTotal: string;
    // TODO: Implement tooltip dictionary for each entity type
    tooltip: string;
}

export const MainWindow = () => {
    const calculateEntityPercentage = (entityCount: number) => {
        let percentage = 100 * (entityCount / totalCountArray[TotalResults.PersistentCount]);
        if (totalCountArray[TotalResults.PersistentCount] === 0 || entityCount === 0) {
            return "0%";
        }
        else if (percentage < 1) {
            return "<1%";
        }
        else {
            return `${Math.round(percentage)}%`;
        }
    }
    const totalCountArray = useValue(TotalEntityCount);
    const typeCountArray = useValue(EntityTypeCount);
    const entityType = [
        "Buildings",
        "Citizens",
        "Vehicles",
        "Plants",
        "Net Segments",
        "Netlanes",
        "Areas",
        "Households",
        "Companies",
        "Flow Nodes",
        "Flow Segments",
        "Prefabs",
        "Creatures",
        "Props",
        "Other Entities"
    ];
    const entityTypeArray: EntityType[] = entityType.map((type, i) => ({
        id: i,
        type,
        count: typeCountArray[i] ?? 0,
        percentOfTotal: calculateEntityPercentage(typeCountArray[i]),
        tooltip: EntityTypeTooltip[i]
    }));
    const GetCountSeverityStyle = (count: number) => {
        const percentOfMax = 100 * (count / ENTITY_INDEX_LIMIT);
        if (percentOfMax > 85) {
            return { color: "#e15e49" }
        }
        else if (percentOfMax > 50) {
            return { color: "#ffe57e" }
        }
        else if (percentOfMax === 0) {
            return { color: "lightgray"}
        }
        else {
            return { color: "#81cd45" }
        }
    }
    const TotalEntitySection = () => {
        const totalCount = totalCountArray[TotalResults.PersistentCount];
        const severityStyle = GetCountSeverityStyle(totalCount);
        // const totalCountData = totalCount.toLocaleString() + "/" + ENTITY_INDEX_LIMIT;
        return (
            <div className={styles.row}>
                <Tooltip tooltip={PersistentCountTooltip}>
                    <div className={styles.data}>
                        Total Entities
                    </div>
                </Tooltip>
                <Tooltip tooltip={PersistentCountTooltip}>
                    <div className={styles.dataNumLimit}>
                    <span style={severityStyle}>
                        <LocalizedNumber unit={Unit.Integer} value={totalCount} />
                    </span>
                        {"/"}
                        <LocalizedNumber unit={Unit.Integer} value={ENTITY_INDEX_LIMIT} />
                    </div>
                </Tooltip>
            </div>
        );
    }
    const TableHeader = () => {
        return (
            <div className={styles.rowHeader}>
                <Tooltip tooltip={ObjectHeaderTooltip}>
                    <div className={styles.data}>Objects</div>
                </Tooltip>
                <Tooltip tooltip={CountHeaderTooltip}>
                    <div className={styles.dataNum}>Count</div>
                </Tooltip>
                <Tooltip tooltip={PercentOfTotalTooltip}>
                    <div className={styles.dataNum}>% of Total</div>
                </Tooltip>
            </div>
        );
    }
    const EntityTypeRow = () => {
        const rowEntry = entityTypeArray.map(entity => {
            return (
                <div className={styles.row} key={entity.id}>
                    <Tooltip tooltip={entity.tooltip}>
                        <div className={styles.data}>
                            {entity.type}
                        </div>
                    </Tooltip>
                    <div className={styles.dataNum}>
                        <LocalizedNumber unit={Unit.Integer} value={entity.count}></LocalizedNumber>
                    </div>
                    <div className={styles.dataNum}>
                        {entity.percentOfTotal}
                    </div>
                </div>
            );
        })
        return (
            <div>{rowEntry}</div>
        )
    }
    return (
        <Panel
            draggable={true}
            className={styles.panel}
            header={(
                <div className={styles.header}>
                    <div></div>
                    <div></div>
                    <span>Show Entity Limit</span>
                    <button className={closeButtonClass}>
                        <div
                            className={closeButtonImageClass}
                            style={{
                                maskImage: "url(Media/Glyphs/Close.svg)",
                            }}>
                        </div>
                    </button>
                </div>
            )}
        >   
            <div className={styles.container}>
                <TotalEntitySection />
                <div>
                    <TableHeader />
                    <EntityTypeRow />
                </div>
            </div>
        </Panel>
    );
}