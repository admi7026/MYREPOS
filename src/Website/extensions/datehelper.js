const moment = require("moment");
export default {
    toLocal: function(utcDate) {
        return moment.utc(utcDate).local().format()
    },
    show: function(utcDate) {
        return moment.utc(utcDate).local().format("DD-MM-YYYY HH:mm")
    }
}