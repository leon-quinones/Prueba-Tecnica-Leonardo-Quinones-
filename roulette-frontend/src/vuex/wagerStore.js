
import { createStore } from 'vuex';

const wagerStore = createStore({
  state() {
    return {
        player: null,       
        sessionCredits: 0,        
        wagerBetType: null,       
        wagerCredits: 0,
        wagerNumber: null,
        wagerColor: null,     
        playerAccountBalance: 0, 
        playerTotalWinnings: 0,
        playedGames: Array(),
        playerWinnings: Array()

    };
  },
  mutations: {

    setUsername(state, data) {
        state.player = data;
      },
      setSessionCredits(state, amount) {
        state.sessionCredits = amount;
      },

    setWagerBetType(state, data) {
      state.wagerBetType = data;
    },
    setWagerAmount(state, amount) {
      state.wagerCredits = amount;
    },
    setWagerNumber(state, number) {
        state.wagerNumber = number;
    },
    setWagerColor(state, color) {
    state.wagerColor = color;
    },       
    setPlayedGames(state, gameId) {
        state.playedGames.push(gameId) ;
    },    
    
    setPlayerWinnings(state, gameId) {
        state.playerWinnings.push(gameId) ;
    },  
    
    clearPlayedGames(state) {
        state.playedGames = Array() ;
    },    
    
    clearPlayerWinnings(state) {
        state.playerWinnings = Array() ;
    },       

    setPlayerTotalWinnings(state, value) {
        state.playerTotalWinnings = value;
    },
    setPlayerBalance(state, value) {
        state.playerAccountBalance = value;
    },         
  },
  getters: {

    getUsername(state) {
        return state.player;
    },
    getSessionCredits(state) {
        return state.sessionCredits;
    },    
    getWagerBetType(state) {
        return state.wagerBetType;
    },
    getWagerAmount(state) {
        return state.wagerCredits;
    },
    getWagerNumber(state) {
        return state.wagerNumber;
    },
    getWagerColor(state) {
        return state.wagerColor;
    },       
    getPlayedGames(state) {
        return state.playedGames;
    },      
    getPlayerWinnings(state) {
        return state.playerWinnings;
    },       

    getPlayerTotalWinnings(state) {
        return state.playerTotalWinnings;
    },           
    getPlayerBalance(state) {
        return state.playerAccountBalance;
    },      
  }
});

export default wagerStore;