<template>
  <div class="main-panel">
    <div class="content">
      <div class="page-inner">
        <div class="page-header header-user-manager">
          <div
            class="d-flex align-items-left align-items-md-center flex-column flex-md-row w-100"
          >
            <div class="d-flex align-items-center">
              <h4 class="page-title">Order #{{ orderId }}</h4>
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
          </div>
        </div>
        <div class="row content-has-sidebar">
          <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="card">
              <div class="card-body">
                <p>
                  Notes: {{ dataSubmit.note }}
                </p>
                <p>
                  Date: {{ dataSubmit.orderDate | toShortDate }}
                </p>
                <p>
                  State: {{ dataSubmit.stateName }}
                </p>
                <table class="table table-hover table-common">
                  <tr class="mt-2">
                    <th>Product</th>
                    <th>Quantity</th>
                    <th>Price</th>
                  </tr>
                  <tr
                    v-for="(product, index) in dataSubmit.products"
                    :key="index"
                  >
                    <td>
                      <label for="">
                        {{ product.productName }}
                      </label>
                    </td>
                    <td>
                      <b-input v-model="product.quantity" readonly />
                    </td>
                    <td>
                      {{ product.price }}
                    </td>                    
                  </tr>
                </table>
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
export default {
  middleware: "authenticated",
  layout: "default",
  components: { Spinner, EmptyData },
  data() {
    return {
      isBusy: false,
      dataSubmit: {
        note: "",
        products: [],
      },
      products: [],
      productSelected: {},
      orderId: -1,
    };
  },
  created() {
    this.orderId = this.$route.query["id"];
    this.getOrderById();
  },
  methods: {
    async getOrderById() {
      this.isBusy = true;
      try {
        let response = await gatewayApi.getOrderById(this.$axios, this.orderId);
        this.dataSubmit = response;
      } catch {}
      this.isBusy = false;
    },
  },
};
</script>
