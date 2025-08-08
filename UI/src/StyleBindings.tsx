import {getModule} from "cs2/modding";

export const defaultStyle = getModule(
    "game-ui/common/panel/themes/default.module.scss",
    "classes"
);
export const panelStyle = getModule(
    "game-ui/common/panel/panel.module.scss",
    "classes"
);
export const iconStyle = getModule(
    "game-ui/common/image/tinted-icon.module.scss",
    "classes"
);
export const closeButtonStyle = getModule(
    "game-ui/common/input/button/themes/round-highlight-button.module.scss",
    "classes"
);
export const tintedIconStyle = getModule(
    "game-ui/common/image/tinted-icon.module.scss",
    "classes"
);


export const closeButtonClass = `${closeButtonStyle.button} ${panelStyle.closeButton}`;
export const closeButtonImageClass = `${tintedIconStyle.tintedIcon} ${iconStyle.icon}`;