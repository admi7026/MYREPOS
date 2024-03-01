const configuration = require('./app')
const fs = require('fs');
const path = require('path');
require('dotenv').config({ path: 'environments/.env.' + process.env.NODE_ENV });

const IS_DEV = process.env.NODE_ENV !== 'production';

export default {   
    mode: 'spa',
    /*
     ** Headers of the page
     */
    head: {
        title: process.env.TITLE,
        meta: [
            { charset: 'utf-8' },
            { name: 'viewport', content: 'width=device-width, initial-scale=1' },
            { hid: 'description', name: 'description', content: process.env.TITLE }
        ],
        link: [
            { rel: 'icon', type: 'image/x-icon', href: '/icon.jpg' }
        ]
    },
    /*
     ** Customize the progress-bar color
     */
    loading: { color: '#fff' },
    /*
     ** Global CSS
     */
    css: [
        '~/assets/css/fonts.min.css',
        '~/assets/css/atlantis.min.css',
        '~/assets/css/custom.css',
        '~/assets/css/login.css'
    ],
    /*
     ** Plugins to load before mounting the App
     */
    plugins: [
        '@/plugins/axios',
        { src: `~plugins/autocomplete`, ssr: false },
        { src: "~plugins/validators", ssr: false },
		{ src: '~/plugins/filters.js', ssr: false },        
        { src: '~/plugins/vue-logger-plugin.js', ssr: false },
    ],
    /*
     ** Nuxt.js dev-modules
     */
    buildModules: [],
    /*
     ** Nuxt.js modules
     */
    modules: [
        // Doc: https://bootstrap-vue.js.org
        '@nuxtjs/axios',
        '@nuxtjs/dotenv',
        'bootstrap-vue/nuxt', ['nuxt-vuex-localstorage', {
            localStorage: ['user']
        }]       
    ],
    axios: {
        baseURL: process.env.API,
        proxyHeaders: false,
        credentials: false
    },
    server: {
        host: '0.0.0.0', // default: localhost,
        timing: false,
        port: process.env.PORT
    },    
    /*
     ** Build configuration
     */
    build: {
        publicPath: '/public/',
        /*
         ** You can extend webpack config here
         */
        extend(config, ctx) { },
        vendor: [],
    }
}