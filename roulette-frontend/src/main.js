import './assets/main.css'
import { createApp } from 'vue'

import App from './App.vue'
import router from './router'
import plugin from "./plugin";
import * as bootstrap from 'bootstrap/dist/js/bootstrap.bundle';
import './assets/main.css';

import wagerStore from "./vuex/wagerStore";


const app = createApp(App);

app.use(plugin);
app.use(router);     
app.use(wagerStore);
app.provide('bootstrap', bootstrap);
app.mount('#app');
