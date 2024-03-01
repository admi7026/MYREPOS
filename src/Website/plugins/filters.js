import Vue from 'vue'
import common from '@/extensions/common'

Vue.filter('toShortDate', val => common.toShortDate(val))