import React, {FC} from "react";
import {ENTITY_INDEX_LIMIT, EntityTypeCount, TotalEntityCount} from "./bindings";
import {Button, Panel} from "cs2/ui";
import {
    closeButtonImageClass,
    panelStyle,
    defaultStyle,
    closeButtonClass
} from "../StyleBindings";
import styles from "./Mainwindow.module.scss"
import {useValue} from "cs2/api";
import {LocalizedNumber, Unit} from "cs2/l10n";

enum TotalResults {
    TotalCount,
    PersistentCount,
    TempCount
}
interface EntityType {
    id: number;
    type: string;
    count: number;
}

export const MainWindow = () => {
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
        count: typeCountArray[i] ?? 0
    }));
    const calculateEntityPercentage = (entityCount: number, totalEntityCount: number): string => {
        let percentage = 100 * (entityCount / totalEntityCount);
        if (totalEntityCount === 0) {
            return "0%";
        }
        else if (percentage < 1) {
            return "<1%";
        }
        else {
            return Math.round(percentage) + "%";
        }
    }

    // TODO: Add color for total count depending on the threshold
    // <50% 81cd45
    // <85% ffe57e
    // >85% e15e49
    const TotalEntitySection = () => {
        const totalCount = totalCountArray[TotalResults.PersistentCount];
        // const totalCountData = totalCount.toLocaleString() + "/" + ENTITY_INDEX_LIMIT;
        return (
            <div className={styles.row}>
                <div className={styles.data}>
                    Total Entities
                </div>
                <div className={styles.dataNum}>
                    <LocalizedNumber unit={Unit.Integer} value={totalCount}></LocalizedNumber>
                    {"/"}
                    <LocalizedNumber unit={Unit.Integer} value={ENTITY_INDEX_LIMIT}></LocalizedNumber>
                </div>
            </div>
        );
    }
    const TableHeader = () => {
        return (
            <div className={styles.rowHeader}>
                <div className={styles.data}>
                    Objects
                </div>
                <div className={styles.dataNum}>
                    Count
                </div>
                <div className={styles.dataNum}>
                    % of Total
                </div>
            </div>
        );
    }
    //TODO: use <LocalizedNumber> instead of toLocaleString()
    const EntityTypeRow = () => {
        const rowEntry = entityTypeArray.map(entity => {
            return (
                <div className={styles.row} key={entity.id}>
                    <div className={styles.data}>
                        {entity.type}
                    </div>
                    <div className={styles.dataNum}>
                        <LocalizedNumber unit={Unit.Integer} value={entity.count}></LocalizedNumber>
                    </div>
                    <div className={styles.dataNum}>
                        {calculateEntityPercentage(entity.count, totalCountArray[TotalResults.PersistentCount])}
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
                <TotalEntitySection />
                <div>
                    <TableHeader />
                    <EntityTypeRow />
                </div>
            </div>
        </Panel>
    );
}