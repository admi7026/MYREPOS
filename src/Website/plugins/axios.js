export default function ({ store, redirect, app: { $axios } }) {
  $axios.onRequest((config) => {
    if (store.getters['user/isAuthenticated']) {
      config.headers.common.Authorization = `Bearer ${store.getters['user/accessToken']}`
    }
    if (store.getters.locale) {
      config.headers.common['Accept-Language'] = 'vi'
    }
  })
}