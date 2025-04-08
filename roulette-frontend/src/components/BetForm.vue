<template>
    <div class="bet-form">
      <h2>Ingresa la cantidad que quieres apostar</h2>
  
      <!-- Campo para ingresar la cantidad de dinero -->
      <div class:bet-options>
        <label for="amount">Cantidad de Apuesta:</label>
        <input
          width="100%"
          type="number"
          id="amount"
          v-model="amount"
          :max="balance"
          :min="1"
          placeholder="Ingresa la cantidad"
          @input="validateAmount"
        />
        <p v-if="amount <= 1" class="error-message">La cantidad mínima es 1.</p>
        <p v-if="amount >= balance" class="error-message">La cantidad máxima es {{ balance }}.</p>
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
  

  