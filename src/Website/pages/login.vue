<template>
  <div class="login-form">
    <h1 class="login-title text-center">Login</h1>
    <b-form @submit="dataSubmit">
      <div class="form-group">
        <label>User name</label>
        <input
          type="text"
          class="form-control"
          v-model="dataForm.userName"
          placeholder="Please enter a username"
        />
      </div>
      <div class="form-group">
        <label>Password</label>
        <input
          type="password"
          class="form-control"
          v-model="dataForm.password"
          placeholder="Please enter a password"
        />
      </div>
      <div class="form-check">
        <label class="form-check-label">
          <input class="form-check-input" type="checkbox" value />
          <span class="form-check-sign"></span>
          Remember
        </label>
      </div>
      <div class="container-login-form-btn">
        <button class="login-form-btn btn btn-block btn-primary">Submit</button>
      </div>
      <div class="text-center mt-3">
        <nuxt-link to="#" class="link">Forgot password?</nuxt-link>
      </div>
    </b-form>
  </div>
</template>
<script>
import gatewayApi from "~/api/gatewayApi";
import common from "@/extensions/common";
import { mapActions } from "vuex";
export default {
  middleware: "unauthenticated",
  layout: "login",
  data() {
    return {
      permissionGranted: false,
      dataForm: {
        userName: null,
        password: null,
      },
    };
  },
  created() {},
  methods: {
    ...mapActions("user", ["setCurrentUser", "setAccessInfo"]),
    //end notification
    async dataSubmit(evt) {
      evt.preventDefault(evt);
      try {
        let response = await gatewayApi.login(this.$axios, this.dataForm);
        console.log(response);
        this.setCurrentUser(response);
        this.setAccessInfo(response.access_token);
        let redirect = "/";
        this.$router.push(redirect);
      } catch (err) {
        common.log(err);
        common.noticeErrorByCode(err);
      }
    },
    makeToast(content) {
      this.$bvToast.toast(content, {
        title: "Login fail!",
        variant: "danger",
        solid: true,
      });
    },
  },
};
</script>
<style>
.form-check,
.form-group {
  margin-bottom: 15px !important;
}
</style>