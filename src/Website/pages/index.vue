<template>
  <div class="main-panel">
    <div class="content">
      <div class="page-inner">
        <div class="page-header header-user-manager">
          <div
            class="d-flex align-items-left align-items-md-center flex-column flex-md-row w-100"
          >
            <div class="d-flex align-items-center">
              <h4 class="page-title">Orders</h4>
              <ul class="breadcrumbs">
                <li class="nav-home">
                  <a href="#">
                    <i class="fas fa-home"></i>
                  </a>
                </li>
                <li class="separator">
                  <i class="fas fa-chevron-right"></i>
                </li>
                <li class="nav-home">
                  <nuxt-link to="/">List orders</nuxt-link>
                </li>
              </ul>
            </div>
            <div class="ml-md-auto py-2 py-md-0 group-button-action">
              <nuxt-link class="btn btn-round btn-success" to="/add-order">
                <i class="fas fa-plus"></i> Add Order
              </nuxt-link>
              <button class="ml-2 btn btn-round btn-danger" type="button" @click="logOut">
                <i class="fas fa-exit"></i> Logout
              </button>
            </div>
          </div>
        </div>
        <div class="row content-has-sidebar">
          <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="card">
              <div class="card-body">
                <b-table
                  :fields="fields"
                  :items="items"
                  hover
                  :table-class="tbClass"
                  responsive
                  :busy="isBusy"
                  show-empty
                >
                  <template #table-busy>
                    <spinner />
                  </template>
                  <template #empty>
                    <empty-data />
                  </template>
                  <template #cell(index)="data">
                    {{ data.index + 1 }}
                  </template>

                  <template #cell(orderDate)="data">
                    {{ data.item.editDate | toShortDate }}
                  </template>

                  <template #cell(actions)="data">
                    <b-dropdown
                      variant="link"
                      toggle-class="text-decoration-none"
                      no-caret
                    >
                      <template v-slot:button-content>
                        <i class="fas fa-ellipsis-h"></i>
                      </template>
                      <b-dropdown-item>
                        <nuxt-link
                          :to="'/order?id=' + data.item.id"
                          variant="link"
                          class="btn-primary btn-round"
                        >
                          <i class="fas fa-edit"></i>
                          Detail
                        </nuxt-link>
                      </b-dropdown-item>
                    </b-dropdown>
                  </template>
                </b-table>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import gatewayApi from "~/api/gatewayApi";
import Spinner from "@/components/Spinner.vue";
import EmptyData from "@/components/EmptyData.vue";
import { mapActions } from "vuex";

export default {
  middleware: "authenticated",
  layout: "default",
  components: { Spinner, EmptyData },
  data() {
    return {
      items: [],
      isBusy: false,
      fields: [
        { key: "index", label: "#" },
        { key: "note", label: "Order" },
        { key: "total", label: "Total" },
        { key: "orderDate", label: "Order date" },
        { key: "stateName", label: "stateName" },
        { key: "actions", label: "Actions" },
      ],
      tbClass: ["table", "table-hover", "table-common"],
    };
  },
  created() {
    this.getOrders();
  },
  methods: {
    ...mapActions("user", ["clearCurrentUser"]),
    async getOrders() {
      this.isBusy = true;
      try {
        let response = await gatewayApi.getOrders(this.$axios);
        console.log(response);
        this.items = response;
      } catch {}
      this.isBusy = false;
    },
    async logOut() {
      this.clearCurrentUser();
      let redirect = "/login";
      this.$router.push(redirect);
    },
  },
};
</script>
