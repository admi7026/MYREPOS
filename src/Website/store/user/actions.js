export default {
    setCurrentUser({ commit, state }, user) {
        commit('setCurrentUser', user);
    },
    setAccessInfo({ commit, state }, accessInfo) {
        commit('setAccessInfo', accessInfo);
    },
    clearCurrentUser({ commit, state }){
        commit('clearCurrentUser');
    }
};