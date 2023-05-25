var product = new Vue({
    el: "main",
    data: {
        product: [],
        buyQuantity: 1,
    },
    methods: {
        GetSingleProduct: function () {
            pId = window.location.search.replace('?', '').split('&')[0].split('=')[1];
            console.log(pId)
            axios.get(`/Retail/GetSingleProduct?productId=${pId}`).then(response => {
                console.log(response.data)
                this.product = response.data;
            }).catch(error => {
                console.log("抓不到產品資訊");
            });
        },
        minusItem: function () {
            if (this.buyQuantity > 1) {
                this.buyQuantity--;
            }
        },
        addItem: function () {
            if (this.buyQuantity < this.product.stockQuantity) {
                this.buyQuantity++;
            }
        },
    },


    mounted() {
        //this.GetSingleProduct(productId);
    },
});