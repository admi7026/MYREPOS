export default function({ store, redirect, route }) {
    if (store.getters['user/isAuthenticated'] === false) {
        return redirect('/login')
    }

    if (!store.getters['user/isAdmin']) {
        return redirect('/user/thong-tin')
    }
}