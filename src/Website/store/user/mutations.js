export default {
    setCurrentUser(state, user) {
        state.user = user;
    },
    setAccessInfo(state, accessInfo) {
        state.accessToken = accessInfo;
    },
    clearCurrentUser(state) {
        state.user = undefined
        state.accessToken = undefined
    }
};