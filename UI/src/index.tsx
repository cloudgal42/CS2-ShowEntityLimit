import { ModRegistrar } from "cs2/modding";
import { HelloWorldComponent } from "mods/hello-world";
import {MainWindow} from "./mods/MainWindow";

const register: ModRegistrar = (moduleRegistry) => {
    
    moduleRegistry.append('Menu', HelloWorldComponent);

    moduleRegistry.append('Game', MainWindow);
}

export default register;