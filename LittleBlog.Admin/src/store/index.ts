import { reactive } from "vue";
import Cookie from "js-cookie";

const store = {
    debug: true,
    state: reactive({
        isLogin: false,
    }),

    checkLogin() {
        if (this.debug) {
            console.log("LOG IN");
        }

        this.state.isLogin = true;
        Cookie.set("login_status", "logined");
    },
    checkLogout() {
        if (this.debug) {
            console.log("LOG OUT");
        }

        this.state.isLogin = false;
        Cookie.remove("login_status");
    },
};

export default store;
