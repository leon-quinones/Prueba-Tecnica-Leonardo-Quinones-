<script setup>
</script>
<template>
    <div>
      <div v-if="showForm">
        <div class="container">
            <label for="username" class="badge text-bg-success" style="font-size: 1.7vh; margin: 1vh">Username:</label>
          <input
            v-model="username"
            type="text"
            id="username"
            @blur="validateUsername"
            :class="{'invalid': usernameError}"
            style="font-size: 1.7vh; margin: 1vh"
          />
          <p v-if="usernameError" class="error-text alert alert-danger error-message">Username es requerido</p>
        </div>
  
        <div>
          <label for="amount" class="badge text-bg-success" style="font-size: 1.7vh; margin: 1vh">Cantidad a Apostar:</label>
          <input
            v-model="amount"
            type="number"
            id="amount"
            @blur="validateAmount"
            :class="{'invalid': amountError}"
            style="font-size: 1.7vh; margin: 1vh"
          />
          <p v-if="amountError" class="error-text alert alert-danger error-message" >Cantidad debe ser mayor que 0</p>
        </div>
        <div style="display: flex; align-items: center; justify-content: center; margin: 2vh;">
          <button 
          :disabled="usernameError || amountError || !username || !amount"
          @click="submitBet"
          class="btn btn-danger"
          style=""
        >
          Enviar Apuesta
        </button>
        </div style="margin: 3vh" v-if="isUserFound">
        </div>
    </div>
  </template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
export default {
  computed: {
    ...mapState(['player', 'credits', 'appDomain']),
    ...mapGetters(['getAppBaseUrl'])   
  },  
  data() {
    return {
      showForm: false,
      username: '',
      amount: '',
      usernameError: false,
      amountError: false,
      playerBalance: 0,
      isUserFound: false
    };
  },
  mounted() {this.displayForm()},
  methods: {
    ...mapMutations(['setUsername', 'setSessionCredits']),
   

    displayForm() {
      this.showForm = true; 
    },
    validateUsername() {
      const regex = /^[\w]+$/; 
      this.usernameError = !regex.test(this.username);
    },
    validateAmount() {
      this.amountError = !this.amount || !(this.amount > 1 && this.amount < 1000000) ;
    },
    async submitBet() {
      if (this.username && this.amount > 0) {
         this.username = this.$ToLowerUsername(this.username);
         await fetch(
          `${this.getAppBaseUrl}/Players/SignUp`,
          {
            method: "POST",
            body: JSON.stringify({
            username: this.username,
            amount: this.amount,
            }),    
            headers: {
              'Content-Type': 'application/json',
              'Accept': 'application/json',
              'User-Agent': 'roulette-frontend/0.1',
              'Access-Control-Allow-Origin': '*'
            }    
          }).then((response => {
            if (!this.$ValidateAPIResponse(response.status, 409)) {
              this.errorMessage = 'Servicio no disponible. Intenta nuevamente más tarde.';
              this.$router.push({ 
                name: 'Home' }); 
  
              return;
            }      
            if (response.status === 409) {
              alert("El usuario ya se encuentra registrado. Se adicionará el balance actual de la cuenta al monto ingresado anteriormente")        
            }
            return response.json();
          })).then( (body) => {
                  
              this.setSessionCredits(this.amount);
              this.setUsername(this.username);
              this.$SaveToken(this.username, body.token);
            
              this.$router.push({ 
                name: 'PlayRoulette' }); 
            }
          )        
      }    
    }
  },
  
}
</script>



<style scoped>
container{
  display: flex
}

.error-message {
  display: flex;
  align-items: center;
  font-size: 1.2vh; 
  height: 2vh;
}

</style>
