import './assets/main.css'
import { createApp } from 'vue'

import App from './App.vue'
import router from './router'
import plugin from "./plugin";

import wagerStore from "./vuex/wagerStore";


const app = createApp(App)

app.use(plugin)
app.use(router)      
app.use(wagerStore) 
app.mount('#app')
