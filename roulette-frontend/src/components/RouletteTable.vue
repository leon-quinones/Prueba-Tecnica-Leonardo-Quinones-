<template>
    <div class="roulette-container">
      <h2>Mesa de Ruleta</h2>
      
      <div class="roulette-table">
        <div
          v-for="(number, index) in numbers"
          :key="index"
          class="roulette-number"
          :class="{
            selected: selectedNumber === number,
            red: isRed(number),
            black: isBlack(number),
            green: number === 0
          }"
          @click="selectNumber(number)"
        >
          {{ number }}
        </div>
      </div>
  

    <div class=" container options-row-2">
        <div class="btn btn-warning" @click="selectRange('evens')"
        style="width: 100%;"
        >Par</div>
        <div class="btn btn-secondary" @click="selectRange('odds')"
        style="width: 100%;"
        >Impar</div>
        <div v-if="showOddsColorOptions" class="color-options-row">
           <p class="badge text-bg-light"
           style="display: flex; align-items: center; font-size: 1.4vh;"
           >Selecciona un color: {{ this.getWagerColor ? this.getWagerColor : 'Ninguno' }}</p>
          <div class="option red" @click="selectColor('Red')" >Rojo</div>
          <div class="option black" @click="selectColor('Black')">Negro</div>
        </div>
        <div class="options-row-2">
            <div class="btn btn-success" @click="selectOnlyColor"
            style="width: 100%;"
            >Color</div>
            <div v-if="showOnlyColorOptions" class="color-options-row">
            <p class="badge text-bg-light" 
            style="display: flex; align-items: center; font-size: 1.4vh;"
            >Selecciona un color: {{ this.getWagerColor ? this.getWagerColor : 'Ninguno' }}</p>
            <div class="option red" @click="selectColor('Red')" >Rojo</div>
            <div class="option black" @click="selectColor('Black')">Negro</div>
        </div>            
        </div>
    </div>
    
    

  
      <div v-if="selectedNumber !== null" class="container">
        <p v-if="selectedNumber>= 0"          
          >Has seleccionado el número: {{ this.getWagerNumber }} y color: {{ this.getWagerColor }}</p>
        <p v-if="selectedNumber === -1"
          
        >Has seleccionado apuesta con: {{ this.getWagerBetType }} y color: {{ this.getWagerColor }}</p>
      </div>
      <div v-if="getWagerBetType" class="container selected-info badge rounded-pill text-bg-primary" 
      style="display: flex; justify-content: center; align-items: center;"
      >
        <p>✨✨✨ Con esta apuesta podrás ganar {{ betTypes[this.getWagerBetType] }} el valor apostado ✨✨✨</p>        
      </div>      
    </div>
  </template>
  
  <script>
    import { mapState, mapMutations, mapGetters } from 'vuex'
  export default {
    computed: {
        ...mapState(['player', 'sessionCredits', 'wagerBetType', 'wagerNumber', 'wagerColor']),
        ...mapGetters(['getUsername', 'getSessionCredits', 'getWagerBetType', 'getWagerAmount', 'getWagerNumber', 'getWagerColor'])
    },
    data() {
      return {
        numbers: Array.from({ length: 37 }, (_, i) => i),
        selectedNumber: null, 
        selectedRange: null, 
        selectedColor: null, 
        betType: null, 
        betTypes: {
            'WagerOddsColor': '1x',
            'WagerEvensColor': '1x',
            'WagerOnlyColor': '0.5x',
            'WagerFull': '3x'

        },
        showOddsColorOptions: false,
        showOnlyColorOptions: false

      };
    },
    methods: {
        ...mapMutations(['setUsername', 'setSessionCredits', 'setWagerBetType', 
        'setWagerAmount', 'setWagerNumber', 'setWagerColor'
      ]),        
      hideColorOptions() {
        this.showOddsColorOptions = false,
        this.showOnlyColorOptions = false
      },
      selectNumber(number) {
        this.setWagerNumber(number);
        if (number !== 0) {
            this.setWagerColor(this.isRed(number) ? 'Red' : 'Black'); 
        } else {
            this.setWagerColor('Green');
        }
        this.setWagerBetType('WagerFull'); 
        this.hideColorOptions();
      },
  
      selectRange(range) {
        this.setWagerNumber(-1);
        this.selectedRange = range;
        this.showOddsColorOptions = true;
        this.showOnlyColorOptions = false;
        if (range === 'odds') {
            this.setWagerBetType('WagerOddsColor');
        } 
        if (range === 'evens') {
            this.setWagerBetType('WagerEvensColor');
        }
      },
  
      selectColor(color) {
        this.setWagerNumber(-1);
        this.setWagerColor(color);
        this.hideColorOptions()

      },

      selectOnlyColor() {
        this.setWagerNumber(-1);
        this.setWagerBetType('WagerOnlyColor');
        this.showOddsColorOptions = false;
        this.showOnlyColorOptions = true;        
      },
  
      isRed(number) {
        const redNumbers = [1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36];
        return redNumbers.includes(number);
      },
  
      isBlack(number) {
        const blackNumbers = [2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35];
        return blackNumbers.includes(number);
      },
  
      toggleColorOptions() {
        this.showColorOptions = !this.showColorOptions;
      }
    },
  };
  </script>
  
  <style scoped>
  .container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    margin: 1vh;
  
  }  
  .roulette-container {
    text-align: center;
    
  }
  
  .roulette-table {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: 10px;
    max-width: 500px;
    margin: 0 auto;
  }
  
  .roulette-number {
    width: 40px;
    height: 40px;
    display: flex;
    align-items: center;
    justify-content: center;
    background-color: #eaeaea;
    border: 1px solid #ccc;
    font-size: 16px;
    font-weight: bold;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s;
  }
  
  .roulette-number:hover {
    background-color: #ccc;
  }
  
  .selected {
    background-color: #ffcc00;
    color: white;
  }
  
  .even {
    background-color: #f2f2f2;
  }
  
  .odd {
    background-color: #eaeaea;
  }
  
  .red {
    background-color: #ff0000;
    color: white;
  }
  
  .black {
    background-color: #000000;
    color: white;
  }
  
  .green {
    background-color: #00cc00;
    color: white;
  }
  .options-row-2 {
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%; 
    }

  .options-row {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: 10px;
    margin-top: 20px;
  }  
  
  .color-options-row {
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 10px;
    margin-top: 10px;
  }
  
  .option {
    padding: 10px 20px;
    border: 1px solid #ccc;
    border-radius: 5px;
    cursor: pointer;
  }
  
  .option:hover {
    background-color: #ccc;
  }
  
  .selected-info {
    margin-top: 20px;
    font-size: 18px;
    font-weight: bold;
  }
  </style>
  