import React, {FC} from "react";
import {Button} from "cs2/ui";

export const ShowEntityLimitButton = () => {
    const showPanel = () => {
        //TODO: Implement this function
    }
    
    return (
        <Button
            variant="floating"
            onClick={showPanel}
        />
    );
}