import { reactive } from "vue";

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
    },
    checkLogout() {
        if (this.debug) {
            console.log("LOG OUT");
        }

        this.state.isLogin = false;
    },
};

export default store;
