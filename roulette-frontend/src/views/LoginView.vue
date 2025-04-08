<script setup>
</script>
<template>
    <div>
      <div v-if="showForm">
        <div>
          <label for="username">Username:</label>
          <input
            v-model="username"
            type="text"
            id="username"
            @blur="validateUsername"
            :class="{'invalid': usernameError}"
          />
          <p v-if="usernameError" class="error-text">Username es requerido</p>
        </div>
  
        <div>
          <label for="amount">Cantidad a Apostar:</label>
          <input
            v-model="amount"
            type="number"
            id="amount"
            @blur="validateAmount"
            :class="{'invalid': amountError}"
          />
          <p v-if="amountError" class="error-text">Cantidad debe ser mayor que 0</p>
        </div>
        <button 
          :disabled="usernameError || amountError || !username || !amount"
          @click="submitBet"
        >
          Enviar Apuesta
        </button>
      </div>
    </div>
  </template>

<script>
import { mapState, mapMutations, mapGetters } from 'vuex'
export default {
  computed: {
    ...mapState(['player', 'credits']),
  },  
  data() {
    return {
      showForm: false,
      username: '',
      amount: '',
      usernameError: false,
      amountError: false,
      playerBalance: 0
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
          'https://localhost:7004/api/v1.0/Players/SignUp',
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
.invalid {
  border: 1px solid red;
}
.error-text {
  color: red;
  font-size: 12px;
}
</style>

<style scoped>
header {
  line-height: 1.5;
  max-height: 100vh;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
}
</style>
