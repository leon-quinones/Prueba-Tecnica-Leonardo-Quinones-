<template >
    <div>
      <button v-if="showButton" @click="Login" class="btn btn-danger">Iniciar Apuesta</button>
    </div>    
</template>

<script>

export default {
  data() {
    return {
      showButton: true,
    };
  },
  methods: {
    async Login() {
      this.showButton = false
      this.$router.push({ name: 'Login' }); 
    }
                  
  }    
}
</script>