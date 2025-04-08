import accountStore from "./accountStore";
import wagerStore from "./wagerStore";

import { createStore } from 'vuex';


const store = createStore({
    modules: {
      accounts: accountStore,
      wagers: wagerStore
    }
  });
  
  export default store;