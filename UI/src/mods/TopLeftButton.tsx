import React, {FC} from "react";
import {Button} from "cs2/ui";

export const ModButton = () => {
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