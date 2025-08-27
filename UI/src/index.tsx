import { ModRegistrar } from "cs2/modding";
import { HelloWorldComponent } from "mods/hello-world";
import {MainWindow} from "./mods/MainWindow";
import {ModButton} from "./mods/TopLeftButton";

const register: ModRegistrar = (moduleRegistry) => {
    
    //TODO: Remove this entry once the TopLeftButton is implemented
    moduleRegistry.append('Menu', HelloWorldComponent);
    
    moduleRegistry.append('GameTopLeft', ModButton)
    moduleRegistry.append('Game', MainWindow);
}

export default register;