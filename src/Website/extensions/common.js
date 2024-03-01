const errorEnum = require('./errorEnum.json')
import Vue from 'vue'
const moment = require("moment");

export default {
    gioiTinh: function (gioiTinhId) {
        return gioiTinhId == 1 ? "Nam" : "Nữ";
    },
    noticeError: function (content) {
        const instance = new Vue({})
        instance.$bvToast.toast(content, {
            title: "Thông báo lỗi.",
            variant: "danger",
            solid: true
        });
    },
    noticeErrorCode: function (errorCode) {
        const instance = new Vue({})
        instance.$bvToast.toast(errorEnum[errorCode], {
            title: "Thông báo lỗi.",
            variant: "danger",
            solid: true
        });
    },
    noticeErrorByCode: function (err) {
        try {            
            let data = err.response.data;
            let errorCode = data.message;
            const instance = new Vue({})

            //lỗi validate
            if(errorCode === '00001000'){                
                data.object.forEach((item) => {                    
                    instance.$bvToast.toast(errorEnum[item.message], {
                        title: "Thông báo lỗi.",
                        variant: "danger",
                        solid: true
                    });
                });
            }else{
                instance.$bvToast.toast(errorEnum[errorCode], {
                    title: "Thông báo lỗi.",
                    variant: "danger",
                    solid: true
                });
            }            
        }
        catch{
            this.noticeError('Không thể thực hiện yêu cầu này. Vui lòng thử lại.')
        }

    },
    noticeSuccess: function (content) {
        const instance = new Vue({})
        instance.$bvToast.toast(content, {
            title: "Thông báo",
            variant: "success",
            solid: true
        });
    },
    toLocal: function (utcDate) {
        return moment.utc(utcDate).local().format()
    },
    show: function (utcDate) {
        return moment.utc(utcDate).local().format("DD-MM-YYYY HH:mm")
    },
    showNoTime: function (utcDate) {
        return moment.utc(utcDate).local().format("DD-MM-YYYY")
    },
    toShortDate: function (utcDate) {
        let d = moment.utc(utcDate).local();
        if(d.isValid()){
            return d.format("DD-MM-YYYY")
        }
        return "đến nay"
    },
    convertTimeURL: function (utcDate) {
        return moment.utc(utcDate).local().format("YYYY-MM-DD")
    },
    getYear: function (utcDate) {
        let d = moment.utc(utcDate).local();
        if(d.isValid()){
            return moment.utc(utcDate).local().format("YYYY")
        }
        return "đến nay"
    },
    getMonthYear: function (utcDate) {
        let d = moment.utc(utcDate).local();
        if(d.isValid()){
            return moment.utc(utcDate).local().format("MM/YYYY")
        }
        return "đến nay"
    },
    log: function (obj) {
        console.log(obj)
    }
}