<template>
  <div class="main-panel">
    <div class="content">
      <div class="page-inner">
        <div class="page-header header-user-manager">
          <div
            class="d-flex align-items-left align-items-md-center flex-column flex-md-row w-100"
          >
            <div class="d-flex align-items-center">
              <h4 class="page-title">Add order</h4>
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
                <ValidationObserver
                  ref="observer"
                  v-slot="{ invalid }"
                  tag="form"
                  @submit.prevent="onSubmit()"
                >
                  <validation-provider name="Note" rules="required|max:4000">
                    <b-form-group
                      slot-scope="{ valid, errors }"
                      label="Note (*)"
                      label-for="Note"
                      class="mb-3"
                    >
                      <b-form-textarea
                        id="Note"
                        v-model="dataSubmit.note"
                        trim
                        :state="errors[0] ? false : valid ? true : null"
                      ></b-form-textarea>
                      <b-form-invalid-feedback>
                        {{ errors[0] }}
                      </b-form-invalid-feedback>
                    </b-form-group>
                  </validation-provider>
                  <table class="table table-hover table-common">
                    <tr>
                      <td>
                        Select product:
                      </td>
                      <td colspan="2">                        
                        <b-form-select v-model="productSelected" :options="productOptions"></b-form-select>
                      </td>
                      <td>
                        <button type="button" class="btn btn-outline-primary" @click="addProduct">
                          Add Product
                        </button>
                      </td>
                    </tr>
                    <tr class="mt-2">
                      <th>Product</th>
                      <th>Quantity</th>
                      <th>Price</th>
                      <th>Action</th>
                    </tr>
                    <tr v-for="(product,index) in dataSubmit.products" :key="index">
                      <td>
                        <label for="">
                          {{ product.productName }}
                        </label>
                      </td>
                      <td>
                        <b-input v-model="product.quantity" />
                      </td>
                      <td>
                        {{ product.price }}
                      </td>
                      <td>
                        <button type="button" class="btn btn-sm btn-outline-danger">
                          remove
                        </button>
                      </td>
                    </tr>                    
                  </table>
                  <button
                    type="submit"
                    :disabled="invalid"
                    class="btn btn-primary"
                  >
                    Save
                  </button>
                </ValidationObserver>
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
      productSelected:{}
    };
  },
  computed: {
    productOptions(){
      let result = []
      this.products.forEach((item, index) => {
        result.push({
          value: item.id,
          text: item.productName
        })
      })
      return result
    }
  },
  created() {
    this.getProducts();
  },
  methods: {
    async getProducts() {
      this.isBusy = true;
      try {
        let response = await gatewayApi.getProducts(this.$axios);        
        this.products = response;
      } catch {}
      this.isBusy = false;
    },
    addProduct(){      
      let line = this.dataSubmit.products.filter(x => x.productId == this.productSelected)
      console.log(line)
      if(!line || line.length == 0){        
        let product = this.products.filter(x => x.id == this.productSelected)[0]
        
        this.dataSubmit.products.push({
          "productId": product.id,
          "productName": product.productName,
          "price": product.price,
          "quantity": 1
        })
      
      }
    },
    async onSubmit(){
      if (this.dataSubmit.products.length == 0){
        this.$bvToast.toast('Please add at least one product to the order', {
          title: "Error",
          variant: "danger",
          solid: true,
        });
        return false
      }
      
      try{
        let response = await gatewayApi.addOrder(this.$axios, this.dataSubmit)
        this.$router.push('/');
      }catch{
        this.$bvToast.toast('Please try again', {
          title: "Error",
          variant: "danger",
          solid: true,
        });
      }
    }
  },
};
</script>
