<script setup>
import {RouterView } from 'vue-router';
</script>

<template>
  <div class="dark-background">
    <RouterView/>
  </div>  
</template>
  

<style scoped>

.dark-background {
  display: flex;
  justify-content: center;     
  align-items: center;         
  background-color: #050c13; 
  color: white; 
  min-height: 100vh;
  min-width: 100vw;
  }  
</style>