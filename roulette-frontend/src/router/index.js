import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import PlayView from '@/views/PlayView.vue'
import LoginView from '@/views/LoginView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Home',
      component: HomeView, // Tu componente "Juego"
    },        
    {
      path: '/play',
      name: 'PlayRoulette',
      component: PlayView, // Tu componente "Juego"
    },

    {
      path: '/login',
      name: 'Login',
      component: LoginView, // Tu componente "Juego"
    },


  ],
})

export default router
