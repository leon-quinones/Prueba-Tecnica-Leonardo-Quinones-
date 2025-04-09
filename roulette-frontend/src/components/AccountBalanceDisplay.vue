<template>
  <div class="container">
    <div class="field-container">
      <h2 class="badge text-bg-primary" style="font-size: 2vh;"
      >Saldo total:
      </h2>
      <h2 class="">
        {{ this.getPlayerBalance }}
      </h2>
    </div>
    <div class="field-container">
      <h2 class="badge text-bg-secondary" style="font-size: 2vh;"
      >Balance partida:</h2>
      <h2>{{ this.getPlayerTotalWinnings }}</h2>
    </div>
    <div >
        <button class="btn btn-danger " @click="saveCredits"
        style="height: 10vh; width: 10vh;font-size: 2vh; text-align: center;"
        >Guardar partida</button>
    </div>  
  </div>
</template>

<script>
import { mapMutations } from 'vuex';
import { mapState, mapGetters } from 'vuex'
export default {

  computed: {
    ...mapState(['player', 'sessionCredits', 'playerAccountBalance', 'playerTotalWinnings', 'playedGames', 'appDomain']),
    ...mapGetters(['getUsername','getSessionCredits', 'getPlayerBalance','getPlayerTotalWinnings', 'getPlayedGames', 'getAppBaseUrl'])
    }, 
  methods: {
    ...mapMutations(['setSessionCredits']),
    async saveCredits() {
      await fetch(`${this.getAppBaseUrl}/Players/${this.getUsername}`,
        {
          method: 'GET',
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
      .then(body => {
        this.$SaveToken(this.getUsername, body.token);
        return  
      });
      await this.addSessionCredits();
      await this.updateGames();
    },
    async updateGames() {

      await fetch(`${this.getAppBaseUrl}/Wagers`,
        {
          method: 'PATCH',
          body: JSON.stringify({
            Username: this.getUsername,
            GameIds: this.getPlayedGames
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
            alert('Servicio no disponible. No se pudo guardar la partida');                
                this.$router.push({ 
                name: 'Home' });         
          }
          return response.json();
      })
      .then(body => {
        this.$SaveToken(this.getUsername, body.token);
        alert('Partida salvada exitosamente');
      });      
    },
    async addSessionCredits() {
      await fetch(`${this.getAppBaseUrl}/Players/${this.getUsername}`,
        {
          method: 'POST',
          body: JSON.stringify({
            Username: this.getUsername,
            Balance: this.getSessionCredits
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
      .then(body => {
        this.$SaveToken(this.getUsername, body.token);
        this.setSessionCredits(0);     
        return;   
      });
    }
  }
};
</script>

<style scoped>
.container {
  display: flex;
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

.submit-button {
  width: 100%;
  padding: 10px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
}

.submit-button:hover {
  background-color: #0056b3;
}
</style>