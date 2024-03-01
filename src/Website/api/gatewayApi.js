const configuration = require('@/app')
export default {
    login: function(axios, user) {
        let url = '/login';
        return axios.$post(url, user)
    },
    getOrders: function(axios){
        let url = '/orders'
        return axios.$get(url)
    },
    getProducts: function(axios){
        let url = '/products'
        return axios.$get(url)
    },
    addOrder: function(axios, data){
        let url = '/orders'
        return axios.$post(url, data)
    },
    getOrderById: function(axios,id){
        let url = `/orders/${id}`
        return axios.$get(url)
    },
}