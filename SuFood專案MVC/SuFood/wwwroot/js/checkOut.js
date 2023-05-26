const vm = new Vue({
    el: "#app",
    data: {
        tabs: ['1.確認購物車', '2.填寫訂單資訊', '3.完成購買'],
        currentTab: '1.確認購物車',
        carts: [
            { 'id': 1, 'productName': '建達繽紛樂', 'quantity': 1, 'price': 1200, 'img': './resources/chocolate (3).jpg' },
            { 'id': 2, 'productName': '金莎巧克力', 'quantity': 1, 'price': 1200, 'img': './resources/chocolate (2).jpg' },
            { 'id': 3, 'productName': '金莎巧克力', 'quantity': 1, 'price': 1200, 'img': './resources/chocolate (2).jpg' },
        ],
        submit: {
            RecipientName: '',
            Email: '',
            Address: '',
            Phone: '',
        },
        isInvalid: true,
    },
    methods: {
        incrementQuantity(c) {
            if (c.quantity < 100) {
                c.quantity += 1
            }
        },
        decrementQuantity(c) {
            if (c.quantity > 0) {
                c.quantity -= 1
            }
        },
        DeleteProducts(id) {
            console.log(this.carts.map(x => x.id).indexOf(id))
            this.carts.splice(this.carts.map(x => x.id).indexOf(id), 1)
        }
    },
    computed: {
        totalPrice() {
            let subTotal = this.carts.map(product => product.quantity * product.price);
            if (this.carts.length > 0) {
                return subTotal.reduce((a, b) => a + b);
            }
        },
        validationForm() {
            if (this.submit.RecipientName === '123') {
                console.log('123');
                return false;
            }
        },
    }
})