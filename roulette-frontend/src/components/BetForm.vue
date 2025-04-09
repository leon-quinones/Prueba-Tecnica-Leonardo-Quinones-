<template>
    <div class="container">
      <h2>Ingresa la cantidad que quieres apostar</h2>
  
      <!-- Campo para ingresar la cantidad de dinero -->
      <div style="margin: 1vh;">
        <label for="amount" style="font-size: 2vh; margin-inline: 2vh;">Cantidad de Apuesta:</label>
        <input
          width="100%"
          type="number"
          id="amount"
          v-model="amount"
          :max="balance"
          :min="1"
          placeholder="Ingresa la cantidad"
          @input="validateAmount"
          style="font-size: 2vh;"
        />
        <p v-if="amount <= 1" class="error-text alert alert-danger error-message">La cantidad mínima es 1.</p>
        <p v-if="amount >= balance" class="error-text alert alert-danger error-message">La cantidad máxima es {{ balance }}.</p>
      </div>
      <div>
        
      </div>
  
      <!-- Mensaje de validación -->
      <p v-if="!isValid" class="error-message">Por favor, ingresa una cantidad válida y selecciona un tipo de apuesta.</p>
    </div>
  </template>
  
  <script>

  import { mapState, mapMutations, mapGetters } from 'vuex'
  export default {


    data() {
      return {
        amount: '',              
        username: this.getUsername,
        balance: this.getPlayerBalance
      };
    },
    computed: {
        ...mapState(['player', 'wagerAmount', 'playerAccountBalance', 'wagerBetType',]),
        ...mapGetters(['getUsername', 'getWagerAmount', 'getPlayerBalance', 'getWagerBetType']),

      isValid() {        
        try
        {
            return Number(this.amount) >= 1 && Number(this.amount) <= this.getPlayerBalance;
        } catch (e)
        {
            return false;
        }
      }
    },
    methods: {
        ...mapMutations(['setWagerAmount']),        
      submitBet() {
        if (this.isValid) {            

          alert(`Apuesta de ${this.getWagerAmount} realizada con éxito.`);
        }
      },
      validateAmount() {
        // Validación extra cuando el usuario cambia el valor
        if (this.amount < 1) {
          this.amount = 1;
        }
        if (this.amount > this.getPlayerBalance) {
            this.amount = this.getPlayerBalance;
        } 
        this.setWagerAmount(this.amount);   
      }
    }
  };
  </script>
  <style scoped>
  .container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    margin: 0.5vh;
  
  }
  
  .field-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;  
    margin: 2vh;
  
  }
  
  .field {
    width: 100%;
    padding: 8px;
    margin-top: 5px;
    border: 1px solid #ccc;
    border-radius: 4px;
  }

  .error-message {
  display: flex;
  align-items: center;
  font-size: 1.2vh; 
  height: 2vh;
}
  </style>

  