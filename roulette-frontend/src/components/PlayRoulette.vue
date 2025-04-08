
<template>
  <div v-if="!isUpdatedBalance">
    <p> Conectando con una sala de juegos disponible </p> 
  </div>
  <div v-if="isUpdatedBalance">
    <div>
      <AccountBalanceDisplay/>
    </div>
    <div>
      <p>My Game</p>
      <BetForm :playerBalance="playerBalance" v-if="isUpdatedBalance"/>
    </div>
    <div >
      <RouletteTable/>      
      <p>{{validateWager()}}</p>
    </div>
    <div v-if="validateWager()">
      <h2>Esta es tu apuesta!:</h2>
      <h3>Monto a apostar: {{ this.getWagerAmount }}</h3>
      <div v-if="this.getWagerBetType === 'WagerFull'">
        <h3>Número: {{ this.getWagerNumber}}</h3>
        <h3>Color: {{ this.options[this.getWagerColor]}}</h3>  
      </div>
      <div v-if="['WagerOddsColor', 'WagerEvensColor'].includes(this.getWagerBetType) ">
        <h3>Número: {{ this.options[this.getWagerBetType] }} de color {{this.options[this.getWagerColor]}}</h3>        
      </div>      
      <div v-if="this.getWagerBetType === 'WagerOnlyColor'">
        <h3>Número: Cualquier número de color {{this.options[this.getWagerColor]}}</h3>        
      </div>
      <div>
        <button @click="startGame">Jugar!</button>
      </div>
    </div>
    <div v-if="!gotWinnings && isGameEnded">
      <h4>La ruleta aún esta rodando...</h4>
    </div>

    <div v-if="gotWinnings && isGameEnded">
      <div>
      <h1>Resultado: {{this.winnerNumber }} {{this.options[this.winnerColor]}}</h1>
    </div>      
      <h2 v-if="winnings[winnings.length-1] > 0"> Felicidades has ganado {{winnings[winnings.length-1]}} Euros!</h2>
      <h2 v-if="winnings[winnings.length-1] < 0"> No has ganado. Vuelve a intentarlo!. Perdiste {{ Math.abs(winnings[winnings.length-1]) }} Euros</h2>
      <div>
        <button @click="newGame">¿Quieres volver a jugar?</button>
      </div>
    </div>
  </div>
</template>
  
  <script>
  import BetForm from '../components/BetForm.vue';
  import RouletteTable from '../components/RouletteTable.vue';
  import AccountBalanceDisplay from '../components/AccountBalanceDisplay.vue';
  import { mapState, mapMutations, mapGetters } from 'vuex'

  export default {
    computed: {
    ...mapState(['player', 'sessionCredits', 'wagerBetType', 'wagerNumber', 'wagerColor', 'playerWinnings', 'playerAccountBalance'
      ,'playedGames'
    ]),
    ...mapGetters(['getUsername', 'getSessionCredits', 'getWagerBetType', 'getWagerAmount', 'getWagerNumber', 'getWagerColor', 'getPlayerBalance'
      ,'getPlayedGames'
    ])
    },  
    components: {
      BetForm, RouletteTable, AccountBalanceDisplay 
    },
    data() {
      return {
        username: this.getUsername,
        playerBalance: 0,
        isUpdatedBalance: false,
        winnerColor: null,
        winnerNumber: null,
        isGameEnded: null,
        gotWinnings: null,
        playedGames: Array(),
        winnings: Array(),
        options: {
          "Red": "Rojo",
          "Black": "Negro",
          "Green": "Verde",
          "WagerOddsColor": "Número Impar",
          "WagerEvensColor": "Número Par"
        }
      };
    },
    async mounted() {
      await this.loadUserData();
    },
    methods: {
      ...mapMutations(['setUsername', 'setSessionCredits', 'setWagerBetType', 
        'setWagerAmount', 'setWagerNumber', 'setWagerColor', 'setPlayerWinnings', 'setPlayerBalance',
        'setPlayedGames'
      ]),

      async loadUserData() {
        await fetch(`https://localhost:7004/api/v1.0/Players/${this.getUsername}`,
            {
              method: 'GET',
              headers: {
              'Content-Type': 'application/json',
              'Accept': 'application/json',
              'User-Agent': 'roulette-frontend/0.1',
              'Access-Control-Allow-Origin': '*',
              'Authorization': `${this.$ReadToken()}`
              }
            })
            .then(response =>{              
              if(response.status === 401){
                alert('Se ha cerrado la sesión');
                this.$router.push({ 
                name: 'Home' }); 
              }
              if (!this.$ValidateAPIResponse(response.status)) {              
                alert('Servicio no disponible. Intenta nuevamente más tarde.');                
                this.$router.push({ 
                name: 'Home' });                 
              }
              return response.json();
            })
            .then((body)=>{
              this.$SaveToken(this.getUsername, body.token);
              this.setPlayerBalance(this.getSessionCredits + Number(body.balance)??0);
              this.playerBalance = this.getPlayerBalance;              
              this.isUpdatedBalance = true;

              return;
            });
        
      },
      async startGame() {        
        await fetch('https://localhost:7004/api/v1.0/Games',
          {
            method: "POST",
            body: JSON.stringify({
              Username: this.getUsername,
              Amount: this.getWagerAmount,
              BetType: this.getWagerBetType,
              Number: this.getWagerNumber,
              Color: this.getWagerColor
            }),    
            headers: {
              'Content-Type': 'application/json',
              'Accept': 'application/json',
              'User-Agent': 'roulette-frontend/0.1',
              'Access-Control-Allow-Origin': '*',
              'Authorization': `${this.$ReadToken()}`
            }              
          }
        )
        .then(response => {
          if (!response.ok) {
            alert('Servicio no disponible. Intenta nuevamente más tarde.');                
                this.$router.push({ 
                name: 'Home' });         
          }
          return response.json();
        })
        .then((body)=> {
          this.$SaveToken(this.getUsername, body.token);
          this.winnerColor = body.color;
          this.winnerNumber = body.number;
          this.playedGames.push(body.gameId);
          this.setPlayedGames(body.gameId);
          this.isGameEnded = true;
          return;
        })
        await this.validateResults();
      },
      validateWager(){
        return [this.getUsername, this.getSessionCredits, this.getWagerBetType, 
        this.getWagerAmount , this.getWagerNumber, this.getWagerColor].every(Boolean);
      },
      async validateResults() {     
        
        const params = new URLSearchParams({
          player: this.getUsername
        }).toString();
        await fetch( 
          `https://localhost:7004/api/v1.0/Games/${this.playedGames[this.playedGames.length-1]}/Wagers?${params}`  
        ,
          {
            method: "GET",    
            headers: {
              'Content-Type': 'application/json',
              'Accept': 'application/json',
              'User-Agent': 'roulette-frontend/0.1',
              'Access-Control-Allow-Origin': '*',
              'Authorization': `${this.$ReadToken()}`
            }              
          }
        )
        .then(response => {
          if (!response.ok) {
            alert('Servicio no disponible. Intenta nuevamente más tarde.');                
                this.$router.push({ 
                name: 'Home' });         
          }
          return response.json();
        })
        .then((body)=> {
          this.$SaveToken(this.getUsername, body.token);
          this.winnings.push(Number(body.winnings));
          this.playerBalance += this.winnings[this.winnings.length-1];
          this.setPlayerWinnings(this.totalWinnings());
          this.setPlayerBalance(this.playerBalance);
          this.gotWinnings = true;
          return;
        })
       
      },
      newGame(){
        this.isGameEnded = null, 
        this.setWagerBetType(null), 
        this.setWagerAmount(0), 
        this.setWagerNumber(null), 
        this.setWagerColor(null)
      },
      totalWinnings(){
        return this.winnings.reduce((total, value) => total + value, 0);
      }
    },
  };
  </script>
  
  <style scoped>
  .juego {
    padding: 20px;
    text-align: center;
  }
  
  button {
    padding: 10px 20px;
    font-size: 16px;
    cursor: pointer;
  }
  </style>
  