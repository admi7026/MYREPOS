export default {
    isAuthenticated: (state, getters, rootState) => {
        if(typeof state.accessToken !== 'undefined'){
            return true;
        }else{
            return false;
        }
    },
    accessToken: (state, getters, rootState) => {
        return state.accessToken;
    },
    user: (state, getters, rootState) => {
        if(typeof state.user !== 'undefined'){
            return state.user;
        }else{
            return undefined;
        }
    },
    isAdmin: (state, getters, rootState) => {
        try {
          if (!getters.isAuthenticated) return false
          if (state.user.role === 1) return true
          return false
        } catch (error) {
          return false
        }
    }
};